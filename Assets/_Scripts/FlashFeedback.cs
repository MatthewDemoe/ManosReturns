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

    [SerializeField]
    float waitOnChadDeath = 5.0f;

    float waitOnManosDeath = 5.0f;

    //More variables for Manos
    [SerializeField]
    bool flashL;
    [SerializeField]
    bool flashR;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<MovementManager>();
    }

    // Update is called once per frame
    /*
    void Update()
    {

    }
    */

    /// <summary>
    /// This method enables the damageFlash effect if a body part is hit
    /// </summary>
    /// <param name="warmup"></param>
    /// <param name="bodyPart"></param>
    public void ReactToDamage(float warmup, Enums.ManosParts bodyPart)
    {
        switch (bodyPart)
        {
            case Enums.ManosParts.None:
                StartCoroutine(damageFlash(warmup));
                break;
            case Enums.ManosParts.Head:
            case Enums.ManosParts.LeftHand:
            case Enums.ManosParts.RightHand:
            case Enums.ManosParts.LeftVambrace:
            case Enums.ManosParts.RightVambrace:
            case Enums.ManosParts.Chest:
                StartCoroutine(damageFlash(warmup, bodyPart));
                break;
        }
    }

    /// <summary>
    /// Flash left arm or white arm
    /// </summary>
    /// <param name="h">This better be either LeftHand or RightHand</param>
    public void ManosChargeActivate(Enums.Hand h)
    {
        switch (h)
        {
            case Enums.Hand.Left:
                flashL = true;
                StartCoroutine(chargingFlashManosLeft());
                break;
            case Enums.Hand.Right:
                flashR = true;
                StartCoroutine(chargingFlashManosRight());
                break;
        }
    }

    /// <summary>
    /// Deactivate flashing for left/right arm
    /// </summary>
    /// <param name="h">The hand in question</param>
    /// <param name="value">The hand's health, affecting it's shade</param>
    public void ManosChargeDeactivate(Enums.Hand h, float value)
    {
        float theVal = Mathf.Lerp(-0.9f, 0, value);
        switch (h)
        {
            case Enums.Hand.Left:
                flashL = false;
                StopCoroutine(chargingFlashManosLeft());
                break;
            case Enums.Hand.Right:
                flashR = false;
                StopCoroutine(chargingFlashManosRight());
                break;
        }
        switch (h)
        {
            case Enums.Hand.Left:
                bodyParts[(int)Enums.ManosParts.LeftHand].material.SetFloat("_Glow", theVal);
                bodyParts[(int)Enums.ManosParts.LeftVambrace].material.SetFloat("_Glow", theVal);
                break;
            case Enums.Hand.Right:
                bodyParts[(int)Enums.ManosParts.RightHand].material.SetFloat("_Glow", theVal);
                bodyParts[(int)Enums.ManosParts.RightVambrace].material.SetFloat("_Glow", theVal);
                break;
        }
    }

    public void CooldownReact()
    {
        StartCoroutine("cooldownFlash");
    }

    public void ChargingFeedback()
    {
        if (!_charging)
        {
            StartCoroutine("chargingFlash");
        }
    }

    public void ManosDeath()
    {
        StartCoroutine("manosDeathFlashes");
        StartCoroutine("manosDeathExplosions");
        StartCoroutine("chadWinScene");

    }
    public void ManosPlayDeath()
    {
        StartCoroutine("manosDeathFlashes");
        StartCoroutine("manosDeathExplosions");
      

    }
    public void ManosStopDeath()
    {
        StopCoroutine("manosDeathFlashes");
        StopCoroutine("manosDeathExplosions");


    }

    public void ChadDeath()
    {
        StartCoroutine("manosWinScene");
    }
    public void FlashReset()
    {
        for (int i = 0; i < bodyParts.Count; i++)
        {
            bodyParts[i].material.SetFloat("_Glow", 0.0f);
        }
        ManosStopDeath();
    }


    /// <summary>
    /// This method allows a non-Manos part or object to flash when taking damage
    /// </summary>
    /// <param name="warmup"></param>
    /// <returns></returns>
    IEnumerator damageFlash(float warmup)
    {

        StopCoroutine("chargingFlash");

        yield return new WaitForSeconds(warmup);

        while (_cdFlashCounter < numFlashes)
        {
            _cdFlashCounter++;

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

        if (_charging)
        {
            StartCoroutine("chargingFlash");
        }
            

        _cdFlashCounter = 0;
    }

    IEnumerator damageFlash(float warmup, Enums.ManosParts bodyPart)
    {
        float ogValue = bodyParts[(int)bodyPart].material.GetFloat("_Glow");

        yield return new WaitForSeconds(warmup);

        while (_damageFlashCounter < numFlashes)
        {
            _damageFlashCounter++;

            switch (bodyPart)
            {
                case Enums.ManosParts.LeftArm:
                    bodyParts[(int)Enums.ManosParts.LeftHand].material.SetFloat("_Glow", 0.75f);
                    bodyParts[(int)Enums.ManosParts.LeftVambrace].material.SetFloat("_Glow", 0.75f);
                    break;
                case Enums.ManosParts.RightArm:
                    bodyParts[(int)Enums.ManosParts.RightHand].material.SetFloat("_Glow", 0.75f);
                    bodyParts[(int)Enums.ManosParts.RightVambrace].material.SetFloat("_Glow", 0.75f);
                    break;
                case Enums.ManosParts.Head:
                case Enums.ManosParts.LeftHand:
                case Enums.ManosParts.RightHand:
                case Enums.ManosParts.LeftVambrace:
                case Enums.ManosParts.RightVambrace:
                case Enums.ManosParts.Chest:
                    bodyParts[(int)bodyPart].material.SetFloat("_Glow", 0.75f);
                    break;
            }

            yield return new WaitForSeconds(timeOn);

            switch (bodyPart)
            {
                case Enums.ManosParts.LeftArm:
                    bodyParts[(int)Enums.ManosParts.LeftHand].material.SetFloat("_Glow", 0.0f);
                    bodyParts[(int)Enums.ManosParts.LeftVambrace].material.SetFloat("_Glow", 0.0f);
                    break;
                case Enums.ManosParts.RightArm:
                    bodyParts[(int)Enums.ManosParts.RightHand].material.SetFloat("_Glow", 0.0f);
                    bodyParts[(int)Enums.ManosParts.RightVambrace].material.SetFloat("_Glow", 0.0f);
                    break;
                case Enums.ManosParts.Head:
                case Enums.ManosParts.LeftHand:
                case Enums.ManosParts.RightHand:
                case Enums.ManosParts.LeftVambrace:
                case Enums.ManosParts.RightVambrace:
                case Enums.ManosParts.Chest:
                    bodyParts[(int)bodyPart].material.SetFloat("_Glow", 0.0f);
                    break;
            }

            yield return new WaitForSeconds(timeOff);

        }

        switch (bodyPart)
        {
            case Enums.ManosParts.LeftArm:
                bodyParts[(int)Enums.ManosParts.LeftHand].material.SetFloat("_Glow", ogValue);
                bodyParts[(int)Enums.ManosParts.LeftVambrace].material.SetFloat("_Glow", ogValue);
                break;
            case Enums.ManosParts.RightArm:
                bodyParts[(int)Enums.ManosParts.RightHand].material.SetFloat("_Glow", ogValue);
                bodyParts[(int)Enums.ManosParts.RightVambrace].material.SetFloat("_Glow", ogValue);
                break;
            case Enums.ManosParts.Head:
            case Enums.ManosParts.LeftHand:
            case Enums.ManosParts.RightHand:
            case Enums.ManosParts.LeftVambrace:
            case Enums.ManosParts.RightVambrace:
            case Enums.ManosParts.Chest:
                bodyParts[(int)bodyPart].material.SetFloat("_Glow", ogValue);
                break;
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

    IEnumerator chargingFlashManosLeft()
    {
        while (flashL)
        {
            bodyParts[(int)Enums.ManosParts.LeftHand].material.SetFloat("_Glow", 0.75f);
            bodyParts[(int)Enums.ManosParts.LeftVambrace].material.SetFloat("_Glow", 0.75f);

            yield return new WaitForSeconds(0.25f);

            bodyParts[(int)Enums.ManosParts.LeftHand].material.SetFloat("_Glow", 0.0f);
            bodyParts[(int)Enums.ManosParts.LeftVambrace].material.SetFloat("_Glow", 0.0f);

            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator chargingFlashManosRight()
    {
        while (flashR)
        {
            bodyParts[(int)Enums.ManosParts.RightHand].material.SetFloat("_Glow", 0.75f);
            bodyParts[(int)Enums.ManosParts.RightVambrace].material.SetFloat("_Glow", 0.75f);

            yield return new WaitForSeconds(0.25f);

            bodyParts[(int)Enums.ManosParts.RightHand].material.SetFloat("_Glow", 0.0f);
            bodyParts[(int)Enums.ManosParts.RightVambrace].material.SetFloat("_Glow", 0.0f);

            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator manosDeathFlashes()
    {
        while (true)
        {
            for (int i = 0; i < bodyParts.Count; i++)
            {
                bodyParts[i].material.SetFloat("_Glow", 0.4f);
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
        //float waitTime = GameObject.Find("OverlordController").GetComponent<FadeOut>().GetFadeTime();
        yield return new WaitForSeconds(waitOnManosDeath);

        AudioManager.GetInstance().StopMusic();

        SceneManager.LoadScene("ChadWins");
    }

    IEnumerator manosWinScene()
    {
        //float waitTime = GameObject.Find("OverlordController").GetComponent<FadeOut>().GetFadeTime();
        yield return new WaitForSeconds(waitOnChadDeath);

        AudioManager.GetInstance().StopMusic();
        
        SceneManager.LoadScene("ManosCinematic");
    }
}
