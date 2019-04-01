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

    [Header("Syncvars Both")]
    [SerializeField]
    bool canGrab;

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
    bool chargeDecayL;

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
    bool chargeDecayR;

    [SerializeField]
    ManosHand leftHand;
    [SerializeField]
    ManosHand rightHand;

    FlashFeedback flash;

    public static GameObject manos;

    PlayerHealth health;

    AnimationState animLeft;
    AnimationState animRight;

    Animation anim;
    Animation anime;

    [SerializeField]
    MeshRenderer vBigL;
    [SerializeField]
    MeshRenderer vBigR;

    // Use this for initialization
    void Start()
    {
        manos = gameObject;

        if (leftHand == null)
        {
            ManosHand[] m = GetComponentsInChildren<ManosHand>();

            leftHand = m[0];
            rightHand = m[1];
        }

        flash = GetComponent<FlashFeedback>();

        anim = GetComponent<Animation>();
        anime = transform.GetChild(0).GetComponent<Animation>();

        animLeft = anim["ManosInterpLeftArm"];
        animRight = anime["ManosInterpRightArm"];

        canGrab = true;
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

        if (chargeDecayL)
        {
            chargeTimerL -= Time.deltaTime;
        }
        if (chargeDecayR)
        {
            chargeTimerR -= Time.deltaTime;
        }
    }

    public void DecayCharge(Enums.Hand h, bool b)
    {
        switch (h)
        {
            case Enums.Hand.Left:
                chargeDecayL = b;
                if (b)
                {
                    chargeTimerL = freeMoveTime;
                }
                break;
            case Enums.Hand.Right:
                chargeDecayR = b;
                if (b)
                {
                    chargeTimerR = freeMoveTime;
                }
                break;
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
            case Enums.Hand.Right:
                return handVisibleR;
        }
        return false;
    }

    public void SetHandVisible(Enums.Hand theHand, bool b)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                handVisibleL = b;
                break;
            case Enums.Hand.Right:
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
            case Enums.Hand.Right:
                return fistActioningR;
        }
        return false;
    }

    public void SetFistActioning(Enums.Hand theHand, bool b)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                fistActioningL = b;
                break;
            case Enums.Hand.Right:
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
            case Enums.Hand.Right:
                return gunActioningR;
        }
        return false;
    }

    public void SetGunActioning(Enums.Hand theHand, bool b)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                gunActioningL = b;
                break;
            case Enums.Hand.Right:
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
            case Enums.Hand.Right:
                return triggerActioningR;
        }
        return false;
    }

    public void SetTriggerActioning(Enums.Hand theHand, bool b)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                triggerActioningL = b;
                break;
            case Enums.Hand.Right:
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
            case Enums.Hand.Right:
                return poweredR;
        }
        return false;
    }

    public void SetFistPowered(Enums.Hand theHand, bool b)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                poweredL = b;
                break;
            case Enums.Hand.Right:
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
            case Enums.Hand.Right:
                return posedOnceR;
        }
        return false;
    }

    public void SetPosedOnce(Enums.Hand theHand, bool b)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                posedOnceL = b;
                break;
            case Enums.Hand.Right:
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
            case Enums.Hand.Right:
                return chargeTimerR;
        }
        return 0f;
    }

    public void ResetChargeTimer(Enums.Hand theHand)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                chargeTimerL = 0;
                animLeft.time = 0;
                anim.Stop();
                vBigL.enabled = false;
                break;
            case Enums.Hand.Right:
                chargeTimerR = 0;
                animRight.time = 0;
                anime.Stop();
                vBigR.enabled = false;
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
            case Enums.Hand.Right:
                chargeTimerR += Time.deltaTime;
                break;
        }
    }

    public void UpdateChargeAnimation(Enums.Hand theHand)
    {
        switch (theHand)
        {
            case Enums.Hand.Left:
                animLeft.time = chargeTimerL;
                anim.Play();
                if (!vBigL.enabled && animLeft.time > 0.1f) vBigL.enabled = true;
                break;
            case Enums.Hand.Right:
                animRight.time = chargeTimerR;
                anime.Play();
                if (!vBigR.enabled && animRight.time > 0.1f) vBigR.enabled = true;
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

    public bool CanGrab()
    {
        return canGrab;
    }

    public void CooldownGrip()
    {
        canGrab = false;
        StartCoroutine(GripCooldown());
    }

    IEnumerator GripCooldown()
    {
        yield return new WaitForSeconds(gripCooldown);
        canGrab = true;
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

    public bool DealDamageToArm(Enums.Hand hand, float amount)
    {
        bool damageDealt = false;

        Enums.ManosParts m = Enums.ManosParts.None;

        switch (hand)
        {
            case Enums.Hand.Left:
                damageDealt = leftHand.TakeDamage(amount);
                m = Enums.ManosParts.LeftVambrace;
                break;
            case Enums.Hand.Right:
                damageDealt = rightHand.TakeDamage(amount);
                m = Enums.ManosParts.RightVambrace;
                break;
        }

        if (damageDealt) flash.ReactToDamage(0.0f, m);
        return damageDealt;
    }

    public FlashFeedback GetFlasher()
    {
        return flash;
    }
}
