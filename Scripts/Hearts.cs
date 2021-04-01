using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hearts : MonoBehaviour
{
    public GameObject heartPrefab;
    List<Vector3> positions = new List<Vector3>();
    List<GameObject> heartList = new List<GameObject>();
    GameObject Player;
    Animator animator;
    SpriteRenderer sp;
    public GameObject DeathPanel;

    public int maxHP;
    int currHP;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        sp = Player.GetComponent<SpriteRenderer>();
        animator = Player.GetComponent<Animator>();

        currHP = maxHP;
        Vector3 startPos = new Vector3(-3f, -1.9f, 0);
        Vector3 change = new Vector3(0.1f, 0, 0);
        for (int i = 0; i < maxHP; i++)
        {
            positions.Add(startPos + change * i);
            GameObject heart = Instantiate(heartPrefab);
            heart.transform.localPosition = positions[i];
            heart.transform.SetParent(transform);
            heartList.Add(heart);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool died = false;

    public void Damage()
    {
        if (died) return;
        currHP--;
        Destroy(heartList[currHP]);
        heartList.RemoveAt(currHP);
        sp.color = Color.red;
        StartCoroutine(DelayWhite(0.1f));

        if (currHP == 0) //Trigger Death
        {
            died = true;
            animator.SetTrigger("death");
            Rigidbody2D rb = Player.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(0, 0);
            rb.isKinematic = true;
            sp.sortingOrder = 101;
            DeathPanel.SetActive(true);
            StartCoroutine(DelayLoad(4f));
        }
    }

    IEnumerator DelayLoad(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator DelayWhite(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        sp.color = Color.white;
    }
}