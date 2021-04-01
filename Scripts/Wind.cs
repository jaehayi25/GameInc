using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public GameObject windParticlePrefab;
    public GameObject[] leafPrefabs;

    float TimeBetweenWind = 0.85f;
    float TimeBetweenLeaf = 0.5f;
    float TimeBetweenMove = 0.3f;

    GameObject Player;
    float windDirection;
    public List<Collider2D> windList = new List<Collider2D>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!windList.Contains(other)) {
            windList.Add(other); 
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        windList.Remove(other);
    }

    float WindStart = 0f;
    float LeafStart = 0f;
    float moveStart = 0f;

    float timeCharged = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        windDirection = Player.transform.localScale.x;
        transform.localScale = new Vector3(Player.transform.localScale.x, 1, 1);
        //GetComponent<BoxCollider2D>().Offset = new Vector2(GetComponent<BoxCollider2D>().Offset.x*2)
        WindStart = Time.time;
        LeafStart = Time.time;
        moveStart = Time.time;
    }

    List<GameObject> leafList = new List<GameObject>();
    Dictionary<GameObject, float> rotateSpeed = new Dictionary<GameObject, float>();

    float windTime = 0f;
    float windTickDuration = 1f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= WindStart)
        {
            WindStart = WindStart + TimeBetweenWind;
            GameObject windParticle = Instantiate(windParticlePrefab);
            windParticle.transform.localPosition = randomPosition(transform.localPosition + new Vector3(.1f * windDirection, 0, 0)); //TO DO: random position
            windParticle.transform.localScale = new Vector3(windDirection, 1f, 1f);
            windParticle.transform.SetParent(transform);
            StartCoroutine(DelayDestroy(windParticle, 1f));
        }
        if (Time.time >= LeafStart)
        {
            LeafStart = LeafStart + TimeBetweenLeaf;
            GameObject leaf = Instantiate(leafPrefabs[UnityEngine.Random.Range(0, leafPrefabs.Length)]);
            leaf.transform.localPosition = randomPosition(transform.localPosition); //TO DO: random position, make leaf move sideways
            leaf.transform.localScale = new Vector3(windDirection, 1f, 1f);
            leaf.transform.SetParent(transform);
            rotateSpeed[leaf] = UnityEngine.Random.Range(1.0f, -1.0f);
            leafList.Add(leaf);
            StartCoroutine(DelayDestroy(leaf, .9f));
        }
        foreach (GameObject leafObj in leafList)
        {
            leafObj.GetComponent<Rigidbody2D>().rotation += rotateSpeed[leafObj];
        }

        if (Time.time >= moveStart)
        {
            moveStart += TimeBetweenMove;
            foreach (GameObject leafObj in leafList)
            {
                leafObj.GetComponent<Rigidbody2D>().velocity = new Vector2(windDirection * UnityEngine.Random.Range(0.9f, 1.1f), UnityEngine.Random.Range(-.2f, .35f));
            }
        }

        if (Time.time >= windTime)
        {
            windTime = Time.time + windTickDuration;
            foreach (Collider2D pushable in windList)
            {
                if (pushable == null) continue;
                if (pushable.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    Enemy_HP hpScript = pushable.gameObject.GetComponent<Enemy_HP>();
                    if (hpScript != null) { hpScript.Damage(1); }
                }
            }
        }

        foreach (Collider2D pushable in windList)
        {
            if (pushable == null) continue;
            Rigidbody2D rb = pushable.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity += new Vector2(1.25f * windDirection, 0f);
                if (Mathf.Abs(rb.velocity.x) > 5)
                {
                    rb.velocity = new Vector2(5f * windDirection, 0f);
                }
            }
        }

    }

    IEnumerator DelayDestroy(GameObject obj, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        rotateSpeed.Remove(obj);
        leafList.Remove(obj);
        Destroy(obj);
    }

    Vector3 randomPosition(Vector3 wind_pos)
    {
        return wind_pos + new Vector3(-.1f * windDirection, UnityEngine.Random.Range(-.07f, .2f), 0);
    }

    public void setTimeCharged(float dt)
    {
        timeCharged = dt;
        if (timeCharged > 6f)
        {
            timeCharged = 6f;
        }
    }
}
