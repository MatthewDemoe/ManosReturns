using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTorsoFollow : MonoBehaviour
{

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //FIELDS///////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    [SerializeField]
    public Transform transHead;
    [SerializeField]
    public Transform transNeckBase;
    [SerializeField]
    public float lookHorizontalThreshold = 0.8f;

    //// angle difference about the y-axis beyond which the torso will rotate to match the head angle
    [SerializeField]
    float torsoAngleTolerance = 10.0f;

    // if the head moves beyond this angle the torso will move to match it very quickly 
    [SerializeField]
    float torsoTurnFastThreshold = 70.0f;

    // speeds for head rotation to lerp between
    [SerializeField]
    float torsoTurnSpeedNormal = 0.03f;
    [SerializeField]
    float torsoTurnSpeedMax = 0.3f;

    [SerializeField]
    float torsoPitchMagnitude = 0.9f;

    [SerializeField]
    float torsoPitchThreshold = 20.0f;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //MEMBERS//////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////


    Vector3 headAzimuthVec;
    Quaternion torsoRotation;

    /// <summary>
    /// Offset of torso from head (i.e. neck length)
    /// </summary>
    [Tooltip("Offset of torso from head")]
    [SerializeField]
    Vector3 torsoOffset;

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //METHODS//////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    void Start()
    {
        //CalibrateNeckLength();
        CalcHeadAzimuth();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) CalibrateNeckLength(); //K for Calibrate
        CalcHeadAzimuth();
        TorsoAzimuthFollow();
        NeckOffset();
    }

    void CalibrateNeckLength()
    {
        torsoOffset = transNeckBase.position - transHead.position;
    }

    void CalcHeadAzimuth()
    {
        Vector3 lookVector = transHead.forward;

        // to avoid edge cases when look vector approaches directly up or down, use different vectors to calculate azimuth
        if (lookVector.y > lookHorizontalThreshold) // if looking far up
        {
            headAzimuthVec = -transHead.up;
        } else if(lookVector.y < -lookHorizontalThreshold) // if looking far down
        {
            headAzimuthVec = transHead.up;
        } else
        {
            headAzimuthVec = transHead.forward;
        }

        headAzimuthVec.y = 0;
    }

    // this method approximates the torso state by rotating the VR torso to face the azimuth of the head's look direction   
    // acts on the torso's azimuth (y rotation)
    private void TorsoAzimuthFollow()
    {

        Vector3 torsoAzim = transNeckBase.forward;
        torsoAzim.y = 0;

        float angleBetweenTorsoAndHead = Vector3.Angle(headAzimuthVec, torsoAzim);

        // should the torso move at all?
        if(angleBetweenTorsoAndHead > torsoAngleTolerance)
        {
            float turnRate;
            if (angleBetweenTorsoAndHead > torsoTurnFastThreshold)
            {
                turnRate = UtilMath.Lmap(angleBetweenTorsoAndHead, torsoTurnFastThreshold, 180, torsoTurnSpeedNormal, torsoTurnSpeedMax);
            }
            else
            {
                turnRate = torsoTurnSpeedNormal;
            }

            Quaternion torsoAzimQuat = Quaternion.LookRotation(torsoAzim, Vector3.up);
            Quaternion headAzimQuat = Quaternion.LookRotation(headAzimuthVec, Vector3.up); //QuarheadAzimuthVec
            Quaternion newTorsoQuat = Quaternion.Slerp(torsoAzimQuat, headAzimQuat, turnRate);

            Vector3 torsoEulers = newTorsoQuat.eulerAngles;

            torsoRotation = newTorsoQuat;
        }

    }

    private void NeckOffset()
    {
        // apply rotation
        transNeckBase.rotation = torsoRotation;

        // slight pitching of torso
        float pitch = -Mathf.Rad2Deg * Mathf.Atan(transHead.forward.y);
        //Debug.Log("Pitch: " + pitch);


        float absPitch = Mathf.Abs(pitch);
        if (absPitch > torsoPitchThreshold)
        {
            float pitchMultiplier = UtilMath.Lmap(absPitch, torsoPitchThreshold, 90.0f, 0.01f, torsoPitchMagnitude);
            //Debug.Log(pitchMultiplier);
            transNeckBase.Rotate(Vector3.right, pitch * pitchMultiplier);
        }

        // offset
       Vector3 newOffset = (transHead.rotation * torsoOffset);
       transNeckBase.position = transHead.position + newOffset;
    }
}
