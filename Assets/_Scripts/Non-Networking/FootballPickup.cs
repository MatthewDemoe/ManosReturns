using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballPickup : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInChildren<ThrowTrigger>().SetPickup(gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInChildren<ThrowTrigger>().ResetPickup();
        }
    }

    public void PickUp()
    {
        Destroy(gameObject);
    }
}
