using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    GameObject Player;
    float moveSpeed = 1.4f; //Player move speed = 1.5
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Move();
        transform.position = new Vector3(Player.transform.position.x, transform.position.y, transform.position.z);
    }

    void Move()
    {
        //float direction = (Player.transform.position.x > transform.position.x ? 1 : -1);
        transform.position = Vector3.Lerp(transform.position, new Vector3(Player.transform.position.x, transform.position.y, transform.position.z), Time.deltaTime * moveSpeed);
    }
}
