using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    int camNum;

    [SerializeField]
    float fov_default = 60;

    [SerializeField]
    bool lockOn;
    [SerializeField]
    float followSpeed;
    [SerializeField]
    float camSensX, camSensY;

    [SerializeField]
    float _aimSpeedModifier = 1.0f;

    [SerializeField]
    Transform target;

    public Transform Target()
    {
        return target;
    }


    [SerializeField]
    Transform lockOnTarget;
    [SerializeField]
    Transform refTarget;


    //public static CameraManager singleton;

    Transform _pivot;
    Transform _camTrans;
    Transform _refTrans;
    [SerializeField]
    float turnSmoothing = 0.1f;
    [SerializeField]
    float minAngle = -75;
    [SerializeField]
    float maxAngle = 75;

    float smoothX;
    float smoothY;
    float smoothXvelocity = 1;
    float smoothYvelocity;
    [SerializeField]
    float lookAngle;
    [SerializeField]
    float tiltAngle;

    [SerializeField]
    bool lookInvertY;

    float horizontal;
    float vertical;

    Camera cam;

    float fovTimer = -5f;
    float fovTime;
    float beginFOV, endFOV;
    bool lerpBack;
    float FOVstart;


    [Header("Camera collision")]

    [SerializeField]
    LayerMask cameraCollisionMask;
    [SerializeField]
    float radius = 0.3f;
    [SerializeField]
    float capsuleCastWidth = 1.4f;
    [SerializeField]
    float rayCastPaddingDistance = 1.0f;

    Vector3 _cameraOffsetDefault;
    float _cameraFollowDistanceDefault;
    float _cameraFollowDistanceColliding;
    float _followDistTarget;

    Vector3 _pivotTarget;
    Vector3 _pivotDefault;
    Vector3 _pivotTargetColliding;

    float _currentFollowDistance;
    [SerializeField]
    float _distanceLerpSpeed = 7.5f;
    [SerializeField]
    float _pivotCollisionLerpSpeed = 7.5f;

    [SerializeField]
    float _collisionRecoverySpeed = 2.5f;

    bool _isColliding = false;


    [Header("Camera Shake Properties")]
    [SerializeField]
    float linearDecay = 0.1f;

    [SerializeField]
    float nonLinearDecay = 0.1f;

    [SerializeField]
    float tweenTime = 0.1f;

    float _trauma = 0.0f;
    float _flip = 1.0f;

    /// <summary>
    /// camera
    /// </summary>
    [SerializeField]
    LayerMask cameraOccluderMask;

    // target lock can be lost if not in view for this duration

    [SerializeField]
    float targetLockLOSLostTime = 2.0f;
    float _targetLockLostTimer;

    [SerializeField]
    Image lockOnSprite;

    [SerializeField]
    float targetLOSLostFlashFreq = 10.0f;
    [SerializeField]
    Color targetLOSLostColor = Color.grey;
    Color _lockOnSpriteColorDefault;

    private void Start()
    {
        _lockOnSpriteColorDefault = lockOnSprite.color;

        if (target != null)
        {
            Init(target);
        }
        cam = GetComponentInChildren<Camera>();

        _cameraOffsetDefault = cam.transform.localPosition;
        _cameraFollowDistanceDefault = _cameraOffsetDefault.magnitude;
        _cameraFollowDistanceColliding = _cameraFollowDistanceDefault;
        _pivotDefault = _pivot.localPosition;
        _pivotTarget = _pivotDefault;
        _pivotTargetColliding = _pivotTarget;
    }

    public void Init(Transform t)
    {
        target = t;
        TargetLockOn tLock = target.GetComponentInChildren<TargetLockOn>();
        if (tLock != null)
        {
            tLock.SetCameraManager(this);
        }

        _camTrans = transform.GetChild(0).GetChild(0);
        _refTrans = transform.GetChild(1);
        _refTrans.transform.parent = null;
        _pivot = _camTrans.parent;
    }

    private void Update()
    {
        if (lockOn)
        {
            //Debug.Log("locked");
            Vector3 viewPos = cam.WorldToViewportPoint(lockOnTarget.position);
            lockOnSprite.rectTransform.anchorMin = viewPos;
            lockOnSprite.rectTransform.anchorMax = viewPos;

        }

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
        return horizontal;
    }

    public Camera GetCamera()
    {
        return cam;
    }

    public void Tick(float delta)
    {
        //if (!(_trauma > 0.0f))
        {
            horizontal = Input.GetAxis("Right Stick X");
            vertical = Input.GetAxis("Right Stick Y") * ((lookInvertY) ? -1 : 1);

            LineOfSightCheck();
           
            FollowTarget(delta);
            HandleRotations(delta, vertical, horizontal);

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

    // follow the host player
    void FollowTarget(float delta)
    {
        float speed = delta * followSpeed;
        Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, speed);
        transform.position = targetPosition;
    }


    /// <summary>
    /// Returns true if 
    /// </summary>
    /// <returns></returns>
    public bool LineOfSightCheck()
    {
        bool targetVisible = false;

        if (lockOn)
        {
            Vector3 camPos = cam.transform.position;
            Vector3 targetVec = lockOnTarget.position - camPos;
            
            if (Physics.Raycast(camPos, targetVec, Mathf.Max(targetVec.magnitude - 3.0f, 0.0f), cameraOccluderMask))
            {
                // no line of sight
                _targetLockLostTimer -= Time.deltaTime;

                if(_targetLockLostTimer <= 0.0f)
                {
                    SetLockedOn(false);
                } else // LOS lost
                {
                    lockOnSprite.enabled = Mathf.Sin(Time.time * targetLOSLostFlashFreq) > 0.0f;
                    lockOnSprite.color = targetLOSLostColor;
                }
            }
            else
            {
                lockOnSprite.enabled = true;
                // line of sight
                _targetLockLostTimer = targetLockLOSLostTime;
                targetVisible = true;
                lockOnSprite.color = _lockOnSpriteColorDefault;
            }

        }

        return targetVisible;
    }

    public void SetLockedTarget(Transform t)
    {
        lockOnTarget = t;
        if (lockOnTarget == null)
        {
            SetLockedOn(false);
        }
            
    }

    public Transform GetLockedTarget()
    {
        return lockOnTarget;
    }

    public void SetLockedOn(bool b)
    {
        lockOn = b;
        lockOnSprite.gameObject.SetActive(b);
    }

    public bool IsLockedOn()
    {
        return lockOn;
    }

    public void SetReferenceTarget(Transform t)
    {
        refTarget = t;
    }

    void HandleRotations(float delta, float v, float h)
    {
        if (lockOn && lockOnTarget != null)
        {
            // Get direction of target
            Vector3 targetDir = lockOnTarget.position - transform.position;
            targetDir.Normalize();

            // Create a rotation from the directional vector
            if (targetDir == Vector3.zero) {
                targetDir = transform.forward;
            }
                
            Quaternion targetRot = Quaternion.LookRotation(targetDir);

            // SLerp towards the new rotation
            Quaternion newRot = Quaternion.Slerp(transform.rotation, targetRot, 0.8f);
            tiltAngle = Mathf.LerpAngle(tiltAngle, targetRot.eulerAngles.x, 0.5f);
            lookAngle = Mathf.LerpAngle(lookAngle, targetRot.eulerAngles.y, 0.5f);

            if (float.IsNaN(tiltAngle)) tiltAngle = 0;
            // Helps to clamp angle once lock on is done
            if (tiltAngle > 180)
            {
                tiltAngle -= 180;
                tiltAngle = -(180 - tiltAngle);
            }
            _pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
            if (float.IsNaN(lookAngle)) lookAngle = 0;
            transform.rotation = Quaternion.Euler(0, lookAngle, 0);

            return;
        }

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
        
        // Clamps tiltAngle after locking on
        tiltAngle -= smoothY * camSensY * _aimSpeedModifier;
        if (tiltAngle > 180)
        {
            tiltAngle -= 180;
        }

        // keep vertical camera angle within set elevation/depression angles 
        tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);

        //if (tiltAngle < minAngle)
        //{
        //    tiltAngle = minAngle;
        //    //tiltAngle = Mathf.LerpAngle(tiltAngle, minAngle, delta * 9);
        //}
        //if (tiltAngle > maxAngle)
        //{
        //    tiltAngle = maxAngle;
        //    //tiltAngle = Mathf.LerpAngle(tiltAngle, maxAngle, delta * 9);
        //}

        if (float.IsNaN(tiltAngle))
        {
            tiltAngle = 0;
        }

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

        lookAngle += smoothX * camSensX * _aimSpeedModifier;
        if (float.IsNaN(lookAngle))
        {
            lookAngle = 0;
        }
            
        transform.rotation = Quaternion.Euler(0, lookAngle, 0);
    }

    public void ResetLookOrientation()
    {
        tiltAngle = 0;
    }

    /// <summary>
    /// Prevent camera from entering walls, floors, and ceilings
    /// </summary>
    void CollideCamera()
    {
        Vector3 castDir = (cam.transform.position - _pivot.position).normalized;

        RaycastHit hit;

        Vector3 middle = _pivot.position - (castDir * (rayCastPaddingDistance));

        Vector3 point1 = middle - _pivot.right * capsuleCastWidth * 0.5f;
        Vector3 point2 = point1 + _pivot.right * capsuleCastWidth;

        _isColliding = false;

        _pivotTarget = _pivotDefault;
        

        if (Physics.CapsuleCast(point1, point2, radius, castDir, out hit, _cameraFollowDistanceDefault + rayCastPaddingDistance, cameraCollisionMask))
        {
            //Debug.DrawLine(point1, point2, Color.green);
            //Debug.DrawLine(point1, hit.point, Color.red);
            _isColliding = true;
        }
        else
        {
            RaycastHit rightHit;
            if (Physics.Raycast(_pivot.position - _pivot.right * 0.1f, _pivot.right, out rightHit, capsuleCastWidth * 0.5f, cameraCollisionMask))
            {
                var newTarg = _pivot.InverseTransformPoint(rightHit.point - _pivot.right * 0.15f);
                if(rightHit.distance < _pivotDefault.x)
                {
                    _pivotTarget = Vector3.Lerp(_pivotTarget, newTarg, _pivotCollisionLerpSpeed * Time.deltaTime);
                    _pivotTarget.y = _pivotDefault.y;
                    _pivotTargetColliding = _pivotTarget;
                    _isColliding = true;
                }
            }
        }

        if(_isColliding)
        {
            _followDistTarget = hit.distance - rayCastPaddingDistance - radius;
            _cameraFollowDistanceColliding = Mathf.Min(_cameraFollowDistanceDefault, _followDistTarget);
            //_cameraFollowDistanceColliding = Mathf.Lerp(_cameraFollowDistanceColliding, Mathf.Min(_cameraFollowDistanceDefault, _followDistTarget), _collisionRecoverySpeed * Time.deltaTime);
        } else
        {
             _pivotTargetColliding = Vector3.MoveTowards(_pivotTargetColliding, _pivotDefault, _collisionRecoverySpeed * Time.deltaTime);
             _cameraFollowDistanceColliding = Mathf.MoveTowards(_cameraFollowDistanceColliding, _cameraFollowDistanceDefault, _collisionRecoverySpeed * Time.deltaTime);
        }



        _pivot.localPosition = Vector3.Lerp(_pivot.localPosition, _pivotTargetColliding, _pivotCollisionLerpSpeed * Time.deltaTime);
        _currentFollowDistance = Mathf.Lerp(_currentFollowDistance, _cameraFollowDistanceColliding, _distanceLerpSpeed * Time.deltaTime);
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

    public void DoCameraShake(float tr)
    {
        StopAllCoroutines();
        StartCoroutine(StartShake(tr));
    }

    IEnumerator StartShake(float tr)
    {
        _trauma = tr;

        while(_trauma > 0.0f)
        {           
            _trauma = Mathf.Min(_trauma, 1.0f);
            _trauma -= ((linearDecay * Time.deltaTime) + (nonLinearDecay * _trauma * Time.deltaTime));
            _trauma = Mathf.Max(_trauma, 0.0f);

            Vector3 dir = transform.position;

            dir += _trauma * _flip * transform.right;

            transform.position = dir;

            _flip = -_flip;

            yield return new WaitForSeconds(tweenTime - (tweenTime * _trauma));
        }
    }
}
