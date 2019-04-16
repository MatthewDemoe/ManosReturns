using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField]
    private List<Transform> targets;

    [SerializeField]
    private Transform origin;

    [SerializeField]
    public Transform currentLockPoint;

    
    [SerializeField]
    public Transform closestTarget;

    /*
    [SerializeField]
    private float distance;
    */

    bool m_HitDetect;
    RaycastHit hit;
    float m_MaxDistance;

    void Start()
    {
        hit = new RaycastHit();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(currentLockPoint != null)
        {
            Ray ray = new Ray(origin.position, origin.forward);

            Debug.DrawRay(origin.position, origin.forward * 75.0f, Color.green);
            //if(Physics.Raycast(ray, out hit))
            if (Physics.CapsuleCast(origin.position, currentLockPoint.position, 2.0f, origin.forward, out hit))
            {
                closestTarget = hit.transform;
                
                if (hit.transform.name != "LockOnTarget")
                {
                    Debug.Log("No Lock");
                }
                else
                {
                    Debug.Log("Lock");
                }
            }
        }
       


        /*
        if(targets.Count > 0)
        {
            distance = 200.0f;
            foreach (var target in targets)
            {
                if(target == null)
                {
                    targets.Remove(target);
                }
                var newDistance = Vector3.Distance(origin.position, target.position);
                if (newDistance < distance)
                {
                    distance = newDistance;
                    closestTarget = target;
                }
            }
        }
        */
    }

    //Draw the BoxCast as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (m_HitDetect)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + transform.forward * hit.distance, transform.localScale);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, transform.forward * m_MaxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + transform.forward * m_MaxDistance, transform.localScale);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform != transform.parent)
        {
            targets.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform != transform.parent)
        {
            targets.Remove(other.transform);
            if(targets.Count == 0)
            {
                //closestTarget = null;
            }
        }
    }
}
