using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HP : MonoBehaviour
{
    public int maxHP;
    public bool facingRight;
    int currHP;

    Animator animator;
    SpriteRenderer sp;
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        currHP = maxHP;
        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Player = GameObject.Find("Player");
    }

    public bool died = false;
    public float deathDuration;

    public void Damage(int amt)
    {
        if (died) return;
        currHP -= amt;
        sp.color = Color.red;
        StartCoroutine(DelayWhite(0.1f));
        Debug.Log(currHP);
        if (currHP <= 0) //Trigger Death
        {
            died = true;
            float dir = (transform.position.x - Player.transform.position.x > 0 ? -1 : 1);
            checkFlip(dir);
            animator.SetTrigger("death");
            //disable boxcollier

            //if (deathDuration != -1) StartCoroutine(DelayDestroy(gameObject, deathDuration));
        }
    }

    public void RestoreHP(int amt)
    {
        currHP = Mathf.Min(maxHP, currHP+amt);
        Debug.Log(gameObject+" "+currHP);
    }

    IEnumerator DelayDestroy(GameObject obj, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (obj != null)
        {
            Destroy(obj);
        }
    }

    IEnumerator DelayWhite(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        sp.color = Color.white;
    }

    public void checkFlip(float vel)
    {
        if (facingRight && vel < 0)
        {
            facingRight = false;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        if (!facingRight && vel > 0)
        {
            facingRight = true;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    public int getHP()
    {
        return currHP;
    }
}
