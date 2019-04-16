using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    bool _pickedUp = false;

    [SerializeField]
    float healAmount = 100.0f;
    [SerializeField]
    GameObject particleEffect;


    public void OnTriggerStay(Collider other)
    {
        if (!_pickedUp)
        {
            if (other.gameObject.tag == "Player")
            {
                if (!other.gameObject.GetComponentInChildren<PlayerHealth>().IsFullHealth())
                {
                    other.gameObject.GetComponentInChildren<PlayerHealth>().Heal(healAmount);
                   // GetComponentInChildren<Animator>().SetTrigger("PickupTrigger");
                    // Destroy(Instantiate(particleEffect, transform), 1.0f);
                   Destroy(this.gameObject, 0.1f);
                    _pickedUp = true;
                }
            }
        }
    }

}
