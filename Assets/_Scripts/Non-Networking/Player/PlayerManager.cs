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
    float DOT = 100;

    ManosHand _cHand;

    [SerializeField]
    ButtonMash buttonMasher;

    CameraManager camManager;
    Animator _anim;

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

        meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;

        //if (_pState != Enums.PlayerState.Grabbed)
        {
            move.MoveUpdate(inputManager.GetLStick().x, inputManager.GetLStick().y);
            move.RotateUpdate(inputManager.GetRStick().x, turnSensitivity * Time.deltaTime);

            ManageInputs();

            CheckState();
        }
        
        if(_lastCState != _cState)
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
        if(_pState== Enums.PlayerState.Grabbed)
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
        _health.TakeDamage(DOT * Time.deltaTime);
        if(_timesPressed== breakOutPress)
        {
            _timesPressed = 0.0f;
            _cHand.ReleasePlayer(false);
            buttonMasher.Reset();
            // SetGrabbed(false);

            Debug.Log("memes");
        }
    }

    void ManageInputs()
    {

        if (inputManager.GetButtonDown(InputManager.Buttons.RB))
            _rBumperDown.execute();

        else if(inputManager.GetButtonUp(InputManager.Buttons.RB))
            _rBumperUp.execute();

        else if (inputManager.GetButtonUp(InputManager.Buttons.RB))
            _rBumperHeld.execute();



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
            

        // Debug inputs

        if (Input.GetKeyDown(KeyCode.G))
            SetGrabbed(true);

        if (Input.GetKeyDown(KeyCode.R))
            SetGrabbed(false);

        if (Input.GetKeyDown(KeyCode.K))
            GetComponent<PlayerHealth>().TakeDamage(1.0f);

        if (Input.GetKeyDown(KeyCode.S))
            camManager.GetComponent<CameraManager>().DoCameraShake(0.5f, 1.0f);

        if (Input.GetKeyDown(KeyCode.B))
            move.Knockback(new Vector3(100.0f, 100.0f, 100.0f));

        if (Input.GetKeyDown(KeyCode.R))
        {
            GetComponent<LobbyScript>().PlayersReady();
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

            if (controller.velocity.y < 0.0f)
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
            _pState = Enums.PlayerState.Grabbed;
            controller.enabled = false;
            GetComponent<Animator>().SetTrigger("Grabbed");
            meshes[0].enabled = false;
            meshes[1].enabled = false;
        }

        else
        {
            _pState = Enums.PlayerState.Falling;
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
            _pState = Enums.PlayerState.Grabbed;
            controller.enabled = false;
            GetComponent<Animator>().SetTrigger("Grabbed");
            meshes[0].enabled = false;
            meshes[1].enabled = false;
        }

        else
        {
            _pState = Enums.PlayerState.Falling;
            controller.enabled = true;
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
        bool disabled = _pState != Enums.PlayerState.Grabbed
            && _pState != Enums.PlayerState.Knocked;

        return disabled;
    }
}
