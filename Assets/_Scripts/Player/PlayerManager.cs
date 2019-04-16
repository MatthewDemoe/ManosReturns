using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    float aimSpeedModifierCharging = 0.6f;

    [SerializeField]
    float aimSpeedModifierDashing = 0.1f;

    [SerializeField]
    float aimSpeedModifierThrowing = 0.5f;

    [SerializeField]
    float turnSensitivity = 1.0f;

    [SerializeField]
    InputManager inputManager;

    [SerializeField]
    ThrowTrigger throwTrigger;

    [SerializeField]
    float dashDamage = 2.0f;

    Command _aDown;
    Command _aUp;
    Command _aHeld;

    Command _bDown;
    Command _bUp;

    Command _xDown;
    Command _xUp;

    Command _rBumperDown;
    Command _rBumperUp;
    Command _rBumperHeld;

    Command _rTriggerDown;
    Command _rTriggerUp;

    Command _lTriggerDown;
    Command _lTriggerUp;
    Command _rTriggerHeld;

    JumpCmd _jumpCmd;
    DashCmd _dashCmd;
    CancelCmd _cancelCmd;
    ChargeJumpCmd _chargeJumpCmd;
    ChargeDashCmd _chargeDashCmd;

    float _elapsedTime;

    [SerializeField]
    Enums.PlayerState _pState;
    [SerializeField]
    Enums.ChargingState _cState;

    Enums.ChargingState _lastCState;

    MovementManager move;
    CharacterController controller;

    SkinnedMeshRenderer[] meshes;

    bool _bButtonPressed;

    float _timesPressed = 0.0f;

    [SerializeField]
    float breakOutPress = 15.0f;

    
    PlayerHealth _health;

    [SerializeField]
    float DOT = 50;

    ManosHand _cHand;

    [SerializeField]
    ButtonMash buttonMasher;

    CameraManager camManager;
    Animator _anim;

    TargetLockOn targetLocker;

    bool tick = false;

    [SerializeField]
    float tickTime=0.5f;

    float _timeTillTick = 0.0f;

    bool _alive = true;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();

        _pState = Enums.PlayerState.Grounded;
        _cState = Enums.ChargingState.None;
        _lastCState = _cState;

        move = GetComponent<MovementManager>();
        controller = GetComponent<CharacterController>();

        _jumpCmd = new JumpCmd(gameObject);
        _dashCmd = new DashCmd(gameObject);
        _chargeJumpCmd = new ChargeJumpCmd(gameObject);
        _chargeDashCmd = new ChargeDashCmd(gameObject);
        _cancelCmd = new CancelCmd(gameObject);

        _aUp = _jumpCmd;
        _aDown = _chargeJumpCmd;
        _aHeld = _chargeJumpCmd;

        _rBumperDown = _chargeJumpCmd;
        _rBumperUp = _jumpCmd;
        _rBumperHeld = _chargeJumpCmd;

        _bDown = _cancelCmd;
        _bUp = new NullCommand();

        _xDown = _cancelCmd;
        _xUp = new NullCommand();

        _rTriggerDown = _chargeDashCmd;
        _rTriggerUp = _dashCmd;
        _rTriggerHeld = _chargeDashCmd;

        //_lTriggerDown = _throwChargeCmd;
        //_lTriggerUp = _throwCmd;


        _health = GetComponent<PlayerHealth>();

        camManager = GameObject.Find("CameraStand").GetComponent<CameraManager>();

        targetLocker = GetComponentInChildren<TargetLockOn>();

        meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;


        move.MoveUpdate(inputManager.GetLStick().x, inputManager.GetLStick().y);
        move.RotateUpdate(inputManager.GetRStick().x, turnSensitivity * Time.deltaTime);

        ManageInputs();

        CheckState();        
        
        if (_lastCState != _cState)
        {
            switch (_cState)
            {
                case Enums.ChargingState.ChargingDash:
                    camManager.SetAimSpeed(aimSpeedModifierCharging);
                    break;

                case Enums.ChargingState.Dashing:
                    camManager.SetAimSpeed(aimSpeedModifierDashing);
                    break;
                case Enums.ChargingState.ChargingThrow:
                    camManager.SetAimSpeed(aimSpeedModifierThrowing);
                    break;
                default:
                    camManager.ResetAimSpeed();
                    break;
            }
            _lastCState = _cState;
        }

        if (_pState == Enums.PlayerState.Grabbed)
        {
            BreakOutLogic();
        }

    }

    void BreakOutLogic()
    {
        if (inputManager.GetButtonDown(InputManager.Buttons.B))
        {
            _timesPressed++;
        }

        buttonMasher.ButtonMashPrompt();
        if (_timeTillTick < tickTime)
        {
            _timeTillTick += Time.deltaTime;
        }
         

        if (_timeTillTick>=tickTime)
        {
            _health.TakeDamage(DOT);
            _timeTillTick = 0.0f;
        }

        if(_timesPressed== breakOutPress)
        {
            _timesPressed = 0.0f;
            _cHand.ReleasePlayer(false);
            buttonMasher.Reset();
            // SetGrabbed(false);

         //   Debug.Log("memes");
        }
    }

    IEnumerator CountTick()
    {

        yield return new WaitForSeconds(tickTime);
        tick = true;
    }

    void ManageInputs()
    {
        //if (inputManager.GetButtonDown(InputManager.Buttons.RB))
        //    _rBumperDown.execute();
        //
        //else if(inputManager.GetButtonUp(InputManager.Buttons.RB))
        //    _rBumperUp.execute();
        //
        //else if (inputManager.GetButtonUp(InputManager.Buttons.RB))
        //    _rBumperHeld.execute();

        if (inputManager.GetButtonDown(InputManager.Buttons.StickRight))
        {
            if (!camManager.IsLockedOn())
            {
                if (targetLocker.GetEnemyCount() > 0)
                {
                    camManager.SetLockedTarget(targetLocker.GetInitialTarget());
                    camManager.SetLockedOn(true);
                }
                else
                {
                    camManager.ResetLookOrientation();
                }
            }
            else
            {
                camManager.SetLockedOn(false);
            }
        }
        if (inputManager.RightStickFlicked() && camManager.IsLockedOn())
        {
            if (inputManager.IsRightStick(InputManager.StickDirection.Left))
            {
                camManager.SetLockedTarget(targetLocker.GetEnemyToLeft(camManager.GetLockedTarget()));
            }
            else if (inputManager.IsRightStick(InputManager.StickDirection.Right))
            {
                camManager.SetLockedTarget(targetLocker.GetEnemyToRight(camManager.GetLockedTarget()));
            }
        }

        if (inputManager.GetButtonDown(InputManager.Buttons.A))
            _aDown.execute();

        else if(inputManager.GetButtonUp(InputManager.Buttons.A))
            _aUp.execute();

        else if (inputManager.GetButton(InputManager.Buttons.A))
        {
            _aHeld.execute();
        }
           


        // Invert Look
        if (inputManager.GetButtonDown(InputManager.Buttons.Select))
        {
            camManager.ToggleLookInvert();
        }
            

        // Ready Up
        if (inputManager.GetButtonDown(InputManager.Buttons.X))
        {
            _xDown.execute();
        }
        else if (inputManager.GetButtonUp(InputManager.Buttons.X))
        {
            _xUp.execute();
        }

        // Chad Dab
        if (inputManager.GetButtonDown(InputManager.Buttons.B) || inputManager.GetButtonDown(InputManager.Buttons.LB))
        {
            move.DabOnManos();
        }

        // Chad Dash

        if (inputManager.RTrigDown() )
        {
            _rTriggerDown.execute();
        }
        else if (inputManager.RTrigUp())
        {
            _rTriggerUp.execute();
        }
        else if (inputManager.RTrigDown())
        {
            _rTriggerDown.execute();
        }



        // Football charge and throw

        if (inputManager.LTrigDown())
        {
            throwTrigger.AttemptChargeFootball();
        }
            

        if (inputManager.LTrigUp())
        {
            throwTrigger.ThrowFootball();
        }
            

        if (inputManager.GetTriggerL() < 0.25f)
        {
            throwTrigger.ThrowFootball();
        }

        if (CoolDebug.hacks)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                SetGrabbed(true);
                CoolDebug.GetInstance().LogHack("Test Grab");
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                SetGrabbed(false);
                CoolDebug.GetInstance().LogHack("Test Release");
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                camManager.GetComponent<CameraManager>().DoCameraShake(0.5f);
                CoolDebug.GetInstance().LogHack("Camera Shake!");
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                move.Knockback(new Vector3(0.0f, -50.0f, -50.0f));
                CoolDebug.GetInstance().LogHack("Test Chad Knockback!");
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                GameObject.Find("OverlordController").GetComponent<LobbyScript>().PlayersReady();
                CoolDebug.GetInstance().LogHack("Readyup!");
            }
                
        }
    }

    public float GetDamage()
    {
        return dashDamage;
    }

    void CheckState()
    {
        if (_pState == Enums.PlayerState.Grabbed)
            return;

        else
        {
            if (_pState == Enums.PlayerState.Knocked)
            {
                return;
            }

            if (move.GetVelocity().y < 0.0f)
            {

                if (_pState != Enums.PlayerState.Grounded)
                {
                    _pState = Enums.PlayerState.Falling;
                }
            }
        }

        if (_cState == Enums.ChargingState.Dashing)
        {
            if (move.GetDashDuration() <= 0.0f)
            {
                move.EndDash();

                _cState = (Enums.ChargingState.None);
            }
        }

        else if (_cState == Enums.ChargingState.Dabbing)
            return;

        else if (_cState == Enums.ChargingState.Flip)
        {
            if (_pState == Enums.PlayerState.Grounded)
            {
                _cState = Enums.ChargingState.None;
            }
        }

        else if (throwTrigger.IsCharging())
        {
            _cState = Enums.ChargingState.ChargingThrow;

        }

        else
        {
            if (move.IsChargingDash())
                _cState = Enums.ChargingState.ChargingDash;

            else if (move.IsChargingJump())
                _cState = Enums.ChargingState.ChargingJump;

            else
                _cState = Enums.ChargingState.None;
        }
    }

    public int GetPlayerState()
    {
        return (int)_pState;
    }

    public Enums.PlayerState GetEnumPlayerState()
    {
        return _pState;
    }

    public int GetChargeState()
    {
        return (int)_cState;
    }

    public Enums.ChargingState GetEnumChargeState()
    {
        return _cState;
    }

    public void SetChargeState(Enums.ChargingState state)
    {
        _cState = state;
    }

    public void SetPlayerState(Enums.PlayerState state)
    {
        _pState = state;
    }

    public void SetGrabbed(bool g)
    {
        if (g)
        {
            move.Cancel();

            _pState = Enums.PlayerState.Grabbed;
            controller.enabled = false;
            _anim.SetTrigger("Grabbed");
            meshes[0].enabled = false;
            meshes[1].enabled = false;
        }

        else
        {
            _pState = Enums.PlayerState.Grounded;
            controller.enabled = true;
            meshes[0].enabled = true;
            meshes[1].enabled = true;
            buttonMasher.Reset();
        }
    }

    public void SetGrabbed(bool g, ManosHand hand)
    {
        _cHand = hand;

        if (g)
        {
            move.Cancel();

            _pState = Enums.PlayerState.Grabbed;
            //controller.enabled = false;
            _anim.SetTrigger("Grabbed");
            meshes[0].enabled = false;
            meshes[1].enabled = false;
        }

        else
        {
            _pState = Enums.PlayerState.Grounded;
            //controller.enabled = true;
            meshes[0].enabled = true;
            meshes[1].enabled = true;
            buttonMasher.Reset();
        }  
    }

    public bool IsPlayerControllable()
    {
        return _pState != Enums.PlayerState.Grabbed && _pState != Enums.PlayerState.Knocked;
    }

    public bool IsInDisabledState()
    {
        bool disabled = _pState == Enums.PlayerState.Grabbed
            || _pState == Enums.PlayerState.Knocked;

        return disabled;
    }

    public void SetAlive(bool a)
    {
        _alive = a;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (_pState == Enums.PlayerState.Knocked)
        {
            if (hit.transform.CompareTag("Ground"))
            {               
                move.OnHitEnvironment(hit.normal);
            }
        }
    }
}
