using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemBehaviour : MonoBehaviour {

    [SerializeField]
    float health;

    [SerializeField]
    float maxHealth;

    [SerializeField]
    float power;

    [SerializeField]
    [Tooltip("MAX POWER! MAXIMUM, AND HIGHER!")]
    float maxPower;

    [SerializeField]
    Transform chest;

    Vector3 ogPos;
    Quaternion ogRot;

    [SerializeField]
    float returnSpeed;

    [SerializeField]
    float disTolerance;

    bool stable;

	// Use this for initialization
	void Start () {
        ogPos = transform.localPosition;
        ogRot = transform.localRotation;
        stable = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (!stable)    
        {
            if (transform.parent == null)
            {
                if (Vector3.Distance(transform.position, chest.position) > disTolerance)
                {
                    Vector3 dir = (transform.position - chest.position).normalized;
                    transform.position -= dir * returnSpeed * Time.deltaTime;
                    transform.Rotate(Vector3.up * returnSpeed);
                }
                else
                {
                    transform.parent = chest;
                    transform.localPosition = ogPos;
                    transform.localRotation = ogRot;
                    stable = true;
                }
            }
        }
	}

    public void Grab()
    {
        stable = false;
    }

    public void Release()
    {
        print("RELEAE");
        transform.parent = null;
    }
}
