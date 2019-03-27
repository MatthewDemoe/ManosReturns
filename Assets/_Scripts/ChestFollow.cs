using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestFollow : MonoBehaviour
{

    [SerializeField]
    float headRotMax;

    [SerializeField]
    float headRotMin;

    [SerializeField]
    float bodyOffSetMax;

    [SerializeField]
    float bodyOffSetMin;

    [SerializeField]
    float neckOffset;

    [SerializeField]
    Transform head;

    [SerializeField]
    Vector3 offset;

    [SerializeField]
    Transform neckBase;

    float bodyRotation;

    [SerializeField]
    float rotationLerpSpeed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //transform.localPosition = offset;
        Matrix4x4 headRotation = Matrix4x4.Rotate(head.transform.rotation);
        Vector3 newOffset = (Vector3)(headRotation * offset);

        neckBase.position = new Vector3(head.position.x + newOffset.x,
            head.position.y + offset.y,
            head.position.z + newOffset.z);

        //bodyRotation = Quaternion.Slerp()

        //Vector3 headLookVector = head.transform.forward;
        //bool lookWithinHorizontalZone = (Mathf.Abs(headLookVector.y) < 0.4);
        // rotation

        bodyRotation = Mathf.Lerp(neckBase.rotation.eulerAngles.y, head.rotation.eulerAngles.y, rotationLerpSpeed * Time.deltaTime);

        neckBase.rotation = Quaternion.Euler(
            0,
            bodyRotation,
            0
            );
    }
}
