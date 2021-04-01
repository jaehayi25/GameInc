using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public float moveSpeed = 1f; 
    GameObject Boss;
    Rigidbody2D rb;
    Hearts HeartScript;

    float damageCD = 1f;
    float timeUntilDmg = 0f;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 12)
        {
            if (Time.time > timeUntilDmg)
            {
                timeUntilDmg = Time.time + damageCD;
                HeartScript.Damage();
            }
            //Destroy(gameObject);
        }
        if (other.gameObject.layer == 14)
        {
            Destroy(gameObject);
        }
    }

    float dir = 0;
    // Start is called before the first frame update
    void Start()
    {
        Boss = GameObject.Find("TheBoss");
        rb = GetComponent<Rigidbody2D>();
        HeartScript = GameObject.Find("Main Camera").GetComponent<Hearts>();
        dir = (transform.position.x - Boss.transform.position.x > 0 ? 1 : -1);
        transform.localScale = new Vector3(transform.localScale.x * dir * -1, transform.localScale.y, transform.localScale.z);
    }

    float horizontal_velocity; 
    // Update is called once per frame
    void Update()
    {
        dir = (transform.position.x - Boss.transform.position.x > 0 ? 1 : -1);
        horizontal_velocity = dir * moveSpeed;
        rb.velocity = new Vector2(horizontal_velocity, rb.velocity.y);
    }

}
