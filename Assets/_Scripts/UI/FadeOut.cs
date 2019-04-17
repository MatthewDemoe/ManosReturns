using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VikingCrewTools.UI;

public class FadeOut : MonoBehaviour
{
    [SerializeField]
    List<Wilberforce.FinalVignette.FinalVignetteCommandBuffer> vignettes;

    [SerializeField]
    float fadeTime = 1.0f;

    [SerializeField]
    float delay = 0.0f;

    float fadeTimer = 0.0f;

    float alpha = 0.0f;

    Color forFading;

    [SerializeField]
    bool fadeInOnStart = false;

    //used to flag when Fadeout is already working
    IEnumerator _currentFade = null;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < vignettes.Count; i++)
        {
            vignettes[i].VignetteOuterColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            vignettes[i].VignetteOuterValueDistance = 0.0f;
        }

        if (fadeInOnStart) BeginFadeIn();
    }

    public float GetFadeTime()
    {
        return fadeTime;
    }

    public void BeginFadeOut(float _delay = 0.0f, float duration = 20.0f)
    {
        if(_currentFade == null)
        {
            _currentFade = FadeToBlack(_delay, duration);
            StartCoroutine(_currentFade);
        }
    }

    public void BeginFadeIn()
    {
        if (_currentFade == null)
        {
            _currentFade = FadeIn();
            StartCoroutine(_currentFade);
        }
    }

    IEnumerator FadeToBlack(float _delay = 0.0f, float holdTime = 10.0f)
    {
        SetVignetteInterruptable(false);

        yield return new WaitForSeconds(_delay);

        fadeTimer = 0.0f;

        while (fadeTimer <= fadeTime)
        {
            fadeTimer += Time.deltaTime;
            alpha = UtilMath.Lmap(fadeTimer, 0.0f, fadeTime, 1.0f, 0.0f);
            alpha = Mathf.Clamp(alpha, 0.0f, 1.0f);

            for (int i = 0; i < vignettes.Count; i++)
            {
                vignettes[i].VignetteOuterColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                vignettes[i].VignetteOuterValueDistance = alpha;
            }

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(holdTime);

        SetVignetteInterruptable(true);
        _currentFade = null;
    }

    IEnumerator FadeIn()
    {
        SetVignetteInterruptable(false);

        while (fadeTimer <= fadeTime)
        {

            fadeTimer += Time.deltaTime;
            alpha = UtilMath.Lmap(fadeTimer, 0.0f, fadeTime, 0.0f, 1.5f);

            for (int i = 0; i < vignettes.Count; i++)
            {
               vignettes[i].VignetteOuterValueDistance = alpha;
            }

            yield return new WaitForEndOfFrame();
        }

        SetVignetteInterruptable(true);
        _currentFade = null;
    }

    void SetVignetteInterruptable(bool what)
    {
        for (int i = 0; i < vignettes.Count; i++)
        {
            vignettes[i].isInterruptable = what;
        }
    }
}
