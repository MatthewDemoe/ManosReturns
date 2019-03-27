using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BubbleScript : MonoBehaviour
{
    //public GameObject obj;
    DialogManager man;
    string text;
    public bool canUpdate;
    Text textObj;
    // Start is called before the first frame update
    void Start()
    {
        canUpdate = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("One");
      //  if (canUpdate)
        //{
            Debug.Log("Two");
            if (!man.IsSpeaking())
            {
                Debug.Log("Three");
                Destroy(gameObject, 0);
            }
       // }
    }


    public void SetText(DialogManager manager, string txt)
    {
        man = manager;
        text = txt;
        textObj = GetComponentInChildren<Text>();
        textObj.text = text;
        canUpdate = true;
    }
    public void BeginUpdate()
    {
        canUpdate = true;
    }
    bool CheckUpdate()
    {
        return canUpdate;
    }
}
