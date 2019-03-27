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

    // Use this for initialization
    void Start() {
        health = baseHealth;
        psm = GameObject.Find("OverlordController").GetComponent<PlayerStateManager>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.D))
        {
            TakeDamage(50);
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

    public void TakeDamage(float damage) {

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

        if (health == 0)
            psm.RegisterPlayerDeath(gameObject);


        //if (damageDealt > 1.0f)
        {
            float damagePercentage = damageDealt / baseHealth;

            foreach (HPBarUI p in playerUI)
            {
                p.DealDamagePercentage(damagePercentage);
            }
            //playerUI.SetHitPointPercentage(health / baseHealth);

            if (_anim != null)
                _anim.SetTrigger("DamageTaken");

            GetComponent<FlashFeedback>().ReactToDamage(0.0f);
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