using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinChief : MonoBehaviour
{
    Animator animator;
    public Animator[] minions;

    public GameObject PoisonPrefab;
    GameObject Player;
    Enemy_HP hpScript;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        animator = gameObject.GetComponent<Animator>();
        hpScript = GetComponent<Enemy_HP>();
    }

    float timeUntilSpell = 0f;
    float spellCD = 5f;
    // Update is called once per frame
    void Update()
    {
        if (hpScript.died) { return; }
        float dir = (transform.position.x - Player.transform.position.x > 0 ? -1 : 1);
        hpScript.checkFlip(dir);
        if (Time.time > timeUntilSpell)
        {
            timeUntilSpell = Time.time + spellCD;
            int mode = Random.Range(0, 1); //heal, poison, rally
            if (mode == 0)
            {
                animator.SetTrigger("heal");
                StartCoroutine(DelayHeal(0.75f));
            }
            if (mode == 1)
            {
                animator.SetTrigger("poison");
                StartCoroutine(DelayPoison(0.75f));
            }
            if (mode == 2)
            {
                animator.SetTrigger("rally");
            }
        }
    }

    IEnumerator DelayHeal(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        foreach (Animator anim in minions)
        {
            if (anim == null || !anim.gameObject.activeInHierarchy || anim.gameObject.GetComponent<Enemy_HP>().died) continue;
            anim.SetTrigger("heal");
            anim.gameObject.GetComponent<Enemy_HP>().RestoreHP(5);
        }
    }

    IEnumerator DelayPoison(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GameObject poison = Instantiate(PoisonPrefab);
        poison.transform.position = Player.transform.position + new Vector3(0, .4f, 0);
        StartCoroutine(DelayDestroy(poison, 1.2f));
    }

    IEnumerator DelayDestroy(GameObject obj, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (obj != null)
        {
            Destroy(obj);
        }
    }

    
}
