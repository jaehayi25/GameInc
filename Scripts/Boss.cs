using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Animator animator;
    GameObject Player;
    Enemy_HP hpScript;
    public StalactiteSpawner stalactiteScript; 

    public float moveSpeed = 1.5f;
    Rigidbody2D rb;

    public CameraShake cameraShaker;

    string[] slam_animation = new string[] { "Slam1", "Slam2", "Slam3", "Slam4", "Slam5", "Slam6" };
    string[] pound_animation = new string[] { "Pound1", "Pound2", "Pound3", "Pound4", "Pound5", "Pound6" };
    /*
    string[] walk_animation = new string[] { "Walk1", "Walk2", "Walk3", "Walk4", "Walk5", "Walk6" };
    string[] slam_animation = new string[] { "Slam1", "Slam2", "Slam3", "Slam4", "Slam5", "Slam6" };
    string[] pound_animation = new string[] { "Pound1", "Pound2", "Pound3", "Pound4", "Pound5", "Pound6" };
    */

    public GameObject rockPrefab; 

    int hp;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        animator = gameObject.GetComponent<Animator>();
        hpScript = GetComponent<Enemy_HP>();
        rb = GetComponent<Rigidbody2D>();
    }

    bool walk = false;

    float timeUntilWalkStop = 0f;
    float walkDuration = 4f;
    float dir = 1;

    float timeUntilAction = 0f;
    float actionCD = 2f; 

    float timeUntilSlam = 0f;
    float slamCD = 7f;

    float timeUntilNormal = 0f;
    float normalCD = 2.5f;

    float timeUntilPound = 0f;
    float poundCD = 3f;

    int mode; 

    // Update is called once per frame
    void Update()
    {
        if (hpScript.died) { return; }
        hp = hpScript.getHP();
        animator.SetInteger("health", hp);

        bool inPound = false;
        for (int i = 0; i < 6; i++)
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(pound_animation[i]))
            {
                inPound = true;
                break;
            }
        if (Mathf.Abs(transform.position.x - Player.transform.position.x) > 0.25 && !inPound)
        {
            dir = (transform.position.x - Player.transform.position.x > 0 ? -1 : 1);
            hpScript.checkFlip(dir);
        }
        //Slam Condition: Facing same direction and in range of slam
        if (Time.time > timeUntilSlam && Mathf.Abs(transform.position.x - Player.transform.position.x) <= 1.2 && (transform.position.x - Player.transform.position.x)*dir < 0)
        {
            walk = false;
            animator.SetBool("walk", false);
            timeUntilSlam = Time.time + slamCD;
            timeUntilNormal = Time.time + normalCD;
            animator.SetTrigger("slam");
            //StartCoroutine(cameraShaker.DelayShake(1.5f, 0.1f, 0.02f));
        }

        bool inSlam = false; 
        for (int i = 0; i < 6; i++)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(slam_animation[i]))
            {
                inSlam = true;
                break; 
            }
        }
        if (!inSlam)
        {
            float horizontal_velocity = 0f;
            if (walk == false && Time.time > timeUntilAction)
            {
                timeUntilAction = Time.time + actionCD;
                mode = Random.Range(0, 3);
                bool isEmpty = !stalactiteScript.stalactite_list.Any();
                if (!isEmpty) { mode = 0; }
                //Pound
                if (Time.time > timeUntilPound && (mode == 0 || mode == 1))
                {
                    timeUntilPound = Time.time + poundCD;
                    animator.SetTrigger("pound");
                    //StartCoroutine(cameraShaker.DelayShake(1.5f, 0.1f, 0.05f));
                    if (mode == 0)
                    {
                        stalactiteScript.Spawn();
                        //Debug.Log("Stalactites!");
                    }
                    if (mode == 1)
                    {
                        //Debug.Log("Ground Wave!");
                    }
                }
                else
                {
                    walk = true;
                    animator.SetBool("walk", true);
                    timeUntilWalkStop = Time.time + walkDuration;
                }
            }
            if (walk == true)
            {
                horizontal_velocity = dir * moveSpeed;
                rb.velocity = new Vector2(horizontal_velocity, rb.velocity.y);
                //If walked for 5 seconds, stop walking 
                if (Time.time > timeUntilWalkStop)
                {
                    walk = false;
                    animator.SetBool("walk", false);
                }
            }
        }
    }

    void ShakeScreen()
    {
        StartCoroutine(cameraShaker.Shake(0.1f, 0.03f));
    }

    void SpawnRock()
    {
        if (mode == 1)
        {
            Instantiate(rockPrefab, transform.position + new Vector3(-0.7f * dir, -0.4f, 0f), Quaternion.identity);
            Instantiate(rockPrefab, transform.position + new Vector3(1f * dir, 0f, -0.4f), Quaternion.identity);
        }
        if (mode == 0)
        {
            ActivateFall();
        }
    }

    void ActivateFall()
    {
        foreach (Stalactite obj in stalactiteScript.stalactite_list) {
            obj.Fall(); 
        }
        stalactiteScript.stalactite_list.Clear();

    }
}
