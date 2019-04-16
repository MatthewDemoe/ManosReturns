using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManosGrab : MonoBehaviour
{
    [SerializeField]
    ManosHand manosHand;

    // Start is called before the first frame update
    void Start()
    {
        if (manosHand == null)
        {
            manosHand = transform.parent.GetComponent<ManosHand>();
        }
    }

    public ManosHand GetHand()
    {
        return manosHand;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manosHand.SetCollidingObject(other);
            print(other.name);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manosHand.SetCollidingObject(other);
            print(other.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        manosHand.SetCollidingObject(null);
    }
}
