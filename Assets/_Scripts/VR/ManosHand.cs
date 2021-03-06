﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ManosHand : MonoBehaviour
{
    [SerializeField]
    SteamVR_Action_Boolean fistAction;

    [SerializeField]
    SteamVR_Action_Boolean gunAction;

    [SerializeField]
    SteamVR_Action_Boolean triggerAction;

    [SerializeField]
    SteamVR_Action_Vibration haptics;

    [SerializeField]
    Enums.Hand thisHand;

    [Header("Object References")]

    // A reference to the object being tracked. In this case, a controller.
    [SerializeField]
    GameObject realController;

    [SerializeField]
    GameObject trueHand;

    [SerializeField]
    Manos manos;

    // Stores the GameObject that the trigger is currently colliding with, 
    // so you have the ability to grab the object.
    [SerializeField]
    private GameObject collidingObject;
    // Serves as a reference to the GameObject that the player is currently grabbing.
    [SerializeField]
    private GameObject objectInHand;

    SteamVR_Behaviour_Skeleton skeltal;
    SteamVR_Behaviour_Skeleton skeltalBig;

    [SerializeField]
    ParticleSystem fireHands;

    [SerializeField]
    ParticleSystem charging;

    [SerializeField]
    ParticleSystem gunCharging;

    [SerializeField]
    Transform wristTransform;

    [SerializeField]
    GameObject energyFist;

    [SerializeField]
    UnityEngine.UI.Text hpText;

    [SerializeField]
    UnityEngine.UI.Image hpBar;
    [SerializeField]
    UnityEngine.UI.Image radialBar;

    Material mat;
    [SerializeField]
    MeshRenderer vambrace;
    Material vMat;

    ManosBullet bullet;

    GameObject gunBarrel;

    SlowLimbInterp interp;
    VelocityInterp vInterp;

    Camera cam;

    Transform headTransform;

    Collider col;

    Rigidbody rb;

    AudioManager am;

    PlayerHealth pHealth;

    [SerializeField]
    SlowGenericInterp gauntlet;

    [Header("Gameplay Properties")]

    [SerializeField]
    float handHealthMax;
    [SerializeField]
    float handHealth;

    /// <summary>
    /// Rate at which health regenerates, health per second
    /// </summary>
    [SerializeField]
    [Tooltip("Rate at which health regenerates, health per second")]
    float healthRegenRate;

    [SerializeField]
    float manosDamageModifier = 1f;

    [SerializeField]
    bool armDisabled;

    [SerializeField]
    Enums.Poses currentPose;

    [SerializeField]
    Enums.Poses powerPose;

    [SerializeField]
    float realSpeed;

    Vector3 prevPos;

    [SerializeField]
    float travelDistance;

    [SerializeField]
    bool rapidFire;

    [SerializeField]
    Vector3 grabOffset;

    GameObject chadMesh;

    float prevAxis = 0;

    [SerializeField]
    Vector3 velocity;

    bool routineOnce;

    [SerializeField]
    ManosBullet bulletPrefab;

    [SerializeField]
    GameObject manosGrabPrefab;

    [SerializeField]
    GameObject manosGrabSuccessPrefab;

    [SerializeField]
    ParticleSystem[] armExplodePrefabs;

    bool _training = true;

    [SerializeField]
    bool flashing;

    [SerializeField]
    List<Collider> troublesomeCol;

    [SerializeField]
    DamageNumbers damageNumbers;

    void Awake()
    {
        col = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        skeltal = GetComponent<SteamVR_Behaviour_Skeleton>();
        skeltalBig = energyFist.GetComponent<SteamVR_Behaviour_Skeleton>();

        fistAction.AddOnChangeListener(OnFistActionChange, skeltal.inputSource);

        gunAction.AddOnChangeListener(OnGunActionChange, skeltal.inputSource);

        triggerAction.AddOnChangeListener(OnTriggerActionChange, skeltal.inputSource);

        prevPos = transform.position;
    }

    private void OnDisable()
    {
        if (fistAction != null)
        {
            fistAction.RemoveOnChangeListener(OnFistActionChange, skeltal.inputSource);
        }

        if (gunAction != null)
        {
            gunAction.RemoveOnChangeListener(OnGunActionChange, skeltal.inputSource);
        }

        if (triggerAction != null)
        {
            triggerAction.RemoveOnChangeListener(OnTriggerActionChange, skeltal.inputSource);
        }
    }

    private void OnTriggerActionChange(SteamVR_Action_In actionIn)
    {
        if (triggerAction.GetStateDown(skeltal.inputSource))
        {
            if (!armDisabled)
            {
                foreach (Collider c in troublesomeCol)
                {
                    c.isTrigger = true;
                }
            }
            if (powerPose == Enums.Poses.Gun && manos.IsFistPowered(thisHand) && !manos.IsFistActioning(thisHand))
            {
                //if (manos.IsTriggerActioning(thisHand))
                //{
                    FireBullet();
                //}
            }

        }
        else if (triggerAction.GetStateUp(skeltal.inputSource))
        {
            if (!armDisabled)
            {
                foreach (Collider c in troublesomeCol)
                {
                    c.isTrigger = false;
                }
            }
        }
    }

    private void OnFistActionChange(SteamVR_Action_In actionIn)
    {
        if (fistAction.GetStateDown(skeltal.inputSource) && !armDisabled)
        {
            if (!manos.PosedOnce(thisHand) && !manos.IsFistPowered(thisHand))
            {
                if (!am.IsSoundPlaying(AudioManager.Sound.ManosCharging, transform))
                    am.PlaySoundOnce(AudioManager.Sound.ManosCharging, transform);
                Instantiate(manosGrabPrefab, transform.GetChild(0).position, Quaternion.identity);
                am.PlaySoundOnce(AudioManager.Sound.ManosGrab);
            }
        }

        if (fistAction.GetStateUp(skeltal.inputSource))
        {
            if (manos.PosedOnce(thisHand))
            {
                manos.SetPosedOnce(thisHand, false);
                am.StopSound(AudioManager.Sound.ManosCharging, transform);
                CancelCharge();
            }
            else if (!manos.IsFistPowered(thisHand))
            {
                am.StopSound(AudioManager.Sound.ManosCharging, transform);
                CancelCharge();
            }
        }
    }

    void OnGunActionChange(SteamVR_Action_In actionIn)
    {
        if (gunAction.GetStateDown(skeltal.inputSource) && !manos.IsFistActioning(thisHand) && !armDisabled)
        {
            if (!manos.PosedOnce(thisHand) && !manos.IsFistPowered(thisHand))
            {
                am.PlaySoundOnce(AudioManager.Sound.ManosCharging, transform);
            }
        }

        if (gunAction.GetStateUp(skeltal.inputSource))
        {
            if (manos.PosedOnce(thisHand) || !manos.IsFistPowered(thisHand))
            {
                manos.SetPosedOnce(thisHand, false);
                am.StopSound(AudioManager.Sound.ManosCharging, transform);
                rapidFire = false;
                CancelCharge();
            }
        }
    }

    private void Start()
    {
        interp = GetComponent<SlowLimbInterp>();
        vInterp = GetComponent<VelocityInterp>();
        rb = GetComponent<Rigidbody>();
        am = AudioManager.GetInstance();
        SetParticlePower(0);

        cam = manos.selfCamera.GetComponent<Camera>();

        bullet = Instantiate(bulletPrefab);

        handHealth = handHealthMax;

        pHealth = GetComponentInParent<PlayerHealth>();

        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        vMat = vambrace.material;

        gunBarrel = transform.GetChild(transform.childCount - 3).gameObject;

        bullet.Init(transform.GetChild(transform.childCount - 2), manos);
        bullet.SetLifeTime(manos.GetBulletLifeTime());
        bullet.SetSpeed(manos.GetBulletSpeed());

        chadMesh = transform.GetChild(transform.childCount - 1).gameObject;
    }

    public void SetCollidingObject(Collider col)
    {
        try
        {
            if (col == null)
            {
                collidingObject = null;
            }
            else
            {
                collidingObject = col.gameObject;
            }
        }
        catch { }
    }

    // Update is called once per frame
    void Update()
    {
        if (CoolDebug.hacks)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                TakeDamage(50);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                DisableArm();
                CoolDebug.GetInstance().LogHack("Disabled Arms!");
            }
        }

        UpdateSyncVars();

        //We no longer care about this mechanic
        //UpdateHandVisiblity();

        if (!armDisabled)
        {
            if (objectInHand == null)
                CheckActions();
           
            CheckGrab();

            //Gem is no longer a thing :(
            //CheckGrabGem();

            CheckPoses();

            if (healthRegenRate != 0)
            {
                ModifyArmHealth(healthRegenRate * Time.deltaTime);
            }

            manos.UpdateChargeAnimation(thisHand);
        }

        realSpeed = velocity.magnitude;

        velocity = (transform.position - prevPos) / Time.deltaTime;

        prevPos = transform.position;
    }

    /// <summary>
    /// Updates our syncvars every frame depending on the state of our actions
    /// </summary>
    void UpdateSyncVars()
    {
        if (fistAction.GetState(skeltal.inputSource)) manos.SetFistActioning(thisHand, true);
        else manos.SetFistActioning(thisHand, false);

        if (gunAction.GetState(skeltal.inputSource) && !manos.IsFistActioning(thisHand)) manos.SetGunActioning(thisHand, true);
        else manos.SetGunActioning(thisHand, false);

        if (triggerAction.GetState(skeltal.inputSource)) manos.SetTriggerActioning(thisHand, true);
        else manos.SetTriggerActioning(thisHand, false);
    }

    void CheckActions()
    {
        if ((currentPose != Enums.Poses.None && !manos.IsFistPowered(thisHand)) && !manos.PosedOnce(thisHand))
        {
            if (manos.IsTriggerActioning(thisHand) && currentPose == Enums.Poses.Gun && manos.IsGunActioning(thisHand))
            {
                bool r = rapidFire; //Keep rapid fire state past the cancel
                CancelCharge();
                rapidFire = r;
                return;
            }
            if (!flashing)
            {
                manos.GetFlasher().ManosChargeActivate(thisHand);
                flashing = true;
            }
            manos.TickChargeTimer(thisHand);

            float power;
            if (rapidFire && currentPose == Enums.Poses.Gun)
                // Magic numbers right now
                power = Mathf.Clamp(manos.GetChargeTimer(thisHand) / manos.GetRapidFireTime(), 0, 1);
            else
                power = Mathf.Clamp(manos.GetChargeTimer(thisHand) / manos.GetPoseChargeTime(), 0, 1);
            float frequency = (power >= 1) ? 100 : Mathf.Lerp(0, 50, power);
            float amp = (power >= 1) ? 0.8f : Mathf.Lerp(0, 0.2f, power);
            try
            {
                haptics.Execute(0, Time.deltaTime, frequency, amp, skeltal.inputSource);
            } catch { }

            SetParticlePower(power);

            if (power < 1 && !rapidFire)
            {
                interp.SetLerp(Mathf.Lerp(manos.GetInterpMax(), manos.GetInterpMin(), power));
            }
            else if (rapidFire)
            {
                interp.SetLerp(manos.GetInterpMax());
            }
        }
        else if (currentPose == Enums.Poses.None && !manos.IsFistPowered(thisHand)) {
            manos.GetFlasher().ManosChargeDeactivate(thisHand, handHealth / handHealthMax);
            flashing = false;
            SetParticlePower(0);
            //Don't need to cancel charge
        }
        else if (currentPose == Enums.Poses.None && manos.PosedOnce(thisHand))
        {
            manos.SetPosedOnce(thisHand, false);
            CancelCharge();
        }

        //if ((
        //    (!manos.IsFistActioning(thisHand) && powerPose == Enums.Poses.Punch) || 
        //    (!manos.IsGunActioning(thisHand) && powerPose == Enums.Poses.Gun)
        //    ) && manos.PosedOnce(thisHand) && !manos.IsFistPowered(thisHand))
        //{
        //    CancelCharge();
        //    print("States Up!");
        //    powerPose = Enums.Poses.None;
        //}

        if (manos.IsFistPowered(thisHand))
        {
            travelDistance -= Vector3.Distance(transform.position, prevPos);
            if (travelDistance <= 0)
            {
                CancelCharge();
                print("Travelled!");
            }
        }
    }

    void CheckGrab()
    {
        if (manos.IsFistActioning(thisHand))
        {
            if (collidingObject)
            {
                if (!collidingObject.CompareTag("Gem"))
                {
                    if (collidingObject.CompareTag("Player") && fistAction.GetStateDown(skeltal.inputSource) && manos.CanGrab() && !manos.PlayerGrabbed())
                    {
                        GrabPlayer();
                    }
                }
            }
            else if (!manos.IsFistActioning(thisHand))
            {
                if (objectInHand != null)
                {
                    ReleasePlayer();
                }
            }
        }
        else
        {
            if (objectInHand != null)
            {
                if (!objectInHand.CompareTag("Gem"))
                {
                    ReleasePlayer();
                }
            }
        }
    }

    void CheckGrabGem()
    {
        if (manos.IsFistActioning(thisHand))
        {
            if (collidingObject)
            {
                if (collidingObject.CompareTag("Gem"))
                {
                    collidingObject.GetComponent<GemBehaviour>().Grab();
                    collidingObject.transform.parent = transform;
                    objectInHand = collidingObject;
                }
            }
        }
        else
        {
            if (objectInHand)
            {
                if (objectInHand.CompareTag("Gem"))
                {
                    objectInHand.GetComponent<GemBehaviour>().Release();
                    objectInHand = null;
                }
            }
        }
    }

    /// <summary>
    /// Sets the state of hand particles
    /// </summary>
    /// <param name="p">
    /// From 0 (min) to 1 (max)
    /// </param>
    void SetParticlePower(float p)
    {
        if (p >= 1)
        {
            ParticleSystem.EmissionModule emission;
            switch (currentPose)
            {
                case Enums.Poses.Punch:
                    var main = fireHands.main;
                    emission = fireHands.emission;

                    main.startLifetime = Mathf.Lerp(0, 2.5f, p);
                    emission.rateOverTime = Mathf.Lerp(0, 90, p);
                    interp.SetShakeFactor(0);

                    fireHands.Play();

                    break;
            }

            emission = charging.emission;
            emission.rateOverTime = 0;

            if (!routineOnce)
            {
                routineOnce = true;
                StartCoroutine("PowerPose");
            }
        }
        else
        {
            switch (currentPose)
            {
                case Enums.Poses.Punch:
                    var main = charging.main;
                    var emission = charging.emission;
                    emission.rateOverTime = Mathf.Lerp(25, 100, p);
                    gunCharging.gameObject.SetActive(false);
                    break;
                case Enums.Poses.Gun:
                    gunCharging.gameObject.SetActive(true);
                    break;
            }

            interp.SetShakeFactor(Mathf.Lerp(0, manos.GetShakeFactor(), p));
        }

        if (p == 0)
        {
            var main = fireHands.main;
            var emission = fireHands.emission;

            main.startLifetime = 0;
            emission.rateOverTime = 0;

            emission = charging.emission;
            emission.rateOverTime = 0;

            gunCharging.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Sets state of our fist to be powered
    /// </summary>
    /// <returns></returns>
    IEnumerator PowerPose()
    {
        //am.PlaySoundLoop(AudioManager.Sound.IntenseFire, transform, AudioManager.Priority.Spam);
        manos.GetFlasher().ManosChargeDeactivate(thisHand, handHealth / handHealthMax);
        am.StopSound(AudioManager.Sound.ManosCharging, transform);
        manos.SetFistPowered(thisHand, true);
        interp.SetLerp(manos.GetInterpReleased());
        powerPose = currentPose;
        manos.DecayCharge(thisHand, true);

        switch (currentPose)
        {
            case Enums.Poses.Punch:
                travelDistance = manos.GetPunchDistance();
                energyFist.SetActive(true);
                am.PlaySoundLoop(AudioManager.Sound.ManosAura, transform);
                //interp.SetOffsetActive(true);
                //transform.localScale = new Vector3(chargedSize, chargedSize, chargedSize);
                break;
            case Enums.Poses.Grab:
                travelDistance = manos.GetGrabDistance();
                break;
            case Enums.Poses.Gun:
                bullet.gameObject.SetActive(true);
                bullet.EnableLaser();
                gunBarrel.SetActive(true);
                travelDistance = manos.GetGunDistance();
                am.PlaySoundLoop(AudioManager.Sound.ManosGunActive, transform, AudioManager.Priority.Spam);
                break;
        }

        yield return new WaitForSeconds(manos.GetFreeMoveTime());
        //print("YIELDED");

        switch (powerPose)
        {
            case Enums.Poses.Punch:
                if (manos.IsFistActioning(thisHand))
                    manos.SetPosedOnce(thisHand, true);
                break;
            case Enums.Poses.Grab:
                if (manos.IsFistActioning(thisHand))
                    manos.SetPosedOnce(thisHand, true);
                break;
            case Enums.Poses.Gun:
                //if (manos.IsGunActioning(thisHand))
                //manos.SetPosedOnce(thisHand, true);
                bullet.gameObject.SetActive(false);
                gunBarrel.SetActive(false);
                break;
        }

        am.StopSoundLoop(AudioManager.Sound.ManosGunActive, true, transform);
        am.StopSoundLoop(AudioManager.Sound.ManosAura, true, transform);

        routineOnce = false;

        CancelCharge();
    }

    public void FireBullet()
    {
        try
        {
            haptics.Execute(0, Time.deltaTime, 1, 1, skeltal.inputSource);
        }
        catch{}
        gunBarrel.SetActive(false);
        am.StopSoundLoop(AudioManager.Sound.ManosGunActive, true, transform);
        am.PlaySoundOnce(AudioManager.Sound.ManosBullet, transform);
        bullet.FireBullet();
        CancelCharge();
        if (manos.IsGunActioning(thisHand))
        {
            rapidFire = true; //This overwrites the rapidFire = false in CancelCharge
        }
    }

    /// <summary>
    /// Updates currentPose with the hand's pose every frame
    /// </summary>
    void CheckPoses()
    {
        if (!manos.IsFistPowered(thisHand))
        {
            if (manos.IsFistActioning(thisHand) /*&& IsPointingAt(Vector3.up)*/ /*&& manos.IsHandVisible(thisHand) */&& interp.enabled)
                currentPose = Enums.Poses.Punch;
            else if (manos.IsGunActioning(thisHand) /*&& manos.IsHandVisible(thisHand)*/ && interp.enabled)
                currentPose = Enums.Poses.Gun;
            else
                currentPose = Enums.Poses.None;
        }
    }

    /// <summary>
    /// Does funky math, returns true if you're pointing at the direction
    /// </summary>
    /// <param name="theDir">
    /// The direction you're checking if the hand is pointing at
    /// </param>
    /// <returns></returns>
    public bool IsPointingAt(Vector3 theDir)
    {
        float angleBetween = Vector3.Angle(wristTransform.forward, theDir.normalized);
        if (angleBetween < manos.GetAngleTolerance())
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if your hand is within the camera view matrix.
    /// </summary>
    void UpdateHandVisiblity()
    {
        Vector3 handPos = cam.WorldToViewportPoint(transform.position);
        if (handPos.x > 0 && handPos.x < 1 &&
            handPos.y > 0 && handPos.y < 1 &&
            handPos.z > 0)
        {
            manos.SetHandVisible(thisHand, true);
            return;
        }
        manos.SetHandVisible(thisHand, false);
    }

    void CancelCharge()
    {
        StopCoroutine("PowerPose");

        manos.DecayCharge(thisHand, false);
        manos.SetFistPowered(thisHand, false);
        manos.ResetChargeTimer(thisHand);

        manos.GetFlasher().ManosChargeDeactivate(thisHand, handHealth / handHealthMax);
        flashing = false;
        interp.SetLerp(manos.GetInterpMax());
        interp.SetOffsetActive(false);
        SetParticlePower(0);
        routineOnce = false;
        rapidFire = false;
        gunBarrel.SetActive(false);
        energyFist.SetActive(false);

        am.StopSoundLoop(AudioManager.Sound.ManosAura, true, transform);
        am.StopSoundLoop(AudioManager.Sound.ManosCharging, true, transform);
        am.StopSoundLoop(AudioManager.Sound.ManosGunActive, true, transform);
        //am.StopSoundLoop(AudioManager.Sound.IntenseFire, true);
    }

    private void GrabPlayer()
    {
        objectInHand = collidingObject;

        if (!objectInHand.GetComponent<PlayerHealth>().CanTakeDamage())
        {
            return;
        }

        //Clear power settings
        manos.GetFlasher().ManosChargeDeactivate(thisHand, handHealth / handHealthMax);
        flashing = false;

        manos.ResetChargeTimer(thisHand);
        interp.SetLerp(manos.GetInterpMax());
        interp.SetOffsetActive(false);
        SetParticlePower(0);

        //// Move the GameObject inside the player’s hand and remove it from the collidingObject variable.
        //objectInHand = collidingObject;
        //collidingObject = null;
        //// Add a new joint that connects the controller to the object using the AddFixedJoint() method below.
        //var joint = AddFixedJoint();
        //joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
        //objectInHand.GetComponent<PlayerHealth>().TakeDamage(15);

        //if (!_training)
        //{
            objectInHand.GetComponent<PlayerManager>().SetGrabbed(true, this);

            chadMesh.SetActive(true);
        //}

        objectInHand.transform.parent = transform;
        objectInHand.transform.localPosition = grabOffset;
        objectInHand.transform.localRotation = Quaternion.identity;

        Instantiate(manosGrabPrefab, transform.GetChild(0).position, Quaternion.identity);

        am.StopSound(AudioManager.Sound.ManosCharging, transform);

        am.PlaySoundOnce(AudioManager.Sound.ManosGrabSuccess, transform);

        manos.SetPlayerGrabbed(true);

        StartCoroutine("GrabPlayerCountdown");
    }

    // Make a new fixed joint, add it to the controller, and then set it up so it doesn’t break easily.
    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    /// <summary>
    /// Releases the player
    /// </summary>
    /// <param name="throwPlayer">Drops the player if false</param>
    public void ReleasePlayer(bool throwPlayer = true)
    {
        StopCoroutine("GrabPlayerCountdown");

        manos.CooldownGrip();

        objectInHand.GetComponent<MovementManager>().ResetVelocity();
        objectInHand.GetComponent<PlayerManager>().SetGrabbed(false, this);
        objectInHand.transform.parent = null;

        if (throwPlayer)
        {
            objectInHand.GetComponent<MovementManager>().Knockback(velocity);
        }

        objectInHand = null;
        //grabCooldown = manos.GetGripCooldown();
        chadMesh.SetActive(false);

        manos.SetPlayerGrabbed(false);

        collidingObject = null;
    }

    IEnumerator GrabPlayerCountdown()
    {
        yield return new WaitForSeconds(manos.GetGripDuration());
        ReleasePlayer(false);
    }

    /// <summary>
    /// Deals damage to the arm
    /// </summary>
    /// <param name="damage">A positive value</param>
    /// <returns>True if damage was dealt</returns>
    public bool TakeDamage(float damage)
    {
        if (!armDisabled && !manos.IsFistPowered(thisHand))
        {
            damageNumbers.ShowDamage(damage);
            Debug.Log("Arm took Damage");

            ModifyArmHealth(-Mathf.Abs(damage));
            // Rumble for 2 seconds
            try
            {
                haptics.Execute(0, 1f, 80, 0.5f, skeltal.inputSource);
            } catch { }
        }
        bool b = !armDisabled;
        return b;
    }

    public void ModifyArmHealth(float h)
    {
        handHealth += h;
        if (handHealth <= 0)
        {
            StartCoroutine(DestroyArm());
            StartCoroutine(ExplodeArms());
        }
        handHealth = Mathf.Clamp(handHealth, 0, handHealthMax);
        float healthPercent = handHealth / handHealthMax;
        hpText.text = Mathf.RoundToInt(healthPercent * 100).ToString() + "%";
        radialBar.fillAmount = healthPercent;
        hpBar.fillAmount = healthPercent;

        mat.SetFloat("_Glow", Mathf.Lerp(-0.9f, 0, healthPercent));
        vMat.SetFloat("_Glow", Mathf.Lerp(-0.9f, 0, healthPercent));
    }

    public void DisableArm()
    {
        StopCoroutine(ExplodeArms());
        armDisabled = true;
        rapidFire = false;
        CancelCharge();
        pHealth.TakeDamage(handHealthMax * manosDamageModifier);
        am.PlaySoundOnce(AudioManager.Sound.ManosCrash);
        StartCoroutine("RepairArm");
        mat.SetFloat("_Glow", -0.9f);
        vMat.SetFloat("_Glow", -0.9f);
        bullet.gameObject.SetActive(false);
        skeltal.enabled = false;
        skeltalBig.enabled = false;

        try
        {
            haptics.Execute(0, manos.GetArmRepairTime(), 100, 0.5f, skeltal.inputSource);
        }
        catch { }

        EnableGravity();
    }

    public void KillArm()
    {
        armDisabled = true;
        rapidFire = false;
        CancelCharge();
       // pHealth.TakeDamage(handHealthMax * manosDamageModifier);
        am.PlaySoundOnce(AudioManager.Sound.ManosCrash);
        //StartCoroutine("RepairArm");
        mat.SetFloat("_Glow", -0.9f);
        vMat.SetFloat("_Glow", -0.9f);
        bullet.gameObject.SetActive(false);
        skeltal.enabled = false;

        EnableGravity();
    }

    public bool IsPowered()
    {
        return manos.IsFistPowered(thisHand);
    }

    void EnableGravity()
    {
        interp.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;

        gauntlet.enabled = false;
        gauntlet.gameObject.AddComponent<Rigidbody>();
    }

    void DisableGravity()
    {
        interp.enabled = true;
        rb.isKinematic = true;
        rb.useGravity = false;

        gauntlet.enabled = true;
        Destroy(gauntlet.GetComponent<Rigidbody>());
    }

    IEnumerator DestroyArm()
    {
        armExplodePrefabs[Random.Range(0, armExplodePrefabs.Length)].Play();
        try {
            haptics.Execute(0, 0.5f, 40, 0.75f, skeltal.inputSource);
        } catch { }
        
        yield return new WaitForSeconds(1);

        armExplodePrefabs[Random.Range(0, armExplodePrefabs.Length)].Play();
        try { 
            haptics.Execute(0, 0.5f, 40, 0.75f, skeltal.inputSource);
        } catch { }

        yield return new WaitForSeconds(0.75f);

        armExplodePrefabs[Random.Range(0, armExplodePrefabs.Length)].Play();
        try
        {
            haptics.Execute(0, 0.5f, 40, 0.75f, skeltal.inputSource);
        } catch { }

        yield return new WaitForSeconds(1.0f);

        armExplodePrefabs[Random.Range(0, armExplodePrefabs.Length)].Play();
        try
        {
            haptics.Execute(0, 0.5f, 40, 0.75f, skeltal.inputSource);
        } catch { }

        yield return new WaitForSeconds(1.5f);
        armExplodePrefabs[Random.Range(0, armExplodePrefabs.Length)].Play();

        DisableArm();
    }

    IEnumerator ExplodeArms()
    {
        while (!armDisabled)
        {
            armExplodePrefabs[Random.Range(0, armExplodePrefabs.Length)].Play();

            am.PlaySoundOnce(AudioManager.Sound.ManosExplodeArm + Random.Range(0, 4), transform);

            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator RepairArm()
    {
        yield return new WaitForSeconds(1.5f); //Time it takes the white flashes to stop

        mat.SetFloat("_Glow", -0.9f);
        vMat.SetFloat("_Glow", -0.9f);

        yield return new WaitForSeconds(manos.GetArmRepairTime() - 1.5f);
        armDisabled = false;
        ModifyArmHealth(handHealthMax);
        mat.SetFloat("_Glow", 0);
        vMat.SetFloat("_Glow", 0);

        DisableGravity();

        //Magic numbers lol
        gauntlet.SetInterpValue((manos.GetInterpMin() + manos.GetInterpMax()) / 2);
        skeltal.enabled = true;

        yield return new WaitForSeconds(2.5f);

        gauntlet.ResetInterpValue();
    }

    /// <summary>
    /// Called by CollisionDamage on hitting Chad
    /// </summary>
    public void HitImpulse()
    {
        try
        {
            haptics.Execute(0, 1f, 120, 0.9f, skeltal.inputSource);
        } catch { }
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Ground"))
    //    {
    //        if (vInterp.enabled == false && !armDisabled)
    //        {
    //            //interp.enabled = false;
    //            //vInterp.enabled = true;
    //            //vInterp.SetDirection(velocity);
    //            //skeltal.enabled = false;
    //            //haptics.Execute(0, 0.5f, 120, 0.9f, skeltal.inputSource);
    //            //haptics.Execute(0, Time.deltaTime, 120, 0.9f, skeltal.inputSource);
    //            //trueHand.SetActive(true);
    //            //if (objectInHand) ReleasePlayer();
    //        }
    //    }
    //    else
    //    {
    //        if (other.CompareTag("Player"))
    //        {
    //            SetCollidingObject(other);
    //        }
    //    }
    //}

    //void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Ground"))
    //    {
    //        if (vInterp.enabled == false && !armDisabled)
    //        {
    //            //haptics.Execute(0, Time.deltaTime, 120, 0.9f, skeltal.inputSource);
    //        }
    //    }
    //    else if (other.CompareTag("Player"))
    //    {
    //        SetCollidingObject(other);
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (!collidingObject)
    //    {
    //        return;
    //    }
    //    collidingObject.transform.parent = null;
    //
    //    collidingObject = null;
    //}

    public GameObject GetCollidingObject()
    {
        return collidingObject;
    }

    public Vector3 GetForward()
    {
        return wristTransform.forward;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public float GetSpeed()
    {
        return realSpeed;
    }

    public Enums.Hand GetHand()
    {
        return thisHand;
    }

    public void SetTraining(bool t)
    {
        _training = t;
    }
}
