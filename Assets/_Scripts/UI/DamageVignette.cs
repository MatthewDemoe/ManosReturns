using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVignette : MonoBehaviour
{

    Wilberforce.FinalVignette.FinalVignetteCommandBuffer vig;

    [SerializeField]
    float outerValue = 1.0f;

    [SerializeField]
    float innerValue = 0.0f;

    [SerializeField]
    float outerValueDistance = 3.0f;

    [SerializeField]
    float innerValueDistance = 0.0f;

    [SerializeField]
    float maxValue = 0.75f;

    [Tooltip("The health percentage at which the vignette will be at its max value")]
    [SerializeField]
    float healthPercentageAtMax = 0.25f;

    [SerializeField]
    float vignetteDuration = 0.5f;

    [SerializeField]
    float vignetteFadeDelay = 0.25f;

    float _vignetteTimer = 0.0f;

    [SerializeField]
    Color damageColor;

    [SerializeField]
    Color healColor;

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.H))
    //    {
    //        SetVignetteHeal(0.5f);
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        vig = GetComponent<Wilberforce.FinalVignette.FinalVignetteCommandBuffer>();

        //vig.VignetteOuterValueDistance = outerValueDistance;
        vig.VignetteInnerValueDistance = innerValueDistance;
        vig.VignetteOuterValue = outerValue;
        vig.VignetteInnerValue = innerValue;
    }

    void SetVignetteFill(float amount)
    {
        if (amount >= healthPercentageAtMax)
        {
            _vignetteTimer = vignetteDuration;

            StopCoroutine("FlashVignette");
            StartCoroutine("FlashVignette");
        } else
        {
            StopCoroutine("FlashVignette");

            vig.VignetteOuterValueDistance = maxValue;
        }
    }

    public void SetVignetteDamage(float amount)
    {
        vig.VignetteOuterColor = damageColor;
        SetVignetteFill(amount);
    }

    public void SetVignetteHeal(float amount)
    {
        vig.VignetteOuterColor = healColor;

        _vignetteTimer = vignetteDuration;

        StopCoroutine("FlashVignette");
        StartCoroutine("FlashVignette");
    }

    IEnumerator FlashVignette()
    {
        float val = UtilMath.Lmap(_vignetteTimer, vignetteDuration, 0.0f, maxValue, outerValueDistance);

        vig.VignetteOuterValueDistance = val;

        yield return new WaitForSeconds(vignetteFadeDelay);

        while (_vignetteTimer > 0.0f)
        {
            val = UtilMath.Lmap(_vignetteTimer, vignetteDuration, 0.0f, maxValue, outerValueDistance);

            vig.VignetteOuterValueDistance = val;
            _vignetteTimer -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

}
