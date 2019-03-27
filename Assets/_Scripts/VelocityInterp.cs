using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityInterp : MonoBehaviour {

    [SerializeField]
    Transform original;

    [SerializeField]
    float speed;

    float ogValue;

    float shakeFactor;

    Rigidbody rb;

    //[SerializeField]
    //float minDist;

    [SerializeField]
    float retractTime = 3f;

    Vector3 direction;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}

    private void OnEnable()
    {
        if (rb != null) rb.isKinematic = false;
        StartCoroutine("RetractCountdown");
    }

    private void OnDisable()
    {
        rb.isKinematic = true;
        StopCoroutine("RetractCountdown");
    }

    // Update is called once per frame
    void FixedUpdate () {
        //float targetDistance = Vector3.Distance(original.position, transform.position);

        rb.velocity = -direction * speed * Time.deltaTime;
        //if (targetDistance > minDist)
        //{
        //float spd = Mathf.InverseLerp(minDist, maxDist, targetDistance);

        //rb.velocity = (original.position - transform.position).normalized * spd * interpValue * Time.deltaTime;
        //}

        //float angleBetween = Vector3.Angle(original.position, transform.position);

        //if (angleBetween > minAngle)
        //{
        //    rb.angularVelocity 
        //}

        //transform.rotation = Quaternion.Lerp(transform.rotation, original.rotation, 0.05f);

        ApplyShakeFactor();
    }

    IEnumerator RetractCountdown()
    {
        yield return new WaitForSeconds(retractTime);
        enabled = false;
    }
    
    public void SetDirection(Vector3 v)
    {
        direction = v.normalized;
    }

    public void ApplyShakeFactor()
    {
        transform.position += Random.insideUnitSphere * shakeFactor;
    }

    public void SetShakeFactor(float s)
    {
        shakeFactor = s;
    }

    public void SetInterp(float v)
    {
        //interpValue = v;
    }
}
