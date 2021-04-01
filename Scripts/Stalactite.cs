using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    GameObject Player;
    Hearts HeartScript;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        Player = GameObject.Find("Player");
        HeartScript = GameObject.Find("Main Camera").GetComponent<Hearts>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Player)
        {
            HeartScript.Damage();
        }
        if (other.gameObject == Player || other.gameObject.layer == 8)
        {
            Destroy(GetComponent<PolygonCollider2D>());
            animator.SetTrigger("fade");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fall()
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
    }

    public void Destroy()
    {

        Destroy(gameObject);
    }
}
