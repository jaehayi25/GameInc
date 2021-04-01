using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StalactiteSpawner : MonoBehaviour
{
    public Stalactite[] stalactites;
    public int[] dy;

    public List<Stalactite> stalactite_list;
    // Start is called before the first frame update
    void Start()
    {
        stalactite_list = new List<Stalactite>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        bool isEmpty = !stalactite_list.Any();
        if (isEmpty)
        {
            for (float x = -2f; x <= 2f; x += 1.33f)
            {
                float dx = Random.Range(-0.5f, 0.5f);
                int index = Random.Range(0, stalactites.Length);
                Stalactite spawned = Instantiate(stalactites[index], transform.position + new Vector3(x + dx, dy[index], 0f), Quaternion.identity);
                stalactite_list.Add(spawned);
            }
        }
        
    }
}
