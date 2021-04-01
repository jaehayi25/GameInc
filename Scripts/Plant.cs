using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Plant : MonoBehaviour
{
    Rigidbody2D rb;
    public PhysicsMaterial2D friction_material;

    Animator animator;
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void enableFall(float direction)
    {
        //gameObject.layer = LayerMask.GetMask("Enemy");
        UnityEngine.Debug.Log(direction);
        if (gameObject.GetComponent<Rigidbody2D>() == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.sharedMaterial = friction_material; 
            //rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
        else
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("idle2"))
        {
            rb.rotation += 0.25f * direction;
        }
        else
        {
            rb.rotation += 6f * direction;
        }
        StartCoroutine(DelayDestroy(gameObject, 7f));
    }

    IEnumerator DelayDestroy(GameObject obj, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (obj != null) { Destroy(obj); }
    }

    int currHP = 2;
    public void Damage()
    {
        currHP--;
        if (currHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
