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
        GameObject bigG = collision.gameObject;

        print(bigG.name);

        AudioManager.GetInstance().PlaySoundOnce(AudioManager.Sound.ChadBomb, transform, AudioManager.Priority.Low, AudioManager.Pitches.Low);
        Instantiate(explosionfx, transform.position, transform.rotation);
        Destroy(gameObject);

        if (bigG.tag == "Hittable")
        {
            //Shake the mesh that you hit
            MeshShaker shaker = collision.gameObject.GetComponent<MeshShaker>();
            if (shaker == null)
            {
                shaker = collision.gameObject.GetComponentInChildren<MeshShaker>();
            }
            if (shaker == null)
            {
                shaker = collision.transform.parent.GetComponent<MeshShaker>();
            }

            if (bigG.name == "GauntletParent_L")
            {
                //Returns true if damage was dealt
                if (collision.transform.root.GetComponent<Manos>().DealDamageToArm(Enums.Hand.Left, damageScale))
                {
                    shaker.enabled = true;
                }
            }
            else if (bigG.name == "GauntletParent_R")
            {
                //Returns true if damage was dealt
                if (collision.transform.root.GetComponent<Manos>().DealDamageToArm(Enums.Hand.Right, damageScale))
                {
                    shaker.enabled = true;
                }
            }
            else if (bigG.name == "Target")
            {
                PlayerHealth targetHP = collision.transform.GetComponent<PlayerHealth>();
                if (targetHP)
                {
                    targetHP.TakeDamage(damageScale, Enums.ManosParts.None);
                }
            }
            else
            {
                ManosHand hand = bigG.GetComponent<ManosHand>();

                Enums.ManosParts part = Enums.ManosParts.None;

                if (bigG.name == "vr_glove_left")
                {
                    part = Enums.ManosParts.LeftHand;
                }
                else if (bigG.name == "vr_glove_right")
                {
                    part = Enums.ManosParts.RightHand;
                }
                else if (bigG.name == "Chest")
                {
                    part = Enums.ManosParts.Chest;
                }
                else if (bigG.name == "Head")
                {
                    part = Enums.ManosParts.Head;
                }
                else if (bigG.name == "GrabBox")
                {
                    switch (bigG.GetComponent<ManosGrab>().GetHand().GetHand())
                    {
                        case Enums.Hand.Left:
                            part = Enums.ManosParts.LeftHand;
                            break;
                        case Enums.Hand.Right:
                            part = Enums.ManosParts.RightHand;
                            break;
                    }
                }

                if (hand)
                {
                    if (hand.TakeDamage(damageScale))
                    {
                        shaker.enabled = true;
                        collision.transform.root.GetComponent<FlashFeedback>().ReactToDamage(0.0f, part);
                    }
                }
                else
                {
                    PlayerHealth hp = collision.transform.root.GetComponent<PlayerHealth>();
                    if (hp)
                    {
                        hp.TakeDamage(damageScale, part);
                    }
                        
                    shaker.enabled = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);

        if (other.tag == "Hittable")
        {
            //Shake the mesh that you hit
            MeshShaker shaker = other.GetComponent<MeshShaker>();
            if (shaker == null)
            {
                shaker = other.GetComponentInChildren<MeshShaker>();
            }
            if (shaker == null)
            {
                shaker = other.GetComponentInParent<MeshShaker>();
            }

            if (other.name == "GauntletParent_L")
            {
                //Returns true if damage was dealt
                if (other.transform.root.GetComponent<Manos>().DealDamageToArm(Enums.Hand.Left, damageScale))
                {
                    shaker.enabled = true;
                }
            }
            else if (other.name == "GauntletParent_R")
            {
                //Returns true if damage was dealt
                if (other.transform.root.GetComponent<Manos>().DealDamageToArm(Enums.Hand.Right, damageScale))
                {
                    shaker.enabled = true;
                }
            }
            else
            {
                ManosHand hand = other.GetComponent<ManosHand>();
                if (hand == null)
                {
                    hand = other.GetComponentInParent<ManosHand>();
                }
                    

                Enums.ManosParts part = Enums.ManosParts.None;

                if (other.attachedRigidbody.name == "vr_glove_left")
                {
                    part = Enums.ManosParts.LeftHand;
                }
                else if (other.attachedRigidbody.name == "vr_glove_right")
                {
                    part = Enums.ManosParts.RightHand;
                }
                else if (other.name == "Chest")
                {
                    part = Enums.ManosParts.Chest;
                }
                else if (other.name == "Head")
                {
                    part = Enums.ManosParts.Head;
                }
                else if (other.name == "GrabBox")
                {
                    switch (other.GetComponent<ManosGrab>().GetHand().GetHand())
                    {
                        case Enums.Hand.Left:
                            part = Enums.ManosParts.LeftHand;
                            break;
                        case Enums.Hand.Right:
                            part = Enums.ManosParts.RightHand;
                            break;
                    }
                }

                if (hand)
                {
                    if (hand.TakeDamage(damageScale))
                    {
                        print("POW!");
                        shaker.enabled = true;
                        other.transform.root.GetComponent<FlashFeedback>().ReactToDamage(0.0f, part);
                    }
                }
                else
                {
                    PlayerHealth hp = other.transform.root.GetComponent<PlayerHealth>();
                    if (hp)
                    {
                        hp.TakeDamage(damageScale, part);
                    }
                        
                    shaker.enabled = true;
                }
            }
        }

        AudioManager.GetInstance().PlaySoundOnce(AudioManager.Sound.ChadBomb, transform, AudioManager.Priority.Low, AudioManager.Pitches.Low);
        Instantiate(explosionfx, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}