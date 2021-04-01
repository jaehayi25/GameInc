using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    float textSpeed = 0.05f;
    Text DisplayBox;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        DisplayBox = gameObject.GetComponent<Text>();
        StartCoroutine(DelayDisable(0f));
        player = GameObject.Find("Player");
    }


    GameObject speaker;
    string prev_words;

    void Update()
    {
        if (speaker != null)
        {
            if (!displayed && Mathf.Abs(player.transform.position.x - speaker.transform.position.x) < 0.6f && Mathf.Abs(player.transform.position.y - speaker.transform.position.y) < 1f)
            {
                StartCoroutine(Display(prev_words, speaker, 0));
            }
        }
    }

    bool displayed = false;

    public IEnumerator Display(string s, GameObject obj, float waitTime)
    {
        displayed = true;
        yield return new WaitForSeconds(waitTime);
        Enable();
        speaker = obj;
        prev_words = s;
        StartCoroutine(Type(s));
        float dx = obj.transform.localScale.x > 0 ? .65f : -.15f;
        gameObject.transform.position = obj.transform.position + new Vector3(dx, .40f, 0);
        StartCoroutine(DelayDisable(5f));
    }

    IEnumerator Type(string sentence)
    {
        foreach (char letter in sentence.ToCharArray())
        {
            DisplayBox.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    IEnumerator DelayDisable(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        displayed = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            DisplayBox.text = "";
            DisplayBox.enabled = false;
        }
    }

    void Enable()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            DisplayBox.enabled = true;
        }
    }
}
