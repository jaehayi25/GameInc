using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class GroundAttack : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Plant")
        {
            //UnityEngine.Debug.Log("Plant detected!");
            float dir = ((other.gameObject.transform.position.x - transform.position.x) > 0 ? -1f : 1f);
            other.gameObject.GetComponent<Plant>().enableFall(dir);
        }
        if (other.gameObject.tag == "Breakable")
        {
            UnityEngine.Debug.Log("Break!");
            other.gameObject.GetComponent<BreakableBlock>().Break();
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy_HP hpScript = other.gameObject.GetComponent<Enemy_HP>();
            if (hpScript != null) { hpScript.Damage(5); }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
