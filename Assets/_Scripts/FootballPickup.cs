﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballPickup : MonoBehaviour
{
    bool _pickedUp = false;

    [SerializeField]
    GameObject particleEffect;

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnTriggerStay(Collider other)
    {
        if (!_pickedUp)
        {
            if (other.gameObject.tag == "Player")
            {
                if (other.gameObject.GetComponentInChildren<ThrowTrigger>().PickUpFootball())
                {
                    GetComponentInChildren<Animator>().SetTrigger("PickupTrigger");
                    Destroy(Instantiate(particleEffect, transform), 1.0f);
                    Destroy(gameObject, 1.0f);
                    _pickedUp = true;
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
       
    }
}
