using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode attack;
    public KeyCode spell;
    public KeyCode windKey;
    public KeyCode shift;

    public float moveSpeed = 1.5f;

    Animator animator;
    Rigidbody2D rb;
    Hearts hpScript;

    bool human = true;

    public GameObject plantPrefab;
    public GameObject windPrefab;

    [SerializeField]
    public LayerMask whatIsGround;
    [SerializeField]
    public LayerMask whatIsEnemy;
    [SerializeField]
    public Transform groundCheck;
    bool isGrounded = false;
    //bool wasGrounded = true;
    float groundCheckRadius = 0.005f;

    [SerializeField]
    public LayerMask whatIsWall;
    public Transform wallCheck;
    bool onWall = false;
    float wallCheckRadius = 0.01f;

    float jumpForce = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        hpScript = GameObject.Find("Main Camera").GetComponent<Hearts>();
    }

    string[] nonMoveAnimations = { "attack", "spell", "transform", "transform_back" };
    bool isJumping = true;
    GameObject plant;
    GameObject wind;
    bool allowMove = true;

    float timeUntilWind = 0f;
    float windCD = 12f;

    bool smallPlant = false;
    float startWind = 0f;

    // Update is called once per frame
    void Update()
    {
        if (hpScript.died) { return;  }
        //handle jump
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround) || Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsEnemy);
        if (isGrounded && Input.GetKey(KeyCode.W) && isJumping && (animator.GetCurrentAnimatorStateInfo(0).IsName("idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("run"))) {
            animator.SetTrigger("jump");
            isJumping = false;
            allowMove = false;
        }
        if (!isJumping && animator.GetCurrentAnimatorStateInfo(0).IsName("jump"))
        {
            rb.velocity = new Vector2(0, jumpForce);
            isJumping = true;
            allowMove = true;
        }
        /*
        if (isJumping && !wasGrounded && isGrounded)
        {
            animator.SetTrigger("grounded");
        }
        wasGrounded = isGrounded; 
        */

        //handle snake climb
        float climb_speed = 1f;
        onWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);
        if (onWall && Input.GetKey(KeyCode.W) && !human)
        {
            rb.velocity = new Vector2(0, climb_speed);
            animator.SetBool("climb", true);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("snake_climb") && !onWall)
        {
            animator.SetBool("climb", false);
        }

        //handle movement
        float horizontal_velocity = Input.GetAxis("Horizontal") * moveSpeed;
        foreach (string s in nonMoveAnimations)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(s))
            {
                horizontal_velocity = 0;
            }
        }
        if (!allowMove) horizontal_velocity = 0;
        rb.velocity = new Vector2(horizontal_velocity, rb.velocity.y);
        checkFlip(horizontal_velocity);
        animator.SetFloat("speed", Mathf.Abs(horizontal_velocity));

        //spells, attacks, etc
        if (Input.GetKey(attack) && (animator.GetCurrentAnimatorStateInfo(0).IsName("idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("run")))
        {
            animator.SetTrigger("attack");
        }
        if (Input.GetKey(spell) && (animator.GetCurrentAnimatorStateInfo(0).IsName("idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("run"))) //plant grow
        {
            animator.SetTrigger("plant");

            bool inRange = false;
            Vector3 new_pos = transform.localPosition + new Vector3(transform.localScale.x * 0.8f, -0.08f, 0);
            if (plant != null)
            {
                if (Mathf.Abs(new_pos.x - plant.transform.position.x) < .05 && Mathf.Abs(new_pos.y - plant.transform.position.y) < .05 && smallPlant) //within range of current plant
                {
                    plant.GetComponent<Animator>().SetTrigger("big");
                    inRange = true;
                    smallPlant = false;
                }
                else
                {
                    Destroy(plant);
                    smallPlant = false;
                }
            }
            //checking ground
            if (!inRange)
            {
                Vector3 root_pos = transform.localPosition + new Vector3(transform.localScale.x * 0.8f, -.38f, 0);
                Collider2D plant_ground = Physics2D.OverlapCircle(root_pos, groundCheckRadius, whatIsGround);
                if (plant_ground && plant_ground.gameObject.tag != "Ungrowable") //plant touching ground
                {
                    plant = Instantiate(plantPrefab);
                    plant.transform.localPosition = new_pos;
                    smallPlant = true;
                }
            }
        }
        if (Input.GetKey(windKey) && Time.time > timeUntilWind && (animator.GetCurrentAnimatorStateInfo(0).IsName("idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("run")))
        {
            animator.SetTrigger("wind");
            startWind = Time.time;
        }
        if (Input.GetKeyUp(windKey) && Time.time > timeUntilWind && (animator.GetCurrentAnimatorStateInfo(0).IsName("wind") || animator.GetCurrentAnimatorStateInfo(0).IsName("spell_charge")))
        {
            animator.SetTrigger("charge_stop");
            if (wind != null) { Destroy(wind); }
            wind = Instantiate(windPrefab);
            wind.transform.localPosition = transform.localPosition + new Vector3(transform.localScale.x * 0.5f, 0, 0);
            StartCoroutine(DelayDestroy(wind, 6f));
            timeUntilWind = Time.time + windCD;
        }
        if (Input.GetKey(shift) && (animator.GetCurrentAnimatorStateInfo(0).IsName("idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("snake_idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("run") || animator.GetCurrentAnimatorStateInfo(0).IsName("snake_move")))
        {
            //TODO: Check ceiling when snake turns back into human
            human = !human;
            animator.SetBool("human", human);
        }
    }

    bool facingRight = true;

    void checkFlip(float vel)
    {
        if (facingRight && vel < 0)
        {
            facingRight = false;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        if (!facingRight && vel > 0)
        {
            facingRight = true;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    void createPlant(Vector3 root_pos, Vector3 new_pos)
    {

    }

    IEnumerator DelayDestroy(GameObject obj, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (obj != null)
        {
            Destroy(obj);
        }
    }
}
