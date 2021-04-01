using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinFighter : MonoBehaviour
{
    Animator animator;
    GameObject Player;
    Enemy_HP hpScript;

    //Movement related variables
    public float moveSpeed = 1.5f;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        hpScript = GetComponent<Enemy_HP>();
    }

    float TimeUntilAttack = 0f;
    float attackCD = 3f;

    float moveDuration = 2f;
    float startPunchTime = 0f;
    float prevPos = 0f;

    // Update is called once per frame
    void Update()
    {
        if (hpScript.died) { return; }
        if (Time.time > startPunchTime)
        {
            startPunchTime = Time.time + moveDuration;
            if (Mathf.Abs(prevPos - transform.position.x) < .01)
            {
                if (Time.time > TimeUntilAttack) {
                    animator.SetTrigger("attack");
                    TimeUntilAttack = Time.time + attackCD;
                }
            }
            prevPos = transform.position.x;
        }
        float dir = (transform.position.x - Player.transform.position.x > 0 ? -1 : 1);
        float horizontal_velocity = 0f;
        if (Time.time > TimeUntilAttack && !animator.GetCurrentAnimatorStateInfo(0).IsName("heal"))
        {
            horizontal_velocity = dir * moveSpeed;
            rb.velocity = new Vector2(horizontal_velocity, rb.velocity.y);
            hpScript.checkFlip(dir);
        }
        animator.SetFloat("speed", Mathf.Abs(horizontal_velocity));
        if (Mathf.Abs(transform.position.x - Player.transform.position.x) < 0.5 && Time.time > TimeUntilAttack)
        {
            animator.SetTrigger("attack");
            TimeUntilAttack = Time.time + attackCD;
        }
        /*
        else if (Time.time > startPunchTime && Time.time > TimeUntilAttack) //Haven't moved for 2 seconds
        {
            animator.SetTrigger("attack");
            TimeUntilAttack = Time.time + attackCD;
        }
        */
    }
}
