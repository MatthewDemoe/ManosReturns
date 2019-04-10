using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerHealth : MonoBehaviour
{
    public static float globalDamageMultiplier = 1.0f;

    [SerializeField]
    private float health;

    [SerializeField]
    float baseHealth;

    [SerializeField]
    HPBarUI[] playerUI;

    PlayerStateManager psm;

    Animator _anim;

    [SerializeField]
    bool invincible;

    [SerializeField]
    DamageVignette vignetteControl;

    FlashFeedback flash;

    string _attachedPlayer;

    bool dead = false;

    // Use this for initialization
    void Start() {
        health = baseHealth;
        psm = GameObject.Find("OverlordController").GetComponent<PlayerStateManager>();
        _anim = GetComponent<Animator>();
        flash = GetComponent<FlashFeedback>();

        _attachedPlayer = gameObject.name;
    }

    // Update is called once per frame
    void Update() {
        if (CoolDebug.hacks)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                TakeDamage(50);
                CoolDebug.GetInstance().LogHack("Global damage!");
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                TakeDamage(-1000);
                CoolDebug.GetInstance().LogHack("Global heal!");
            }
        }
    }

    public void SetInvincible(bool b)
    {
        invincible = b;
    }
    
    public bool CanTakeDamage()
    {
        return !invincible;
    }

    public void SetHealth(float h)
    {
        health = h;
    }

    public void TakeDamage(float damage, Enums.ManosParts m = Enums.ManosParts.None) {

        if (invincible) return;
        float damageDealt = damage * globalDamageMultiplier;

        if ((health - damageDealt) < 0)
        {
            health = 0;
        }
        else
        {
            health -= damageDealt;
        }

        if ((health == 0) && !dead)
        {
            dead = true;
            psm.RegisterPlayerDeath(gameObject);
        }

        float damagePercentage = damageDealt / baseHealth;

        foreach (HPBarUI p in playerUI)
        {
            p.DealDamagePercentage(damagePercentage);
        }

        if (_anim != null)
            _anim.SetTrigger("DamageTaken");

        flash.ReactToDamage(0.0f, m);

        //PlayerHealth on everything is making things fucky, eh?
        if (vignetteControl != null)
        {
            vignetteControl.SetVignetteFill(GetHealthPercentage());
        }
        
    }

    public void TakeDamageComplex(float baseDamage, float baseDamageMultiplier, float variableDamage, float variableDamageMultiplier)
    {
        float damageDealt = ((baseDamage * baseDamageMultiplier) + (variableDamage * variableDamageMultiplier)) * globalDamageMultiplier;
    }

    public float GetHealthPercentage() {
        return health / baseHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetBaseHealth()
    {
        return baseHealth;
    }

}