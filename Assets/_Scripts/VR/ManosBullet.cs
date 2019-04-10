using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManosBullet : MonoBehaviour
{

    /// <summary>
    /// Object reference to Manos
    /// </summary>
    [SerializeField]
    Manos manos;

    /// <summary>
    /// Force of knockback
    /// </summary>
    [SerializeField]
    Vector3 knockBack;

    /// <summary>
    /// Should the laser pointer be visible?
    /// </summary>
    [SerializeField]
    bool enableLaser;

    /// <summary>
    /// what layers the bullet should show decals on when it collides
    /// </summary>
    [SerializeField]
    LayerMask bulletLayerMask;

    Transform spawn;

    /// <summary>
    /// Because the bullet moves so fast it appears to spawn far ahead
    /// </summary>
    [SerializeField]
    [Tooltip("Because the bullet moves so fast it appears to spawn far ahead")]
    float spawnOffset;

    /// <summary>
    /// Original position of the bullet
    /// </summary>
    Vector3 ogPos;

    float speed;

    float lifeTime;
    float lifeTimer;

    Rigidbody rb;

    Transform bigP;

    bool fired;

    LineRenderer laser;

    MeshRenderer mesh;

    /// <summary>
    /// Object reference to the bullet's line
    /// </summary>
    [SerializeField]
    VolumetricLines.VolumetricLineBehavior line;

    [SerializeField]
    float trailSpeedMultiplier;

    Collider col;

    [SerializeField]
    GameObject collisionParticles;

    [SerializeField]
    GameObject collisionParticlesReverse;

    public void Init(Transform _spawn, Manos m)
    {
        manos = m;
        mesh = GetComponent<MeshRenderer>();
        col = transform.GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        //ogPos = transform.localPosition;
        //bigP = transform.parent;
        laser = GetComponent<LineRenderer>();
        if (line == null)
            line = GetComponentInChildren<VolumetricLines.VolumetricLineBehavior>();
        spawn = _spawn;
    }

    // Fires the bullet
    private void OnEnable()
    {
        laser.enabled = true;
        fired = false;
        line.enabled = false;
    }

    /// <summary>
    /// Cause haha
    /// </summary>
    public void EnableLaser()
    {
        laser.enabled = true;
        fired = false;
        line.enabled = false;
    }

    public void FireBullet()
    {
        gameObject.SetActive(true);
        mesh.enabled = true;
        lifeTimer = lifeTime;
        fired = true;
        transform.position = spawn.position;
        transform.rotation = spawn.rotation;
        transform.position -= transform.up * spawnOffset;
        col.enabled = true;
        line.enabled = true;
        laser.enabled = false;
        line.StartPos = Vector3.zero;
        
        //Show hit FX where the laser hits an object
        {
            RaycastHit[] hits;

            // front side of hits
            hits = Physics.RaycastAll(transform.position, transform.up, GetComponent<CapsuleCollider>().height, bulletLayerMask);

            foreach (RaycastHit hit in hits)
            {

                //Debug.Log("ray hit " + hit.collider.name);
                Vector3 impactPoint = hit.point;
                Destroy(Instantiate(collisionParticles, impactPoint, transform.rotation), 2.0f);
            }

            // reverse side of hits
            hits = Physics.RaycastAll(transform.position + transform.up * GetComponent<CapsuleCollider>().height, -transform.up, bulletLayerMask);

            foreach (RaycastHit hit in hits)
            {
                if(hit.collider.gameObject.layer != LayerMask.NameToLayer("Manos"))
                {
                    Vector3 impactPoint = hit.point;
                    Destroy(Instantiate(collisionParticlesReverse, impactPoint, transform.rotation), 5.0f);
                }
                //Debug.Log("ray hit backward " + hit.collider.name);
            }
        }
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }

    public void SetLifeTime(float l)
    {
        lifeTime = l;
    }

    // Update is called once per frame
    void Update()
    {
        if (fired)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
        else
        {
            if (enableLaser)
            {
                laser.SetPosition(0, spawn.position);
                laser.SetPosition(1, spawn.position + 300 * spawn.up);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Hittable"))
        //{
        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            // Apply Knockback to player
            MovementManager player = other.GetComponent<MovementManager>();
            if (player)
            {
                if (ph.CanTakeDamage())
                {
                    //print(other);
                    other.GetComponent<PlayerHealth>().TakeDamage(manos.GetBulletDamage());

                    if (other.GetComponent<PlayerManager>().GetEnumPlayerState() != Enums.PlayerState.Grabbed)
                    {
                        player.Knockback(new Vector3(
                        -transform.forward.x * knockBack.x,
                        -transform.forward.y * knockBack.y,
                        -transform.forward.z * knockBack.z
                        ));
                    }
                }
            }
        }
        //}
    }
}
