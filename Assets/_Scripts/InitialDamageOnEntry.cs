using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialDamageOnEntry : MonoBehaviour
{
    [SerializeField]
    PlayerHealth health;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.name == "Chad")
        {
            health.TakeDamage(300);
            Destroy(this.gameObject);
        }
    }
}
