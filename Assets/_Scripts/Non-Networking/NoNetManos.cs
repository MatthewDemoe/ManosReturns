using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoNetManos : MonoBehaviour
{
    /// <summary>
    /// Maximum time for the player to struggle free from Manos' grip
    /// </summary>
    [Header("Gameplay Properties")]
    [SerializeField]
    [Tooltip("Maximum time for the player to struggle free from Manos' grip")]
    float gripDuration;

    /// <summary>
    /// Time Manos must rest his hand before grabbing a player again
    /// </summary>
    [SerializeField]
    [Tooltip("Time Manos must rest his hand before grabbing a player again")]
    float gripCooldown;

    /// <summary>
    /// Time it takes for Manos to register a pose
    /// </summary>
    [SerializeField]
    [Tooltip("Time it takes for Manos to register a pose")]
    float chargeTime;

    /// <summary>
    /// Time it takes for Manos charge another bullet after shooting once
    /// </summary>
    [SerializeField]
    [Tooltip("Time it takes for Manos charge another bullet after shooting once")]
    float rapidFireTime;

    /// <summary>
    /// Time Manos can move freely after posing
    /// </summary>
    [SerializeField]
    [Tooltip("Time Manos can move freely after posing")]
    float freeMoveTime;

    /// <summary>
    /// Manos moves this slow at the peak of his pose
    /// </summary>
    [SerializeField]
    [Tooltip("Manos moves this slow at the peak of his pose")]
    float lerpMin;

    /// <summary>
    /// Manos' limbs move this slowly
    /// </summary>
    [SerializeField]
    [Tooltip("Manos' limbs move this slowly")]
    float lerpMax;

    /// <summary>
    /// Speed at which Manos moves freely
    /// </summary>
    [SerializeField]
    [Tooltip("Speed at which Manos moves freely")]
    float lerpReleased;

    /// <summary>
    /// Manos' limbs shake this much during his pose
    /// </summary>
    [SerializeField]
    [Tooltip("Manos' limbs shake this much during his pose")]
    float shakeFactor;

    /// <summary>
    /// Distance Manos can move before punch ends
    /// </summary>
    [SerializeField]
    [Tooltip("Distance Manos can move before punch ends")]
    float punchDistance;

    /// <summary>
    /// Distance Manos can move before grab ends
    /// </summary>
    [SerializeField]
    [Tooltip("Distance Manos can move before grab ends")]
    float grabDistance;

    /// <summary>
    /// Distance Manos can move before gun ends
    /// </summary>
    [SerializeField]
    [Tooltip("Distance Manos can move before gun ends")]
    float gunDistance;

    /// <summary>
    /// Manos' limbs shake this much during his pose
    /// </summary>
    [SerializeField]
    [Tooltip("Manos' limbs shake this much during his pose")]
    float angleTolerance;

    /// <summary>
    /// Time bullet lasts before disabling itself
    /// </summary>
    [SerializeField]
    [Tooltip("Time bullet lasts before disabling itself")]
    float bulletLife;

    /// <summary>
    /// Speed of the bullet
    /// </summary>
    [SerializeField]
    [Tooltip("Speed of the bullet")]
    float bulletSpeed;

    /// <summary>
    /// Damage Manos bullet deals to player
    /// </summary>
    [SerializeField]
    [Tooltip("Damage Manos bullet deals to player")]
    int bulletDamage;

    /// <summary>
    /// Time it takes arms to reactivate in seconds
    /// </summary>
    [SerializeField]
    [Tooltip("Time it takes arms to reactivate in seconds")]
    float armRepairTime;

    [Header("Object References")]
    [SerializeField]
    public GameObject selfCamera;
    [SerializeField]
    ManosUI manosUI;

    [Header("Syncvars Left")]
    [SerializeField]
    bool handVisibleL;
    [SerializeField]
    bool fistActioningL;
    [SerializeField]
    bool gunActioningL;
    [SerializeField]
    bool triggerActioningL;
    [SerializeField]
    bool poweredL;
    [SerializeField]
    bool posedOnceL;
    [SerializeField]
    float chargeTimerL;

    [Header("Syncvars Right")]
    [SerializeField]
    bool handVisibleR;
    [SerializeField]
    bool fistActioningR;
    [SerializeField]
    bool gunActioningR;
    [SerializeField]
    bool triggerActioningR;
    [SerializeField]
    bool poweredR;
    [SerializeField]
    bool posedOnceR;
    [SerializeField]
    float chargeTimerR;

    public static GameObject manos;

    PlayerHealth health;

    // Use this for initialization
    void Start()
    {
        manos = gameObject;
    }

    private void OnEnable()
    {
        //if (GameObject.Find("OverlordController").GetComponent<PlayerStateManager>().activePlayers == 0)
        //{
        //    if (pui == null)
        //        pui = GameObject.Find("PlayerUI(Clone)").GetComponent<PlayerUserInterface>();
        //
        //    pui.RegisterPlayer(GetComponent<PlayerHealth>());
        //
        //    pui.KillYouAreSelf();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //F for manos health debug
        if (Input.GetKeyDown(KeyCode.F))
        {
            GetComponent<PlayerHealth>().TakeDamage(10);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {

        }
    }

    public void EnableVR()
    {
        GameObject theCam = GameObject.Find("Default Camera");

        if (theCam)
        {
            theCam.SetActive(false);
        }
    }

    public bool IsHandVisible(Enums.Hand theHand)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                return handVisibleL;
            default:
                return handVisibleR;
        }
    }

    public void SetHandVisible(Enums.Hand theHand, bool b)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                handVisibleL = b;
                break;
            default:
                handVisibleR = b;
                break;
        }

        manosUI.SetHandStatus(theHand, b);
    }

    public bool IsFistActioning(Enums.Hand theHand)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                return fistActioningL;
            default:
                return fistActioningR;
        }
    }

    public void SetFistActioning(Enums.Hand theHand, bool b)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                fistActioningL = b;
                break;
            default:
                fistActioningR = b;
                break;
        }
    }

    public bool IsGunActioning(Enums.Hand theHand)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                return gunActioningL;
            default:
                return gunActioningR;
        }
    }

    public void SetGunActioning(Enums.Hand theHand, bool b)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                gunActioningL = b;
                break;
            default:
                gunActioningR = b;
                break;
        }
    }

    public bool IsTriggerActioning(Enums.Hand theHand)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                return triggerActioningL;
            default:
                return triggerActioningR;
        }
    }

    public void SetTriggerActioning(Enums.Hand theHand, bool b)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                triggerActioningL = b;
                break;
            default:
                triggerActioningR = b;
                break;
        }
    }

    public bool IsFistPowered(Enums.Hand theHand)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                return poweredL;
            default:
                return poweredR;
        }
    }

    public void SetFistPowered(Enums.Hand theHand, bool b)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                poweredL = b;
                break;
            default:
                poweredR = b;
                break;
        }
    }

    public bool PosedOnce(Enums.Hand theHand)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                return posedOnceL;
            default:
                return posedOnceR;
        }
    }

    public void SetPosedOnce(Enums.Hand theHand, bool b)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                posedOnceL = b;
                break;
            default:
                posedOnceR = b;
                break;
        }
    }

    public float GetChargeTimer(Enums.Hand theHand)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                return chargeTimerL;
            default:
                return chargeTimerR;
        }
    }

    public void ResetChargeTimer(Enums.Hand theHand)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                chargeTimerL = 0;
                break;
            default:
                chargeTimerR = 0;
                break;
        }
    }

    /// <summary>
    /// Increment charge timer by delta time
    /// </summary>
    /// <param name="theHand">
    /// Left or right hand
    /// </param>
    public void TickChargeTimer(Enums.Hand theHand)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                chargeTimerL += Time.deltaTime;
                break;
            default:
                chargeTimerR += Time.deltaTime;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //  if (other.CompareTag("PlayerBullet"))
        //  {
        //      health.TakeDamage(10);
        //  }
    }

    public float GetGripDuration()
    {
        return gripDuration;
    }

    public float GetGripCooldown()
    {
        return gripCooldown;
    }

    public float GetPoseChargeTime()
    {
        return chargeTime;
    }

    public float GetRapidFireTime()
    {
        return rapidFireTime;
    }

    public float GetFreeMoveTime()
    {
        return freeMoveTime;
    }

    public float GetInterpMin()
    {
        return lerpMin;
    }

    public float GetInterpMax()
    {
        return lerpMax;
    }

    public float GetInterpReleased()
    {
        return lerpReleased;
    }

    public float GetShakeFactor()
    {
        return shakeFactor;
    }

    public float GetAngleTolerance()
    {
        return angleTolerance;
    }

    public float GetPunchDistance()
    {
        return punchDistance;
    }

    public float GetGrabDistance()
    {
        return grabDistance;
    }

    public float GetGunDistance()
    {
        return gunDistance;
    }

    public float GetBulletLifeTime()
    {
        return bulletLife;
    }

    public float GetBulletSpeed()
    {
        return bulletSpeed;
    }

    public int GetBulletDamage()
    {
        return bulletDamage;
    }

    public float GetArmRepairTime()
    {
        return armRepairTime;
    }
}
