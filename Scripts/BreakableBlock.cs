using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    public GameObject tinyBlockPrefab;

    public void Break()
    {
        //create 3 blocks in random position
        for (int i = 0; i < 1; i++)
        {
            GameObject tinyBlock = Instantiate(tinyBlockPrefab);
            tinyBlock.transform.localPosition = randomPosition(transform.localPosition);
        }
        Destroy(gameObject); //self destruct
    }

    Vector3 randomPosition(Vector3 center)
    {
        return center + new Vector3(Random.Range(-.25f, .25f), Random.Range(-.4f, .4f), 0);
    }
}
