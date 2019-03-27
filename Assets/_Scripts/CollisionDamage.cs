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
                    //Calculate damage
                    velocity = hand.GetVelocity();
                    //float damage = velocity * multiplier;

                    Vector3 contactPt = col.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                    //Damage only happens here

                    if (velocity.magnitude > minimumSpeed && velocity.magnitude <= lightHitThresh)
                    {
                        GameObject p = Instantiate(lightPrefab, contactPt, Quaternion.identity);
                        ph.TakeDamage(lightDamage);
                        Destroy(p, 2f);
                    }
                    else if (velocity.magnitude > lightHitThresh && velocity.magnitude <= medHitThresh)
                    {
                        GameObject p = Instantiate(medPrefab, contactPt, Quaternion.identity);
                        ph.TakeDamage(medDamage);
                        Destroy(p, 2f);
                    }
                    else if (velocity.magnitude > medHitThresh)
                    {
                        GameObject p = Instantiate(heavyPrefab, contactPt, Quaternion.identity);
                        ph.TakeDamage(heavyDamage);
                        Destroy(p, 2f);
                    }

                    if (velocity.magnitude > minimumSpeed)
                    {
                        AudioManager.GetInstance().PlaySoundOnce(AudioManager.Sound.ManosPunch, transform, AudioManager.Priority.Default, AudioManager.Pitches.Low);

                        player.Knockback(new Vector3(
                        velocity.x * knockBackMultiplier.x,
                        velocity.y * knockBackMultiplier.y,
                        velocity.z * knockBackMultiplier.z
                        ));
                    }
                }
            }
        }
    }
}
