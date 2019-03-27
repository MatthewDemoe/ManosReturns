using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDuration : MonoBehaviour {
    public float delay = 0.0f;

	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, delay);
	}
	

}
