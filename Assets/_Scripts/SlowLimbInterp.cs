using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SlowLimbInterp : MonoBehaviour {

    [SerializeField]
    Transform original;

    [SerializeField]
    float lerpValue;

    [SerializeField]
    float minDist;

    [SerializeField]
    float speed;

    float shakeFactor;

    [SerializeField]
    bool enableOffset;

    [SerializeField]
    Vector3 posOffset;

    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }

    public void OptimizedUpdate()
    {
        if (enabled)
        {
            Vector3 pos = original.position;
            if (enableOffset)
            {
                pos += original.right * posOffset.x;
                pos += original.up * posOffset.y;
                pos += original.forward * posOffset.z;
            }

            float theDist = Vector3.Distance(transform.position, pos);
            if (theDist < minDist)
            {
                Vector3 dir = (pos - transform.position).normalized;

                float theSpeed = Mathf.Lerp(0, speed, theDist / minDist);

                transform.position += dir * theSpeed * Time.deltaTime;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, pos, lerpValue);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, original.rotation, lerpValue);

            ApplyShakeFactor();
        }
    }

    public void ApplyShakeFactor()
    {
        transform.position += Random.insideUnitSphere * shakeFactor;
    }

    public void SetOffsetActive(bool b)
    {
        enableOffset = b;
    }

    public void SetShakeFactor(float s)
    {
        shakeFactor = s;
    }

    public void SetLerp(float v)
    {
        lerpValue = v;
    }
}
