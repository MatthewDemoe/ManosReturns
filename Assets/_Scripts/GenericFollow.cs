using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericFollow : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    Vector3 followOffset;

    [SerializeField]
    bool followRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + followOffset;

        if (followRotation)
        {
            transform.rotation = target.rotation;
        }
    }

}
