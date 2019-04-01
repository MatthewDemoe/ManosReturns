using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SlowGenericInterp : MonoBehaviour
{
    [SerializeField]
    Transform original;

    [SerializeField]
    float lerpValue;
    float ogLerp;

    [SerializeField]
    float rotLerpValue;

    [SerializeField]
    float minDist;

    [SerializeField]
    float speed;

    float shakeFactor;

    private void Start()
    {
        ogLerp = lerpValue;
    }

    // Update is called once per frame
    void Update()
    {
        float theDist = Vector3.Distance(transform.position, original.position);
        if (theDist < minDist)
        {
            Vector3 dir = (original.position - transform.position).normalized;

            float theSpeed = Mathf.Lerp(0, speed, theDist / minDist);

            transform.position += dir * theSpeed * Time.deltaTime;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, original.position, lerpValue);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, original.rotation, lerpValue);
    }

    public void SetInterpValue(float i)
    {
        lerpValue = i;
    }

    public void ResetInterpValue()
    {
        lerpValue = ogLerp;
    }

}
