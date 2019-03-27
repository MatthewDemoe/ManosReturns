using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FootBomb : MonoBehaviour
{
    [SerializeField]
    float damageScale = 2.0f;

    [SerializeField]
    GameObject explosionfx;

   // [SerializeField]
    //GameObject sonicBoom;

    public Vector3 angle { get { return transform.rotation.eulerAngles; } }

    [SerializeField]
    float rotationSpeed = 5.0f;   //[SerializeField]

    float _rotation = 0.0f;
    Rigidbody rigidBody;

    void OnEnable()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();
    }

    void UpdateRotation()
    {
     
        transform.forward=GetComponent<Rigidbody>().velocity.normalized;
        _rotation += rotationSpeed * Time.deltaTime;
        transform.RotateAround(transform.position, GetComponent<Rigidbody>().velocity.normalized,_rotation);
    }

    public void Boost(Vector3 velocity)
    {
        rigidBody.velocity = velocity;
        //Instantiate(sonicBoom, transform.position, transform.rotation);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hittable")
        {
            ManosHand h = collision.gameObject.GetComponent<ManosHand>();

            //Shake the mesh that you hit
            MeshShaker m = collision.gameObject.GetComponent<MeshShaker>();
            if (m == null) m = collision.gameObject.GetComponentInChildren<MeshShaker>();
            m.enabled = true;

            if (h)
            {
                h.TakeDamage(damageScale);
            }
            else
            {
                PlayerHealth hp = collision.gameObject.transform.root.GetComponent<PlayerHealth>();
                if (hp)
                    hp.TakeDamage(damageScale);
            }
        }

        AudioManager.GetInstance().PlaySoundOnce(AudioManager.Sound.ChadBomb, transform, AudioManager.Priority.Low, AudioManager.Pitches.Low);
        Instantiate(explosionfx, transform.position, transform.rotation);
        Destroy(gameObject);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hittable")
        {
            ManosHand h = other.GetComponent<ManosHand>();

            //Shake the mesh that you hit
            MeshShaker m = other.GetComponent<MeshShaker>();
            if (m == null) m = other.GetComponentInChildren<MeshShaker>();
            m.enabled = true;

            if (h)
            {
                h.TakeDamage(damageScale);
            }
            else
            {
                PlayerHealth hp = other.transform.root.GetComponent<PlayerHealth>();
                if (hp)
                    hp.TakeDamage(damageScale);
            }
        }

        AudioManager.GetInstance().PlaySoundOnce(AudioManager.Sound.ChadBomb, transform, AudioManager.Priority.Low, AudioManager.Pitches.Low);
        Instantiate(explosionfx, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}