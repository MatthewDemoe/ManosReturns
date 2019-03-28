using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{

    [Header("References")]

    GameObject player;
    CharacterController controller;
    PlayerManager pManager;

    [SerializeField]
    GameObject characterSprite;

    [SerializeField]
    Camera playerCamera;
    CameraManager camManager;

    [SerializeField]
    GameObject cameraStand;

    [Header("Movement")]

    [SerializeField]
    float baseMoveSpeed = 10.0f;

    [SerializeField]
    float controlAccelerationMax = 50.0f;

    [SerializeField]
    float jumpChargeMoveSpeedMultiplier = 0.5f;

    [SerializeField]
    float chargeMoveSpeedMultiplier = 0.5f;

    Vector3 _velocity = new Vector3(0.0f, 0.0f, 0.0f);
    Vector2 _desiredVelocity = new Vector2(0.0f, 0.0f);


    [Header("Kinematics")]

    // max speed the character is allowed to move at all (ignored by Dash)
    [SerializeField]
    float terminalSpeed = 50.0f;

    // max speed the character is allowed to fall
    [SerializeField]
    float terminalFallSpeed = 40.0f;
    [SerializeField]
    float gravity = -9.8f;

    [SerializeField]
    float gravityModifier = 1.0f;

    [SerializeField]
    float drag = 0.1f;

    [Header("Jump")]

    [SerializeField]
    float jumpMin = 10.0f;

    [SerializeField]
    float jumpMax = 30.0f;

    [SerializeField]
    float landEaseTime = 0.5f;
    float _landEase = 0.0f;

    float jumpChargeRate;

    float _jumpCharge = 1.0f;

    [Tooltip("allows the player some time just after walking off an edge to jump as if on land")]
    [SerializeField]
    float coyoteTime = 0.25f;

    // timer that allows the player to jump as normal just after walking off an edge
    float _coyoteTimer = 0.0f;


    [SerializeField]
    Vector3 _target = new Vector3(0.0f, 0.0f, 0.0f);

    Vector3 _knockDir = new Vector3(0.0f, 0.0f, 0.0f);

    [SerializeField]
    float knockDecay = 0.9f;

    [SerializeField]
    float rotationSpeed = 15.0f;

    // timer that ticks down. While this value is above 0, the player has no input control
    float _movementDisabledDurationTimer = 0.0f;

    [Header("Dash attack")]

    [SerializeField]
    float _dashDamage = 1.0f;

    float _dashSpeedCurrent = 0.0f;

    [SerializeField]
    Vector3 _dashVelocity = new Vector3(0.0f, 0.0f, 0.0f);

    // how long dash has been charged for
    float _dashChargingTime = 0.0f;
    float _dashChargeNormalized = 0.0f;

    // time left in dash attack after execution
    public float _dashStateDurationTimer = 0.0f;

    [SerializeField]
    float dashCooldown = 5.0f;

    [SerializeField]
    float dashDurationMin = 0.5f;
    [SerializeField]
    float dashDurationMax = 1.5f;


    [SerializeField]
    float dashSpeedMin = 50.0f;

    [SerializeField]
    float dashSpeedMax = 100.0f;


    [SerializeField]
    float dashDamageMin = 10.0f;

    [SerializeField]
    float dashDamageMax = 30.0f;

    [SerializeField]
    float chargeTime = 2.0f;

    float _dashCooldownTimer = 0.0f;

    [Header("Kick-off of dash attack")]

    [SerializeField]
    float kickoffDelay = 0.5f;

    [SerializeField]
    float kickOffSpeedMin = 20.0f;

    [SerializeField]
    float kickOffSpeedMax = 35.0f;


    Vector3 _dashDir = new Vector3(0.0f, 0.0f, 0.0f);

    float _spinRotation = 0;

    /// <summary>
    /// Amount of time after knockback where the player is immune to damage
    /// </summary>
    [SerializeField]
    [Tooltip("Amount of time after knockback where the player is immune to damage")]
    float invulnTime = 1f;

    [Header("Particles")]

    [SerializeField]
    ParticleSystem dustTrail;

    [SerializeField]
    ParticleSystem kickOffDust;

    [SerializeField]
    ParticleSystem jumpDust;

    bool _wasGroundedLastFrame = false;

    bool _chargingJump = false;
    bool _chargingDash = false;

    bool _kicking = false;
    bool _jumpUsed = false;
    bool _doubleJumpUsed = false;
    bool _airDashUsed = false;

    Vector3 _jumpDir = new Vector3(0.0f, 0.0f, 0.0f);
    float _landTimer;

    Vector3 _lateralInput = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 _forwardInput = new Vector3(0.0f, 0.0f, 0.0f);

    Animator _anim;
    AudioManager _am;
    PlayerHealth _playerHealth;

    Vector3 _targetRotation = new Vector3(0.0f, 0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
        controller = GetComponent<CharacterController>();
        jumpChargeRate = (jumpMax - jumpMin) / chargeTime;

        pManager = GetComponent<PlayerManager>();
        camManager = playerCamera.transform.root.GetComponent<CameraManager>();

        _am = AudioManager.GetInstance();
        _anim = GetComponent<Animator>();
        _playerHealth = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_wasGroundedLastFrame != IsGrounded())
        {
            // special behaviour when landing or leaving the ground
            if (_wasGroundedLastFrame)
            {
                OnLeaveGround();
            }
            else
            {
                OnLand();
            }

            _wasGroundedLastFrame = IsGrounded();
        }

        float val = UtilMath.Lmap(_landTimer, 0.0f, landEaseTime, 0.0f, 1.0f);

        //val = UtilMath.EasingFunction.Spring(0.0f, 1, val);

        if (_landTimer >= 0.0f)
            _anim.SetFloat("LandTime", val);

        if (_chargingDash)
            ChargeDash();

        if (_chargingJump)
            ChargeJump();

        float dt = Time.deltaTime;

        _coyoteTimer -= dt;
        _movementDisabledDurationTimer -= dt;
        _dashStateDurationTimer -= dt;
        _dashCooldownTimer -= dt;
        camManager.Tick(dt);
        _landTimer -= dt;

        LandRotation(_landTimer);

    }

    void LandRotation(float dt)
    {
        if (_landTimer > 0.0f)
        {
            _landEase = UtilMath.Lmap(_landTimer, 0.0f, landEaseTime, 1.0f, 0.0f);

            characterSprite.transform.rotation = Quaternion.Slerp(characterSprite.transform.rotation, transform.rotation, _landEase);
        }

    }

    public Vector3 GetVelocity()
    {
        return transform.worldToLocalMatrix.MultiplyVector(_velocity);
    }

    public Vector2 GetDesiredVelocity()
    {
        return _desiredVelocity;
    }

    public bool IsChargingDash()
    {
        return _chargingDash;
    }

    public bool IsChargingJump()
    {
        return _chargingJump;
    }

    public bool IsInputDisabled()
    {
        return _movementDisabledDurationTimer > 0.0f;
    }

    public void MoveUpdate(float horizontal, float vertical)
    {
        _lateralInput = horizontal * cameraStand.transform.right.normalized;
        _forwardInput = vertical * cameraStand.transform.forward.normalized;

        bool isMovementEnabled = pManager.GetEnumPlayerState() != Enums.PlayerState.Grabbed && !_kicking;

        //The edge case where you go off a ledge or are shot into the air while charging
        if (_chargingJump && !controller.isGrounded)
        {
            CancelJumpCharge();
        }

        if (controller.isGrounded)
        {
            _velocity.y = Mathf.Max(_velocity.y, 0.0f);
            _airDashUsed = false;
        }

        // Apply movement, velocity and translate etc
        if (isMovementEnabled)
        {
            ///////////////////////////////////////////////////////
            // Determine input control ///
            ///////////////////////////////////////////////////////
            _desiredVelocity = new Vector2((_lateralInput.x + _forwardInput.x), (_lateralInput.z + _forwardInput.z)) * baseMoveSpeed;

            if (pManager.GetEnumChargeState() == Enums.ChargingState.Dashing)
            {
                _dashVelocity = _dashDir * _dashSpeedCurrent;
                _velocity = _dashVelocity;
                CheckAhead();

            }

            else
            {
                // Apply user input

                if (!IsInputDisabled())
                {
                    Vector2 velXZ = new Vector2(_velocity.x, _velocity.z);

                    Vector2 desiredVectorChange;

                    if (controller.isGrounded)
                    {
                        // apply acceleration to reach target direction
                        desiredVectorChange = _desiredVelocity - velXZ;
                    }
                    else
                    {
                        desiredVectorChange = _desiredVelocity;
                    }

                    // limit acceleration of charcter applied through controls
                    float maxDeltaSpeed = controlAccelerationMax * Time.deltaTime;
                    desiredVectorChange = Vector2.ClampMagnitude(desiredVectorChange, maxDeltaSpeed);

                    float maxMoveSpeed = baseMoveSpeed;

                    // limit max movement speed when charging movement skills
                    if (_chargingJump)
                    {
                        float movementSpeedModification = Mathf.Lerp(1.0f, jumpChargeMoveSpeedMultiplier, GetJumpChargePercent());

                        maxMoveSpeed *= movementSpeedModification;
                    }
                    else if (_chargingDash)
                    {
                        //float movementSpeedModification = Mathf.Lerp(1.0f, chargeMoveSpeedMultiplier, GetDashChargePercent());

                        maxMoveSpeed *= chargeMoveSpeedMultiplier;
                    }

                    velXZ.x += desiredVectorChange.x;
                    velXZ.y += desiredVectorChange.y;

                    velXZ = Vector2.ClampMagnitude(velXZ, maxMoveSpeed);

                    //apply horizontal movement
                    _velocity.x = velXZ.x;
                    _velocity.z = velXZ.y;
                }

                //apply gravity and friction
                _velocity.y = _velocity.y + (gravity * gravityModifier * Time.deltaTime);
                _velocity.y = Mathf.Max(_velocity.y, -terminalFallSpeed);
                _velocity = Vector3.ClampMagnitude(_velocity, terminalSpeed);
            }

            _velocity *= (1.0f - drag);

            if (controller.isGrounded && (_desiredVelocity.magnitude >= 3.0f))
            {
                if (!_am.IsSoundLooping(AudioManager.Sound.ChadRun))
                {
                    _am.PlaySoundLoop(AudioManager.Sound.ChadRun, transform);
                }
            }
            else
            {
                _am.StopSoundLoop(AudioManager.Sound.ChadRun, true);
            }

            if (pManager.GetEnumPlayerState() == Enums.PlayerState.Knocked)
            {
                _spinRotation += (rotationSpeed * Time.deltaTime);
                characterSprite.transform.up = _velocity;
                characterSprite.transform.RotateAround(characterSprite.transform.position, characterSprite.transform.up, _spinRotation);
            }

            controller.Move(_velocity * Time.deltaTime);
        }
    }

    public void RotateUpdate(float horizontal, float sensitivity)
    {
        transform.forward = playerCamera.transform.forward;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    public void BeginChargeJump()
    {
        //Don't want to charge both at the same time
        if (!_chargingDash && (pManager.GetEnumPlayerState() != Enums.PlayerState.Grabbed) && (_movementDisabledDurationTimer <= 0.0f))
        {
            GetComponent<FlashFeedback>().ChargingFeedback();

            if (controller.isGrounded)
            {
                _chargingJump = true;
            }
        }
    }

    void ChargeJump()
    {
        //Give him the clamps
        _jumpCharge += jumpChargeRate * Time.deltaTime;
        _jumpCharge = Mathf.Clamp(_jumpCharge, jumpMin, jumpMax);
    }

    public void Jump()
    {
        //Cancel Charging if you jump
        if ((pManager.GetChargeState() >= 3))
        {
            Cancel();
            return;
        }

        if ((pManager.GetEnumPlayerState() != Enums.PlayerState.Grabbed) && (_movementDisabledDurationTimer <= 0.0f))
        {
            //either grounded, or not grounded and still having coyote time
            bool canJump = !_jumpUsed && (controller.isGrounded || (!controller.isGrounded && _coyoteTimer > 0.0f));

            if (canJump)
            {
                pManager.SetPlayerState(Enums.PlayerState.JumpUp);

                if (GetJumpChargePercent() < 0.1f)
                {
                    SingleJump();
                }
                else
                {
                    ChargedJump();
                }
            }
            else
            if (!_doubleJumpUsed) // Double Jump
            {
                DoubleJump();
            }
        }

        else
            GetComponent<FlashFeedback>().CooldownReact();
    }

    void SingleJump()
    {
        _jumpUsed = true;

        _am.PlaySoundOnce(AudioManager.Sound.ChadJump, transform, AudioManager.Priority.Default, 0);
        _anim.SetTrigger("JumpTrigger");

        _velocity.y += jumpMin;

        // apply slight speed boost
        //float jumpSpeedBoost = jumpMin;
        //_velocity.x += _desiredVelocity.x * jumpSpeedBoost;
        //_velocity.z += _desiredVelocity.y * jumpSpeedBoost;

        _jumpCharge = 0.0f;
        _chargingJump = false;

        Instantiate(jumpDust, transform.position, Quaternion.Euler(transform.rotation.eulerAngles + jumpDust.transform.rotation.eulerAngles));
    }

    void ChargedJump()
    {
        _jumpUsed = true;

        _am.PlaySoundOnce(AudioManager.Sound.ChadJump, transform);
        _anim.SetTrigger("JumpTrigger");

        _velocity.y += _jumpCharge;

        // apply slight speed boost
        float jumpSpeedBoost = _jumpCharge;
        _velocity.x += _desiredVelocity.x * jumpSpeedBoost;
        _velocity.z += _desiredVelocity.y * jumpSpeedBoost;

        _jumpCharge = 0.0f;
        _chargingJump = false;

        Instantiate(jumpDust, transform.position, Quaternion.Euler(transform.rotation.eulerAngles + jumpDust.transform.rotation.eulerAngles));
    }

    void DoubleJump()
    {
        _am.PlaySoundOnce(AudioManager.Sound.ChadDoubleJump, transform);

        pManager.SetChargeState(Enums.ChargingState.None);
        pManager.SetPlayerState(Enums.PlayerState.JumpUp);

        _jumpDir = (_lateralInput + _forwardInput).normalized;

        GetComponent<Animator>().SetTrigger("DoubleJump");

        _velocity.y += jumpMin;

        // apply slight speed boost
        float jumpSpeedBoost = jumpMin;
        _velocity.x += _jumpDir.x * jumpSpeedBoost;
        _velocity.z += _jumpDir.z * jumpSpeedBoost;

        _doubleJumpUsed = true;

        Instantiate(jumpDust, transform.position, Quaternion.Euler(transform.rotation.eulerAngles + jumpDust.transform.rotation.eulerAngles));

        if (_jumpDir != Vector3.zero)
            characterSprite.transform.forward = _jumpDir;
        //GameObject.Find("Armature").transform.forward = _jumpDir.normalized;

    }

    public void BeginChargeDash()
    {
        //Don't want to charge both at the same time
        if (!_chargingJump && (pManager.GetEnumPlayerState() != Enums.PlayerState.Grabbed) && (_movementDisabledDurationTimer <= 0.0f))
        {
            if (_dashCooldownTimer <= 0.0f && !_airDashUsed)
            {
                _chargingDash = true;

                GetComponent<FlashFeedback>().ChargingFeedback();

                characterSprite.transform.rotation = transform.rotation;

                _am.PlaySoundOnce(AudioManager.Sound.ChadCharge, transform);
            }
        }
    }

    public void ChargeDash()
    {
        _dashChargingTime += Time.deltaTime;
        _dashChargingTime = Mathf.Clamp(_dashChargingTime, 0.0f, chargeTime);
        _dashChargeNormalized = UtilMath.Lmap(_dashChargingTime, 0.0f, chargeTime, 0.0f, 1.0f);

        if (!controller.isGrounded)
            _velocity = Vector3.zero;
    }


    public void Dash()
    {
        if ((_chargingDash) && (pManager.GetEnumPlayerState() != Enums.PlayerState.Grabbed))
        {
            _am.StopSound(AudioManager.Sound.ChadCharge);

            _airDashUsed = true;

            _dashDamage = Mathf.Lerp(dashDamageMin, dashDamageMax, _dashChargeNormalized);
            _dashSpeedCurrent = Mathf.Lerp(dashSpeedMin, dashSpeedMax, _dashChargeNormalized);

            //_target = playerCamera.transform.position + (playerCamera.transform.forward * _dashSpeedCurrent);
            if ((Vector3.Angle(playerCamera.transform.forward, -transform.up) < 90.0f) && controller.isGrounded)
            {
                _dashDir = transform.forward;
            }
            else
            {
                //rotate to face dash direction
                characterSprite.transform.rotation = playerCamera.transform.rotation;

                _dashDir = playerCamera.transform.forward;
            }

            _dashCooldownTimer = dashCooldown;
            _chargingDash = false;
            _dashStateDurationTimer = Mathf.Lerp(dashDurationMin, dashDurationMax, _dashChargeNormalized);

            ParticleSystem trail = Instantiate(dustTrail, characterSprite.transform);

            pManager.SetChargeState(Enums.ChargingState.Dashing);

            if (pManager.GetPlayerState() == 1)
            {
                pManager.SetPlayerState(Enums.PlayerState.Falling);
            }

            _velocity /= 100;//Vector3.zero;

            // FOV change effect
            float fovMax = 90;
            float fovChange = Mathf.Lerp(60.0f, fovMax, _dashChargeNormalized);
            camManager.LerpFOV(_dashStateDurationTimer, 60, fovChange);

            _am.PlaySoundOnce(AudioManager.Sound.ChadDash, transform);

        }

        else
            GetComponent<FlashFeedback>().CooldownReact();
    }

    public void Cancel()
    {
        if (pManager.GetEnumChargeState() == Enums.ChargingState.Dashing)
        {
            EndDash();
        }else if (pManager.GetEnumChargeState() == Enums.ChargingState.ChargingJump)
        {
            CancelJumpCharge();
        }

        pManager.SetChargeState(Enums.ChargingState.None);

        _am.StopSound(AudioManager.Sound.ChadCharge);
        _chargingDash = false;
    }

    public void CancelJumpCharge()
    {
        _chargingJump = false;
        _jumpCharge = jumpMin;
    }

    public void EndDash()
    {
        _dashChargingTime = 0.0f;
        _dashStateDurationTimer = 0.0f;
        _dashSpeedCurrent = 0.0f;
        _dashCooldownTimer = dashCooldown;
        _dashChargeNormalized = 0.0f;
        ResetRotation();

        //characterSprite.transform.localRotation = Quaternion.identity;

        //cancel FOV effect
        camManager.ResetFOV();

        _kicking = false;
    }

    IEnumerator KickOff(RaycastHit hit, bool damageManos = false)
    {
        if ((pManager.GetEnumChargeState() == Enums.ChargingState.Dashing))
        {
            print(hit.transform.name);

            // determine kickoff speed
            float kickOffSpeed = (Mathf.Lerp(kickOffSpeedMin, kickOffSpeedMax, _dashChargeNormalized));

            Vector3 kickoffVelocity = -characterSprite.transform.forward;
            kickoffVelocity.y += 1.0f;
            kickoffVelocity.Normalize();
            kickoffVelocity *= kickOffSpeed;
            _velocity = Vector3.zero;

            _kicking = true;

            _movementDisabledDurationTimer = kickoffDelay + 0.3f;
            pManager.SetChargeState(Enums.ChargingState.Flip);

            yield return new WaitForSeconds(kickoffDelay);

            _anim.SetTrigger("FlipTrigger");

            if (damageManos)
            {
                ManosHand h = hit.transform.GetComponent<ManosHand>();
                if (h == null) h = hit.transform.GetComponentInParent<ManosHand>();

                //Shake the mesh that you hit
                MeshShaker m = hit.transform.GetComponent<MeshShaker>();
                if (m == null) m = hit.transform.GetComponentInChildren<MeshShaker>();

                if (hit.transform.name == "GauntletParent_L")
                {
                    //Returns true if damage was dealt
                    if (hit.transform.root.GetComponent<NoNetManos>().DealDamageToArm(Enums.Hand.Left, _dashDamage))
                    {
                        m.enabled = true;
                    }
                }
                else if (hit.transform.name == "GauntletParent_R")
                {
                    //Returns true if damage was dealt
                    if (hit.transform.root.GetComponent<NoNetManos>().DealDamageToArm(Enums.Hand.Right, _dashDamage))
                    {
                        m.enabled = true;
                    }
                }
                else
                {
                    if (h)
                    {
                        // If damage was successfuly dealt
                        if (h.TakeDamage(_dashDamage))
                        {
                            m.enabled = true;
                        }
                    }
                    else
                    {
                        PlayerHealth hp = hit.transform.transform.root.GetComponent<PlayerHealth>();
                        if (hp)
                            hp.TakeDamage(_dashDamage);
                        m.enabled = true;
                    }
                }
            }

            if (pManager.GetEnumChargeState() != Enums.ChargingState.Flip)
                Instantiate(kickOffDust, transform.position, Quaternion.Euler(transform.rotation.eulerAngles + kickOffDust.transform.rotation.eulerAngles));

            _velocity = kickoffVelocity;


            EndDash();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
            GameObject.Find("OverlordController").GetComponent<LobbyScript>().ReachedWaypoint();

        else if (other.gameObject.tag == "Respawn")
            GameObject.Find("OverlordController").GetComponent<LobbyScript>().ResetPosition();
    }

    public float GetChargePercent()
    {
        if (_chargingDash)
        {
            return GetDashChargePercent();

        }
        else
        if (_chargingJump)
        {
            return GetJumpChargePercent();
        }

        return 0.0f;
    }

    public float GetJumpChargePercent()
    {
        return (_jumpCharge - jumpMin) / (jumpMax - jumpMin);
    }

    public float GetDashChargePercent()
    {
        return (_dashChargeNormalized);
    }

    public void Knockback(Vector3 vec)
    {
        StartCoroutine("InvincibilityCountdown");
        _knockDir = vec;
        _velocity += _knockDir;

        controller.enabled = false;
        transform.position += _velocity.normalized;
        controller.enabled = true;

        if (_knockDir.magnitude >= 40.0f)
        {
            _movementDisabledDurationTimer = 100.0f;
            pManager.SetPlayerState(Enums.PlayerState.Knocked);
        }
    }

    IEnumerator InvincibilityCountdown()
    {
        _playerHealth.SetInvincible(true);
        yield return new WaitForSeconds(invulnTime);
        _playerHealth.SetInvincible(false);
    }

    public float GetDashDuration()
    {
        return _dashStateDurationTimer;
    }

    void CheckAhead()
    {
        RaycastHit hit;


        if (Physics.SphereCast(transform.position, 1.0f, _dashDir, out hit, _dashSpeedCurrent))
        {
            if ((hit.distance <= 1.5f))
            {
                if ((hit.transform.gameObject.tag.Equals("Hittable") && (pManager.GetEnumChargeState() == Enums.ChargingState.Dashing)))
                {
                    StartCoroutine(KickOff(hit, true));
                }

                else
                {
                    float angleOfDeflection = (Vector3.Angle(_dashDir, -hit.normal));

                    if (angleOfDeflection <= 30.0f)
                    {
                        if (hit.transform.gameObject.tag.Equals("Ground"))
                            StartCoroutine(KickOff(hit));
                    }
                    else if (angleOfDeflection <= 45.0f)
                    {
                        EndDash();
                    }
                }
            }
        }
    }

    public void ResetRotation()
    {
        _landTimer = landEaseTime;

    }

    public bool IsGrounded()
    {
        return controller.isGrounded;
    }

    /// <summary>
    /// Called when player hits the ground
    /// </summary>
    public void OnLand()
    {
        if ((pManager.GetEnumPlayerState() != Enums.PlayerState.Grounded) || _chargingDash)
        {
            pManager.SetPlayerState(Enums.PlayerState.Grounded);
            _anim.SetTrigger("LandTrigger");
            _am.PlaySoundOnce(AudioManager.Sound.ChadLand, transform);

            EndDash();
            ResetRotation();

        }
        
        _movementDisabledDurationTimer = 0.0f;

        _chargingJump = false;

        _velocity.y = 0.0f;

        _doubleJumpUsed = false;
        _jumpUsed = false;
    }

    /// <summary>
    /// Called when player leaves the ground
    /// </summary>
    private void OnLeaveGround()
    {
        _coyoteTimer = coyoteTime;
        CancelJumpCharge();
    }
    public void ResetVelocity()
    {
        _velocity = Vector3.zero;
    }

}
