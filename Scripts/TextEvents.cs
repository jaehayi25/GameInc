using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEvents : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Enemy_HP hpScript;
    public Dialog dialogScript;
    bool event1 = false;

    // Update is called once per frame
    void Update()
    {
        //check event #1
        if (hpScript.died && !event1)
        {
            event1 = true;
            StartCoroutine(dialogScript.Display("Leave! There is no key here.", hpScript.gameObject, 2f));
        }
    }

}
