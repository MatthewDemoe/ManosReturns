using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericBillboard : MonoBehaviour {

    [SerializeField]
    Enums.Player theTarget;

    [SerializeField]
    Transform target;

	// Use this for initialization
	void Start () {
        switch (theTarget)
        {
            case Enums.Player.Manos:
                target = GameObject.Find("Manos 2.3").transform.GetChild(0).GetChild(4);
                break;
            case Enums.Player.Player1:
                target = GameObject.Find("CameraStand").transform.GetChild(0).GetChild(0);
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(target);
	}
}
