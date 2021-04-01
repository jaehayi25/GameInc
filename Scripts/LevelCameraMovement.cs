using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCameraMovement : MonoBehaviour
{
    Camera cam; 
    float halfHeight, halfWidth;
    GameObject Player; 

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main; 
        halfHeight = cam.orthographicSize;
        halfWidth = cam.orthographicSize * cam.aspect;
        //Debug.Log(halfHeight);
        Player = GameObject.Find("Player"); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.transform.localPosition.x > transform.localPosition.x + halfWidth)
        {
            transform.localPosition += new Vector3(halfWidth * 2, 0, 0); 
        }
        if (Player.transform.localPosition.x < transform.localPosition.x - halfWidth)
        {
            transform.localPosition -= new Vector3(halfWidth * 2, 0, 0);
        }
        if (Player.transform.localPosition.y < transform.localPosition.y - halfHeight)
        {
            transform.localPosition -= new Vector3(0, halfHeight * 2, 0);
        }
        if (Player.transform.localPosition.y > transform.localPosition.y + halfHeight)
        {
            transform.localPosition += new Vector3(0, halfHeight * 2, 0);
        }
    }
}
