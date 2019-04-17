using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    [SerializeField]
    ManosHand hand;

    [SerializeField]
    float multiplier;

    Vector3 velocity;

    [SerializeField]
    Vector3 knockBackMultiplier;

    /// <summary>
    /// Minimum amount of speed before registering as a hit
    /// </summary>
    [SerializeField]
    [Tooltip("Minimum amount of speed before registering as a hit")]
    float minimumSpeed;

    [SerializeField]
    GameObject lightPrefab;
    [SerializeField]
    GameObject medPrefab;
    [SerializeField]
    GameObject heavyPrefab;

    [SerializeField]
    float lightHitThresh;
    [SerializeField]
    float medHitThresh;

    [SerializeField]
    float lightDamage;
    [SerializeField]
    float medDamage;
    [SerializeField]
    float heavyDamage;

    [SerializeField]
    GameObject chadBlaze;
    
    // Use this for initialization
    void Start()
    {
        if (hand == null)
            hand = GetComponent<ManosHand>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    print(collision.gameObject.name);
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        PlayerHealth ph = collision.gameObject.GetComponent<PlayerHealth>();
    //        MovementManager player = collision.transform.GetComponent<MovementManager>();
    //        if (ph != null)
    //        {
    //            if (ph.CanTakeDamage())
    //            {
    //                //Calculate damage
    //                velocity = hand.GetVelocity();
    //                //float damage = velocity * multiplier;
    //
    //                Vector3 contactPt = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
    //
    //                //Damage only happens here
    //
    //                if (velocity.magnitude > minimumSpeed && velocity.magnitude <= lightHitThresh)
    //                {
    //                    GameObject p = Instantiate(lightPrefab, contactPt, Quaternion.identity);
    //                    ph.TakeDamage(lightDamage);
    //                    Destroy(p, 2f);
    //                    AudioManager.GetInstance().PlaySoundOnce(AudioManager.Sound.ManosPunchLight, transform, AudioManager.Priority.Default, AudioManager.Pitches.Low);
    //                }
    //                else if (velocity.magnitude > lightHitThresh && velocity.magnitude <= medHitThresh)
    //                {
    //                    GameObject p = Instantiate(medPrefab, contactPt, Quaternion.identity);
    //                    ph.TakeDamage(medDamage);
    //                    Destroy(p, 2f);
    //                    AudioManager.GetInstance().PlaySoundOnce(AudioManager.Sound.ManosPunchMedium, transform, AudioManager.Priority.Default, AudioManager.Pitches.Low);
    //                }
    //                else if (velocity.magnitude > medHitThresh)
    //                {
    //                    GameObject p = Instantiate(heavyPrefab, contactPt, Quaternion.identity);
    //                    ph.TakeDamage(heavyDamage);
    //                    Destroy(p, 2f);
    //                    AudioManager.GetInstance().PlaySoundOnce(AudioManager.Sound.ManosPunchHeavy, transform, AudioManager.Priority.Default, AudioManager.Pitches.Low);
    //                }
    //
    //                if (velocity.magnitude > minimumSpeed)
    //                {
    //                    player.Knockback(new Vector3(
    //                    velocity.x * knockBackMultiplier.x,
    //                    velocity.y * knockBackMultiplier.y,
    //                    velocity.z * knockBackMultiplier.z
    //                    ));
    //
    //                    hand.HitImpulse();
    //                }
    //            }
    //        }
    //    }
    //}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerHealth ph = col.gameObject.GetComponent<PlayerHealth>();
            MovementManager player = col.GetComponent<MovementManager>();
            if (ph != null)
            {
                if (ph.CanTakeDamage())
                {
                    float damage = 0;
                    if (hand.IsPowered())
                    {
                        damage += 20;
                    }

                    //Calculate damage
                    velocity = hand.GetVelocity();
                    //float damage = velocity * multiplier;
    
                    Vector3 contactPt = col.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
    
                    //Damage only happens here

                    if (hand.IsPowered())
                    {
                        Destroy(Instantiate(chadBlaze, col.transform), 3f);
                    }
    
                    if (velocity.magnitude > minimumSpeed && velocity.magnitude <= lightHitThresh)
                    {
                        GameObject p = Instantiate(lightPrefab, contactPt, Quaternion.identity);
                        damage += lightDamage;
                        ph.TakeDamage(damage);
                        Destroy(p, 2f);
                        AudioManager.GetInstance().PlaySoundOnce(AudioManager.Sound.ManosPunchLight, transform, AudioManager.Priority.Default, AudioManager.Pitches.Low);
                    }
                    else if (velocity.magnitude > lightHitThresh && velocity.magnitude <= medHitThresh)
                    {
                        GameObject p = Instantiate(medPrefab, contactPt, Quaternion.identity);
                        damage += medDamage;
                        ph.TakeDamage(damage);
                        Destroy(p, 2f);
                        AudioManager.GetInstance().PlaySoundOnce(AudioManager.Sound.ManosPunchMedium, transform, AudioManager.Priority.Default, AudioManager.Pitches.Low);
                    }
                    else if (velocity.magnitude > medHitThresh)
                    {
                        GameObject p = Instantiate(heavyPrefab, contactPt, Quaternion.identity);
                        damage += heavyDamage;
                        ph.TakeDamage(damage);
                        Destroy(p, 2f);
                        AudioManager.GetInstance().PlaySoundOnce(AudioManager.Sound.ManosPunchHeavy, transform, AudioManager.Priority.Default, AudioManager.Pitches.Low);
                    }

                    Vector3 knockBack = knockBackMultiplier;
                    if (hand.IsPowered())
                    {
                        knockBack = new Vector3(knockBack.x * 1.25f, knockBack.y * 1.25f, knockBack.z * 1.25f);
                    }
                    if (velocity.magnitude > minimumSpeed)
                    {
                        player.Knockback(new Vector3(
                        velocity.x * knockBack.x,
                        velocity.y * knockBack.y,
                        velocity.z * knockBack.z
                        ));
    
                        hand.HitImpulse();
                    }
                }
            }
        }
    }
}
