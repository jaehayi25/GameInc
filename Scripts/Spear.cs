using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject != null && other.gameObject.tag != "Wind") {
            Destroy(gameObject); 
        }
    }

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float angle = Mathf.Rad2Deg* Mathf.Atan(rb.velocity.y / rb.velocity.x);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

}
