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

    [Tooltip("minimum and maximums for vignette intensity based on damage")]
    [SerializeField]
    Vector2 vignetteScaleRange = new Vector2(0.1f, 1.0f);

    [Tooltip("values of damage dealt that will produce the minimum and maximum vignette intensities")]
    [SerializeField]
    Vector2 damageRange = new Vector2(20.0f, 80.0f);

    [Tooltip("The health percentage at which the vignette will be at its max value")]
    [SerializeField]
    float healthPercentageAtMax = 0.25f;

    [SerializeField]
    float vignetteDuration = 0.5f;

    [SerializeField]
    float vignetteFadeDelay = 0.25f;

    float _vignetteTimer = 0.0f;

    [SerializeField]
    Color damageColor = Color.white;

    [SerializeField]
    Color healColor = Color.white;

    [SerializeField]
    Color currentOuterColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        vig = GetComponent<Wilberforce.FinalVignette.FinalVignetteCommandBuffer>();

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
            StartCoroutine(FlashVignette(vignetteScaleRange.y));
        }
        else
        {
            StopCoroutine("FlashVignette");

            vig.VignetteOuterValueDistance = vignetteScaleRange.y;
        }
    }

    public void SetVignetteDamage(float amount)
    {
        //SetVignetteFill(amount);
        DamagePulse(amount);
    }

    public void SetVignetteHeal(float healedDamage)
    {
        if (vig.isInterruptable)
        {
            float val = UtilMath.Lmap(healedDamage, damageRange.x, damageRange.y, vignetteScaleRange.x, vignetteScaleRange.y);
            val = Mathf.Clamp(val, vignetteScaleRange.x, vignetteScaleRange.y);

            currentOuterColor = healColor;

            _vignetteTimer = vignetteDuration;


            StopCoroutine("FlashVignette");
            StartCoroutine(FlashVignette(val));
        }
    }

    IEnumerator FlashVignette(float value)
    {
        _vignetteTimer = vignetteDuration;
        UpdateVignetteCol(value);

        yield return new WaitForSeconds(vignetteFadeDelay);

        while (_vignetteTimer > 0.0f)
        {
            UpdateVignetteCol(value);
            _vignetteTimer -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    public void DamagePulse(float damageDealt = 10.0f)
    {
        if (vig.isInterruptable)
        {
            currentOuterColor = damageColor;

            float val = UtilMath.Lmap(damageDealt, damageRange.x, damageRange.y, vignetteScaleRange.x, vignetteScaleRange.y);
            val = Mathf.Clamp(val, vignetteScaleRange.x, vignetteScaleRange.y);

            StopCoroutine("FlashVignette");
            StartCoroutine(FlashVignette(val));
        }
    }

    void UpdateVignetteCol(float value)
    {
        float val = UtilMath.Lmap(_vignetteTimer, vignetteDuration, 0.0f, value, 0.0f);
        //vig.VignetteOuterValue = val;
        Color newCol = Color.Lerp(new Color(0.0f, 0.0f, 0.0f, 0.0f), currentOuterColor, val);

        vig.VignetteOuterValueDistance = 1.0f - val;

        //vig.VignetteOuterColor;
        //newCol.a = val;
        vig.VignetteOuterColor = newCol;
    }
}
