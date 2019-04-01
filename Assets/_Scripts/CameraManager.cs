using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    int camNum;

    [SerializeField]
    float fov_default = 60;

    public bool lockOn;
    public float followSpeed;
    public float camSensX, camSensY;

    [SerializeField]
    float _aimSpeedModifier = 1.0f;

    public Transform target;
    public Transform lockOnTarget;
    public Transform refTarget;

    Vector3 _cameraOffsetDefault;
    float _cameraFollowDistanceDefault;

    float _currentFollowDistance;
    float _distanceLerpSpeed = 7.5f;

    //public static CameraManager singleton;

    Transform _pivot;
    Transform _camTrans;
    Transform _refTrans;
    [SerializeField]
    float turnSmoothing = 0.1f;
    public float minAngle = -75;
    public float maxAngle = 75;

    [SerializeField]
    float smoothX;
    float smoothY;
    float smoothXvelocity = 1;
    float smoothYvelocity;
    public float lookAngle;
    public float tiltAngle;

    public bool lookInvertY;

    float h;
    float v;

    Camera cam;

    float fovTimer = -5f;
    float fovTime;
    float beginFOV, endFOV;
    bool lerpBack;
    float FOVstart;

    [SerializeField]
    LayerMask cameraCollisionMask;

    [Header("Camera Shake Properties")]
    [SerializeField]
    float linearDecay = 0.1f;

    [SerializeField]
    float nonLinearDecay = 0.1f;

    [SerializeField]
    float tweenTime = 0.1f;

    float _trauma = 0.0f;
    float _flip = 1.0f;


    private void Start()
    {

        if (target != null) Init(target);
        cam = GetComponentInChildren<Camera>();


        _cameraOffsetDefault = cam.transform.localPosition;
        _cameraFollowDistanceDefault = _cameraOffsetDefault.magnitude;
    }

    public void Init(Transform t)
    {
        target = t;

        _camTrans = transform.GetChild(0).GetChild(0);
        _refTrans = transform.GetChild(1);
        _refTrans.transform.parent = null;
        _pivot = _camTrans.parent;
    }


    private void Update()
    {

    }
    public void SetAimSpeed(float newValue)
    {
        _aimSpeedModifier = newValue;

    }

    public void ResetAimSpeed()
    {
        _aimSpeedModifier = 1.0f;
    }

    public float getLookX()
    {

        return h;
    }

    public void Tick(float delta)
    {
        //if (!(_trauma > 0.0f))
        {
            h = Input.GetAxis("Right Stick X");
            v = Input.GetAxis("Right Stick Y") * ((lookInvertY) ? -1 : 1);

            FollowTarget(delta);
            HandleRotations(delta, v, h);

            CollideCamera();
            HandleFOV();
        }
    }

    public void ResetFOV()
    {
        cam.fieldOfView = fov_default;
    }

    void HandleFOV()
    {
        if (fovTimer != -5f)
        {
            fovTimer -= Time.deltaTime;
            cam.fieldOfView = UtilMath.EasingFunction.EaseInSine(endFOV, beginFOV, fovTimer / fovTime);
            if (fovTimer < 0)
            {
                if (lerpBack)
                {
                    LerpFOV(fovTime, endFOV, beginFOV, false);
                }
                else
                {
                    fovTimer = -5f;
                    cam.fieldOfView = endFOV;
                }
            }
        }
    }

    public void ToggleLookInvert()
    {
        lookInvertY = !lookInvertY;
    }

    void FollowTarget(float delta)
    {
        float speed = delta * followSpeed;
        Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, speed);
        transform.position = targetPosition;
        //transform.position = target.position;
    }

    public void SetLockedTarget(Transform t)
    {
        lockOnTarget = t;
        lockOn = true;
    }

    public void SetReferenceTarget(Transform t)
    {
        refTarget = t;
    }

    void HandleRotations(float delta, float v, float h)
    {
        if (turnSmoothing > 0)
        {
            smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothXvelocity, turnSmoothing);
            smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothYvelocity, turnSmoothing);
        }
        else
        {
            smoothX = h;
            smoothY = v;
        }

        tiltAngle -= smoothY * camSensY * _aimSpeedModifier;
        tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);
        if (float.IsNaN(tiltAngle)) tiltAngle = 0;
        _pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);

        _refTrans.position = transform.position;

        if (refTarget != null)
        {
            Vector3 dir = _refTrans.position - refTarget.position;
            _refTrans.forward = dir.normalized;
            Debug.DrawLine(_refTrans.position, -_refTrans.forward * 100, Color.white);
            //refTrans.LookAt(refTarget);
        }

        //refTrans.localRotation = Quaternion.Euler(0, refTrans.localRotation.y, refTrans.localRotation.z);
        //refTrans.rotation = Quaternion.Euler(0, refTrans.rotation.y, refTrans.rotation.z);

        if (lockOn && lockOnTarget != null)
        {
            Vector3 targetDir = lockOnTarget.position - transform.position;
            targetDir.Normalize();
            //targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = transform.forward;
            Quaternion targetRot = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, delta * 9);
            lookAngle = transform.eulerAngles.y;

            return;
        }

        lookAngle += smoothX * camSensX * _aimSpeedModifier;
        if (float.IsNaN(lookAngle)) lookAngle = 0;
        transform.rotation = Quaternion.Euler(0, lookAngle, 0);
    }

    /// <summary>
    /// Prevent camera from entering walls, floors, and ceilings
    /// </summary>
    void CollideCamera()
    {
        Vector3 castDir = (cam.transform.position - _pivot.position).normalized;


        RaycastHit hit;

        float radius = 0.3f;
        float capsuleCastWidth = 1.4f;
        float rayCastPaddingDistance = capsuleCastWidth;

        Vector3 point1 = _pivot.position - (castDir * (rayCastPaddingDistance)) - Vector3.right * capsuleCastWidth * 0.5f;
        Vector3 point2 = point1 + Vector3.right * capsuleCastWidth;

        // = (1 << LayerMask.NameToLayer("Player"));
        //mask &= (LayerMask.NameToLayer("NoPlayerCollide"));
        //mask = ~mask;


        if (Physics.CapsuleCast(point1, point2, radius, castDir, out hit, _cameraFollowDistanceDefault + rayCastPaddingDistance, cameraCollisionMask))
        {
            Debug.DrawLine(point1, point2, Color.green);
            Debug.DrawLine(point1, hit.point, Color.red);

            _currentFollowDistance = Mathf.Lerp(_currentFollowDistance, hit.distance - rayCastPaddingDistance - radius, _distanceLerpSpeed * Time.deltaTime);
        }
        else
        {
            _currentFollowDistance = Mathf.Lerp(_currentFollowDistance, _cameraFollowDistanceDefault, _distanceLerpSpeed * Time.deltaTime);
        }

        _currentFollowDistance = Mathf.Min(_currentFollowDistance, _cameraFollowDistanceDefault);
        Vector3 localPlacement = _cameraOffsetDefault.normalized * _currentFollowDistance;

        cam.transform.localPosition = localPlacement;
    }

    public Transform GetReference() {
        return _refTrans;
    }

    public void AddAngle(float x, float y)
    {
        lookAngle += x * 0.2f;
        tiltAngle += y * 0.33f;
    }

    /// <summary>
    /// Interpolates the fieldOfView of the camera
    /// </summary>
    /// <param name="time">Duration of the interpolation</param>
    /// <param name="_startFOV">The field of view you're starting with</param>
    /// <param name="_endFOV">The field of view you want to end at</param>
    /// <param name="comeBack">The field of view you want to end at</param>
    public void LerpFOV(float time, float _startFOV, float _endFOV, bool comeBack = true)
    {
        if (comeBack)
        {
            fovTime = time / 2;
            FOVstart = _startFOV;
        }
        else
        {
            fovTime = time;
        }
        beginFOV = _startFOV;
        endFOV = _endFOV;
        fovTimer = fovTime;
        lerpBack = comeBack;
    }

    private void Awake()
    {
        //if (singleton != null)
        //{
        //    
        //}
        //singleton = this;
    }

    public void DoCameraShake(float tr, float intensity)
    {
        StartCoroutine(StartShake(tr, intensity));

    }

    IEnumerator StartShake(float tr, float intensity)
    {
        _trauma = tr;

        while(_trauma > 0.0f)
        {
            

            _trauma = Mathf.Min(_trauma, 1.0f);
            _trauma -= ((linearDecay * Time.deltaTime) + (nonLinearDecay * _trauma * Time.deltaTime));
            _trauma = Mathf.Max(_trauma, 0.0f);

            Vector3 dir = transform.position;

            dir.x += _trauma * intensity * _flip;

            transform.position = dir;

            _flip = -_flip;

            yield return new WaitForSeconds(tweenTime);
        }
    }
}
