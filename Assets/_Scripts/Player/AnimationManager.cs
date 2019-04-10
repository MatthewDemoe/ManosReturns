using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    InputManager input;

    [SerializeField]
    float throwEase = 0.5f;
    float _easeNorm = 0.0f;

    float _throwTimer = 0.0f;

    bool _beginningThrow = false;
    bool _endingThrow = false;

    PlayerManager player;

    MovementManager move;

    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        move = GetComponent<MovementManager>();
        player = GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleThrowEasing();

        anim.SetFloat("x_Vel", move.GetVelocity().x);
        anim.SetFloat("z_Vel", move.GetVelocity().z);

        //anim.SetBool("Jumping", !GetComponent<CharacterController>().isGrounded);
        anim.SetFloat("Jump_Vel", move.GetVelocity().y);

        anim.SetBool("Charging_Dash", move.IsChargingDash());
        anim.SetBool("Charging_Jump", move.IsChargingJump());

        anim.SetInteger("PlayerState", player.GetPlayerState());
        anim.SetInteger("ChargingState", player.GetChargeState());
    }

    public void BeginThrow()
    {
        _throwTimer = throwEase;
        _beginningThrow = true;
    }

    public void EndThrow()
    {
        _throwTimer = throwEase;
        _endingThrow = true;
        _beginningThrow = false;
        anim.SetTrigger("ThrowTrigger");
    }

    void HandleThrowEasing()
    {
        if (_throwTimer > 0.0f)
        {
            _throwTimer -= Time.deltaTime;

            if (_beginningThrow)
                _easeNorm = UtilMath.Lmap(_throwTimer, 0.0f, throwEase, 1.0f, 0.0f);

            else 
                _easeNorm = UtilMath.Lmap(_throwTimer, 0.0f, throwEase, 0.0f, 1.0f);


            anim.SetFloat("Throwing", _easeNorm);
        }

        else
        {
            _endingThrow = false;
        }
    }
}
