using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField]
    GameObject teleportTransform;

    [SerializeField]
    Vector3 teleportPoint;
    // Start is called before the first frame update
    void Start()
    {
        //teleportPoint = teleportTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CharacterController>().enabled = false;
              other.gameObject.transform.position= teleportTransform.transform.position;
            other.gameObject.GetComponent<CharacterController>().enabled = true;

        }


    }
}
