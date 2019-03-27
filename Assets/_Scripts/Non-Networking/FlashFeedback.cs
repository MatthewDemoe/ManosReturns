using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;


public class FlashFeedback : MonoBehaviour
{
    [Header("Body Parts")]
    [SerializeField]
    List<Renderer> bodyParts;

    [Header("Damaged Feedback Properties")]
    [SerializeField]
    float timeOn = 0.04f;

    [SerializeField]
    float timeOff = 0.04f;

    [SerializeField]
    int numFlashes = 3;

    [Header("Charging Feedback Properties")]
    [SerializeField]
    float startDelay = 0.25f;

    [SerializeField]
    float minDelay = 0.1f;

    [SerializeField]
    float maxDelay = 0.5f;

    MovementManager move;

    int _damageFlashCounter = 0;
    int _cdFlashCounter = 0;
    bool _charging = false;

    [Header("Death Feedback Properties")]
    [SerializeField]
    int numDeathFlashes = 10;

    int _deathFlashCounter = 0;

    [SerializeField]
    float deathOnTime = 0.2f;

    [SerializeField]
    float deathOffTime = 0.2f;

    [SerializeField]
    float timeBetweenExplosions = 1.0f;

    [SerializeField]
    GameObject deathParticles;



    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<MovementManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReactToDamage(float warmup)
    {
        StartCoroutine("damageFlash", warmup);
    }

    public void CooldownReact()
    {
        StartCoroutine("cooldownFlash");
    }

    public void ChargingFeedback()
    {
        if(!_charging)
            StartCoroutine("chargingFlash");
    }

    public void ManosDeath()
    {
        StartCoroutine("manosDeathFlashes");
        StartCoroutine("manosDeathExplosions");
        StartCoroutine("chadWinScene");

    }

    public void ChadDeath()
    {
        StartCoroutine("manosWinScene");
    }

    IEnumerator damageFlash(float warmup)
    {
        yield return new WaitForSeconds(warmup);

        while (_damageFlashCounter < numFlashes)
        {
            _damageFlashCounter++;

            for (int i = 0; i < bodyParts.Count; i++)
            {
                bodyParts[i].material.SetFloat("_Glow", 0.75f);
            }

            yield return new WaitForSeconds(timeOn);

            for (int i = 0; i < bodyParts.Count; i++)
            {
                bodyParts[i].material.SetFloat("_Glow", 0.0f);
            }

            yield return new WaitForSeconds(timeOff);

        }

        _damageFlashCounter = 0;
    }

    IEnumerator cooldownFlash()
    {
        StopCoroutine("chargingFlash");

        while (_cdFlashCounter < numFlashes)
        {
            _cdFlashCounter++;

            for (int i = 0; i < bodyParts.Count; i++)
            {
                bodyParts[i].material.SetFloat("_Glow", -0.9f);
            }

            yield return new WaitForSeconds(timeOn);

            for (int i = 0; i < bodyParts.Count; i++)
            {
                bodyParts[i].material.SetFloat("_Glow", 0.0f);
            }

            yield return new WaitForSeconds(timeOff);

        }

        if (_charging)
            StartCoroutine("chargingFlash");

        _cdFlashCounter = 0;
    }

    IEnumerator chargingFlash()
    {
        _charging = true;
        yield return new WaitForSeconds(startDelay);

        while (move.GetChargePercent() > 0.0f)
        {
            float chargePercent = move.GetChargePercent();
            float waitTime = Mathf.Lerp(maxDelay, minDelay, chargePercent);

            for (int i = 0; i < bodyParts.Count; i++)
            {
                bodyParts[i].material.SetFloat("_Glow", 0.75f);
            }

            yield return new WaitForSeconds(waitTime);

            for (int i = 0; i < bodyParts.Count; i++)
            {
                bodyParts[i].material.SetFloat("_Glow", 0.0f);
            }

            yield return new WaitForSeconds(waitTime);

        }

        _charging = false;
    }

    IEnumerator manosDeathFlashes()
    {
        while (true)
        {
            for (int i = 0; i < bodyParts.Count; i++)
            {
                bodyParts[i].material.SetFloat("_Glow", 0.75f);
            }

            yield return new WaitForSeconds(deathOnTime);

            for (int i = 0; i < bodyParts.Count; i++)
            {
                bodyParts[i].material.SetFloat("_Glow", 0.0f);
            }

            yield return new WaitForSeconds(deathOffTime);

            _deathFlashCounter++;
        }


    }

    IEnumerator manosDeathExplosions()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenExplosions);


            Vector3 pos = Random.insideUnitSphere * 3.0f;
            Instantiate(deathParticles, GameObject.Find("NeckBase").transform.position + pos, Quaternion.identity);
        }
    }

    IEnumerator chadWinScene()
    {
        float waitTime = GameObject.Find("OverlordController").GetComponent<FadeOut>().GetFadeTime();
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene("MatthewCinematic");
    }

    IEnumerator manosWinScene()
    {
        float waitTime = GameObject.Find("OverlordController").GetComponent<FadeOut>().GetFadeTime();
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene("ManosWins");

    }


}
