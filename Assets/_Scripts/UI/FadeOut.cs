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

    public void BeginFadeOut(float delay = 0.0f)
    {
        StartCoroutine("FadeToBlack");
    }

    public void BeginFadeIn()
    {
        StartCoroutine("FadeIn", delay);
    }

    IEnumerator FadeToBlack()
    {
        fadeTimer = 0.0f;

        yield return new WaitForSeconds(delay);

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
    }

    IEnumerator FadeIn()
    {
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
    }
}
