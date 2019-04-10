using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField]
    FlashFeedback flash;

    [SerializeField]
    GameObject manosLeftHand;

    [SerializeField]
    GameObject manosRightHand;

    [SerializeField]
    float explosiontime = 3.0f;

    [SerializeField]
    float shaketime = 3.0f;

    [SerializeField]
    float handtime = 0.3f;

    [SerializeField]
    float killbodytime = 1.0f;


    [SerializeField]
    float killheadtime = 1.0f;

    [SerializeField]
    float shake = 2.0f;

    MeshShaker _shakeLeft;

    MeshShaker _shakeRight;

    ManosHand _handLeft;

    ManosHand _handRight;

    [SerializeField]
    GameObject manosHead;

    [SerializeField]
    GameObject fakeHead;

    [SerializeField]
    GameObject manosBody;

    [SerializeField]
    GameObject fakeBody;



    [SerializeField]
    GameObject fakeRight;



    [SerializeField]
    GameObject fakeLeft;

    [SerializeField]
    GameObject fakeRightVambrace;



    [SerializeField]
    GameObject fakeLeftVambrace;

    [SerializeField]
    GameObject fakeRightArmor;



    [SerializeField]
    GameObject fakeLeftArmor;

    [SerializeField]
    GameObject explosionPrefab;

    [SerializeField]
    GameObject gaunletL;



    [SerializeField]
    GameObject gaunletR;


    [SerializeField]
    float scalar = 20.0f;

    [SerializeField]
    float boomtime=0.5f;




    // Start is called before the first frame update
    void Start()
    {
        _shakeLeft = manosLeftHand.GetComponent<MeshShaker>();
        _shakeRight = manosRightHand.GetComponent<MeshShaker>();

        _handLeft = manosLeftHand.GetComponent<ManosHand>();
        _handRight = manosRightHand.GetComponent<ManosHand>();


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine("Explode");
        }
    }
    IEnumerator Explode()
    {
        flash.ManosPlayDeath();
        //  Debug.Log("Memes");

        yield return new WaitForSeconds(explosiontime);
        StartCoroutine("ShakeHands");
    }

    IEnumerator ShakeHands()
    {

        _shakeLeft.shakeIntensity = shake;
        _shakeRight.shakeIntensity = shake;
        _shakeLeft.shakeTime = shaketime;
        _shakeRight.shakeTime = shaketime;
        _shakeLeft.enabled = true;
        _shakeRight.enabled = true;
        yield return new WaitForSeconds(shaketime);
        flash.FlashReset();
        _shakeLeft.enabled = false;
        _shakeRight.enabled = false;
        //    StartCoroutine("KillHands");
        Instantiate(explosionPrefab, manosBody.transform.position, transform.rotation);
        yield return new WaitForSeconds(boomtime);
        Detonate();
    }
    void Detonate()
    {
        //   Debug.Log("Memes");
        fakeBody.SetActive(true);
        fakeBody.transform.SetPositionAndRotation(manosBody.transform.position, manosBody.transform.rotation);
        manosBody.SetActive(false);
        fakeHead.SetActive(true);
        fakeHead.transform.SetPositionAndRotation(manosHead.transform.position, manosHead.transform.rotation);
        manosHead.SetActive(false);
        manosLeftHand.SetActive(false);
        manosRightHand.SetActive(false);
        gaunletL.SetActive(false);
        gaunletR.SetActive(false);

        fakeRight.SetActive(true);
        fakeRight.transform.SetPositionAndRotation(manosRightHand.transform.position, manosRightHand.transform.rotation);

        fakeRightVambrace.SetActive(true);
        fakeRightVambrace.transform.SetPositionAndRotation(manosRightHand.transform.position, manosRightHand.transform.rotation);

        fakeRightArmor.SetActive(true);
        fakeRightArmor.transform.SetPositionAndRotation(manosRightHand.transform.position, manosRightHand.transform.rotation);

        fakeLeft.SetActive(true);
        fakeLeft.transform.SetPositionAndRotation(manosLeftHand.transform.position, manosLeftHand.transform.rotation);

        fakeLeftVambrace.SetActive(true);
        fakeLeftVambrace.transform.SetPositionAndRotation(manosLeftHand.transform.position, manosLeftHand.transform.rotation);

        fakeLeftArmor.SetActive(true);
        fakeLeftArmor.transform.SetPositionAndRotation(manosLeftHand.transform.position, manosLeftHand.transform.rotation);

        fakeBody.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere * scalar;
        fakeHead.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere * scalar;
        fakeRight.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere * scalar;
        fakeRightVambrace.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere * scalar;
        fakeRightArmor.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere * scalar;
        fakeLeft.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere * scalar;
        fakeLeftVambrace.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere * scalar;
        fakeLeftArmor.GetComponent<Rigidbody>().velocity = Random.insideUnitSphere * scalar;




    }
    IEnumerator KillHands()
    {

        _handLeft.KillArm();
        yield return new WaitForSeconds(handtime);
        _handRight.KillArm();
        yield return new WaitForSeconds(killbodytime);
        StartCoroutine("KillChest");
    }

    IEnumerator KillChest()
    {

        fakeBody.SetActive(true);
        fakeBody.transform.SetPositionAndRotation(manosBody.transform.position, manosBody.transform.rotation);
        manosBody.SetActive(false);
        yield return new WaitForSeconds(killheadtime);
        KillHead();
    }
    void KillHead()
    {
        fakeHead.SetActive(true);
        fakeHead.transform.SetPositionAndRotation(manosHead.transform.position, manosHead.transform.rotation);
        manosHead.SetActive(false);
    }
}