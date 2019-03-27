using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour {

    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField]
    Camera cam;

    [SerializeField]
    float normalSpread;

    // Use this for initialization
    void Start () {
       // cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FireProjectile()
    {

        //GameObject p = Instantiate(projectilePrefab, transform.position + selfCamera.transform.forward, selfCamera.transform.rotation)
        GameObject p = Instantiate(projectilePrefab, transform.position + transform.forward + new Vector3(0.0f, 1.0f, 0.0f),cam.transform.rotation);
        Vector3 direction = p.transform.forward;
        direction.x += Random.Range(-normalSpread, normalSpread);
        direction.y += Random.Range(-normalSpread, normalSpread);
        direction.z += Random.Range(-normalSpread, normalSpread);
        p.transform.forward = direction;


    }
}

