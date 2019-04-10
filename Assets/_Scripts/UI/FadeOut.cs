using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    [SerializeField]
    List<Image> quads = new List<Image>();

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
        forFading = quads[0].color;
        forFading.a = 0.0f;

        for (int i = 0; i < quads.Count; i++)
        {
            quads[i].color = forFading;
        }

        if (fadeInOnStart) BeginFadeIn();
    }

    public float GetFadeTime()
    {
        return fadeTime;
    }

    public void BeginFadeOut(float delay = 0.0f)
    {
        fadeTimer = 0.0f;
        StartCoroutine("FadeToBlack", delay);
    }

    public void BeginFadeIn(float delay = 0.0f)
    {
        StartCoroutine("FadeIn", delay);
    }

    IEnumerator FadeToBlack(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay);


        while (fadeTimer <= fadeTime)
        {

            fadeTimer += Time.deltaTime;
            alpha = UtilMath.Lmap(fadeTimer, 0.0f, fadeTime, 0.0f, 1.0f);

            forFading.a = alpha;
            //Debug.Log(alpha);


            for (int i = 0; i < quads.Count; i++)
            {
                quads[i].color = forFading;
            }

            yield return new WaitForEndOfFrame();
        }
        
        fadeTimer = 0.0f;
    }

    IEnumerator FadeIn(float delay = 0.0f)
    {
        while (fadeTimer <= fadeTime)
        {
            yield return new WaitForSeconds(delay);

            fadeTimer += Time.deltaTime;
            alpha = UtilMath.Lmap(fadeTimer, 0.0f, fadeTime, 1.0f, 0.0f);

            forFading.a = alpha;

            for (int i = 0; i < quads.Count; i++)
            {
                quads[i].color = forFading;
            }
        }
    }
}
