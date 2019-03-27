using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandCollision : MonoBehaviour {

    [SerializeField]
    float dist;

    [SerializeField]
    SlowLimbInterp interper;

    [SerializeField]
    VelocityInterp vInterp;

    [SerializeField]
    GameObject trueHand;

    [SerializeField]
    SteamVR_Behaviour_Skeleton skeltal;

    bool canCheck;

    private void Start()
    {

    }

    private void Update()
    {
        if (canCheck)
        {
            if (Vector3.Distance(interper.transform.position, transform.position) <= dist)
            {
                Deactivate();
            }
        }
    }

    void Deactivate()
    {
        interper.enabled = true;
        vInterp.enabled = false;
        skeltal.enabled = true;
        trueHand.SetActive(false);
        canCheck = false;
    }

    IEnumerator CheckCountdown()
    {
        yield return new WaitForSeconds(0.1f);
        canCheck = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            StartCoroutine("CheckCountdown");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Ground"))
        //{
        //    Deactivate();
        //}
    }
}
