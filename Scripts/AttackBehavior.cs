using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : MonoBehaviour
{
    GameObject Player;
    Hearts HeartScript;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        HeartScript = GameObject.Find("Main Camera").GetComponent<Hearts>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Player)
        {
            HeartScript.Damage();
            //Debug.Log("Hit!!!");
        }
        if (other.gameObject.tag == "Plant")
        {
            other.gameObject.GetComponent<Plant>().Damage();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
