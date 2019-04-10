using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class AnimeEffects : MonoBehaviour
{
    [SerializeField]
    Image _animeSpeedLines;
    [SerializeField]
    float timeIn = 0.1f;
    [SerializeField]
    float timeOut = 0.2f;

    [SerializeField]
    float maxSpeedOpacity = 0.3f;

    [SerializeField]
    Vector2 scaleRange;

    Vector3 _baseScale;

    UIShake shaker;

    IEnumerator _coroutine = null;

    void Start()
    {
        shaker = GetComponent<UIShake>();
        _animeSpeedLines.CrossFadeAlpha(0.0f, 0.01f, true);
        _baseScale = _animeSpeedLines.transform.localScale;
    }

    public void Play()
    {
        Cancel();

        _coroutine = LerpInOut();
        
        StartCoroutine(_coroutine);
        shaker.Shake();
    }

    public void Play(float duration, float speedLinesIntensity = 0.1f, float opacityScale = 0.3f, float shakeIntensity = 20.0f)
    {
        Cancel();

        float scale = Mathf.Lerp(scaleRange.x, scaleRange.y, speedLinesIntensity);
        _coroutine = LerpInOut(duration, opacityScale * maxSpeedOpacity, scale);

        StartCoroutine(_coroutine);
        shaker.Shake(duration, shakeIntensity);
    }

    public void Cancel()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            //StartCoroutine(CancelCrt(timeOut));
        }
        _animeSpeedLines.CrossFadeAlpha(0.0f, timeOut, false);
    }

    //IEnumerator CancelCrt(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    StopCoroutine(_coroutine);
    //}

    IEnumerator LerpInOut()
    {
        _animeSpeedLines.transform.localScale = _baseScale;

        _animeSpeedLines.CrossFadeAlpha(1.0f, timeIn, false);
        yield return new WaitForSeconds(timeIn);
        _animeSpeedLines.CrossFadeAlpha(0.0f, timeOut, false);
        yield return new WaitForSeconds(timeOut);

        _coroutine = null;
    }

    IEnumerator LerpInOut(float duration, float opacity, float scale)
    {
        _animeSpeedLines.transform.localScale = _baseScale * scale;

        _animeSpeedLines.CrossFadeAlpha(opacity, timeIn, false);
        yield return new WaitForSeconds(duration);
        _animeSpeedLines.CrossFadeAlpha(0.0f, timeOut, false);
        yield return new WaitForSeconds(timeOut);

        _coroutine = null;
    }
}
