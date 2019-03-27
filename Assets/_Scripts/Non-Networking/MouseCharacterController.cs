using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCharacterController : MonoBehaviour {
    public GameObject player;
    public float rotSpeed = 5;
   public Vector3 offSet;
    Transform t;
	// Use this for initialization
	void Start () {
        offSet = player.transform.position - transform.position;
        
	}
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxis("Look X") * rotSpeed;
        float vertical = Input.GetAxis("Look Y") * rotSpeed;
        player.transform.Rotate(0, horizontal, 0);
        t.Rotate(vertical, 0, 0);
        float deltaY = player.transform.eulerAngles.y;
        float deltaX = t.eulerAngles.x;
        Quaternion quat = Quaternion.Euler(deltaX, deltaY, 0);
        transform.position = player.transform.position - (quat * offSet);

        transform.LookAt(player.transform);
		
	}
}
