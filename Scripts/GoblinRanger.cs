using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinRanger : MonoBehaviour
{
    Animator animator;
    GameObject Player;
    public GameObject SpearPrefab;
    Enemy_HP hpScript;

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

    float timeUntilAttack = 0f;
    float attackCD = 3f;
    float throwTime = 0f;
    float timeUntilThrow = .36f;
    bool enableThrow = false;
    // Update is called once per frame
    void Update()
    {
        if (hpScript.died) { return; }
        float horizontal_velocity = 0f;
        float dir = (transform.position.x - Player.transform.position.x > 0 ? -1 : 1);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack") && !animator.GetCurrentAnimatorStateInfo(0).IsName("heal"))
        {
            float moveDir = dir;
            if (Mathf.Abs(transform.position.x - Player.transform.position.x) < 1)
            {
                moveDir *= -1;
                horizontal_velocity = moveSpeed * moveDir;
            }
            if (Mathf.Abs(transform.position.x - Player.transform.position.x) > 1.5)
            {
                horizontal_velocity = moveSpeed * moveDir;
            }
            animator.SetFloat("speed", Mathf.Abs(horizontal_velocity));
            rb.velocity = new Vector2(horizontal_velocity, rb.velocity.y);
            hpScript.checkFlip(moveDir);
        }
        if (Time.time > timeUntilAttack && Mathf.Abs(transform.position.x - Player.transform.position.x) >= 1 && Mathf.Abs(transform.position.x - Player.transform.position.x) <= 1.5) //attack
        {
            animator.SetTrigger("attack");
            timeUntilAttack = Time.time + attackCD;
            throwTime = Time.time + timeUntilThrow;
            enableThrow = true;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("heal"))
        {
            throwTime = Time.time + timeUntilThrow;
        }
        if (Time.time > throwTime && enableThrow && animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            enableThrow = false;
            GameObject spear = Instantiate(SpearPrefab, transform.position + new Vector3(0.283f * dir, 0.12f, 0), Quaternion.identity);
            //spear.transform.position = transform.position + new Vector3(0.183f, 0.061f, 0);
            if (spear != null) { spear.GetComponent<Rigidbody2D>().velocity = new Vector2(3.5f * dir, 1f); }
        }
    }

}
