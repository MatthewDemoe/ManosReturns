using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHitResponse : MonoBehaviour
{
    FlashFeedback flash;

    // Start is called before the first frame update
    void Start()
    {
        flash = GetComponent<FlashFeedback>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
       // flash.ReactToDamage(0.0f, Enums.ManosParts.None);
    }
}
