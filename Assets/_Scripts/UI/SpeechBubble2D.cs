using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Handles all animations within a speech bubble, and scaling the speech bubble according to text size
/// </summary>
public class SpeechBubble2D : MonoBehaviour
{
    [SerializeField]
    Vector3 quipTarget;
    [SerializeField]
    Text myText;
    [SerializeField]
    Image myBubbleImage;
    [SerializeField]
    RectTransform offset;

    [SerializeField]
    public Image chargeBar;
    private float _chargeState = 0.0f;

    private float _chargeSpeed = 0.5f;

    public bool prompting = false;

    // where the speech bubble should be by default
    Vector3 _defaultLoc;
    RectTransform _myRect;

    // used for text bubble shake
    private float _decayingShakeMagnitude;


    // full text to appear in the speech bubble
    string _message;

    // used in animations
    private IEnumerator _animationCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        _myRect = GetComponent<RectTransform>();
        _animationCoroutine = null;
        CrossfadeTo(0.0f, 0.0f);

        // play animation
        StartCoroutine(Crt_UIState());
    }

    // Update is called once per frame
    void Update()
    {
    }

    void PromptedBehaviour()
    {
        if (chargeBar)
        {
            chargeBar.rectTransform.localScale = Vector3.one;
            float alpha = 1.0f + Mathf.Sin(Mathf.PI * 2.0f * Time.time * SpeechBubble2DFlyweight.Instance.promptedFlashFrequency);
            Vector4 col = new Vector4(chargeBar.color.r, chargeBar.color.g, chargeBar.color.b, alpha * 1.0f);
            chargeBar.color = col;
        }
    }

    void ReadyBehaviour()
    {
        if (chargeBar)
        {
            chargeBar.rectTransform.localScale = Vector3.one;
            float alpha = 1.0f + Mathf.Sin(Mathf.PI * 2.0f * Time.time * SpeechBubble2DFlyweight.Instance.readyFlashFrequency);
            Vector4 col = new Vector4(chargeBar.color.r, chargeBar.color.g, chargeBar.color.b, alpha * 1.0f);
            chargeBar.color = col;
        }
    }



    void ChargingBehaviour()
    {
        if (chargeBar)
        {
            float alpha = 0.8f; //+ Mathf.Sin(Mathf.PI * 2.0f * Time.time * SpeechBubble2DFlyweight.Instance.readyFlashFrequency);
            Vector4 col = new Vector4(chargeBar.color.r, chargeBar.color.g, chargeBar.color.b, alpha);
            chargeBar.color = col;

            Vector3 scal = chargeBar.rectTransform.localScale;
            scal.x = _chargeState;
            chargeBar.rectTransform.localScale = scal;
        }
    }

    void DisabledBehaviour()
    {
        if (chargeBar)
        {
            chargeBar.rectTransform.localScale = Vector3.one;

            Vector4 col = new Vector4(0.0f, 0.0f, 0.0f, QuipBattleContext.SPEECH_OPACITY_STALE);
            chargeBar.color = col;
        }
    }
    
    public bool PlayFreshQuip(string quiptext, float damage, float time = 3.0f)
    {
        Debug.Log("playing quip. chargeState: " + _chargeState);

        if (_chargeState == 1.0f) // if able to quip
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }

            // reset position
            _defaultLoc = GetComponentInParent<RectTransform>().localPosition;
            _myRect.position = _defaultLoc;
            _myRect.localPosition = Vector3.zero;

            // play animation
            _animationCoroutine = Crt_PlayQuip(quiptext, damage, time);
            StartCoroutine(_animationCoroutine);

            return true;
        } else return false;
    }

    private IEnumerator Crt_PlayQuip(string quiptext, float damage, float time = 5.0f)
    {
        prompting = false;
        // pause charge animations
        _chargeState = -1.0f;
        // reset opacity
        SetAlpha(QuipBattleContext.SPEECH_OPACITY_FRESH);

        // Type message 1 character at a time
        myText.text = "";
        _message = quiptext;
        float typingDelay;

        AudioManager.GetInstance().PlaySoundOnce(AudioManager.Sound.ChadVoice);

        foreach (char letter in _message.ToCharArray())
        {
            typingDelay = SpeechBubble2DFlyweight.Instance.letterTypingPauseNormal;

            // respond to punctuation marks
            if (letter == ',') typingDelay = SpeechBubble2DFlyweight.Instance.letterTypingPauseComma;
            else if (letter == '.') typingDelay = SpeechBubble2DFlyweight.Instance.letterTypingPausePeriod;
            else if (letter == '!') Shake(0.5f, 6.0f);

            myText.text += letter;
            //if (typeSound1 && typeSound2)
            //    SoundManager.instance.RandomizeSfx(typeSound1, typeSound2);
            yield return new WaitForSeconds(typingDelay);
        }


        // Animate translation of bubble to "attack" the quiptarget
        {
            float dt = 0.033f;
            float timerMax = 0.4f;
            float timer = timerMax;
            float normalizedTime;
            float nonlinearLerpVal;

            //Vector3 quipTarget = SpeechBubble2DFlyweight.Instance.quipTarget.localPosition;

            // towards target
            while (timer > 0)
            {
                normalizedTime = UtilMath.Lmap(timer, timerMax, 0.0f, 0.0f, 1.0f);
                nonlinearLerpVal = UtilMath.EasingFunction.EaseInQuint(0.0f, 0.9f, normalizedTime);
                Vector3 pos = Vector3.Lerp(_defaultLoc, quipTarget, nonlinearLerpVal);
                _myRect.localPosition = pos;

                yield return new WaitForSeconds(dt);
                timer -= dt;
            }


            // impact with target!
            Shake(0.5f, 6.0f);

            // test
            SpeechBubble2DFlyweight.Instance.egoBar.dealEgoDamage(damage);//SetBalanceValue(SpeechBubble2DFlyweight.Instance.egoBar._egoBalanceTarget + damage);
            // /test

            // away from target
            timerMax = 1.0f;
            timer = timerMax;
            while (timer > 0)
            {
                normalizedTime = UtilMath.Lmap(timer, timerMax, 0.0f, 0.0f, 1.0f);
                nonlinearLerpVal = UtilMath.EasingFunction.EaseOutBounce(0.8f, 0.0f, normalizedTime);
                Vector3 pos = Vector3.Lerp(_defaultLoc, quipTarget, nonlinearLerpVal);
                _myRect.localPosition = pos;

                yield return new WaitForSeconds(dt);
                timer -= dt;
            }
            _myRect.localPosition = Vector3.zero;
        }
        //

        // fade to low opacity after some time
        yield return new WaitForSeconds(time);
        CrossfadeTo(QuipBattleContext.SPEECH_OPACITY_STALE, QuipBattleContext.SPEECH_FADE_TIME);
        yield return new WaitForSeconds(QuipBattleContext.SPEECH_FADE_TIME);

        SetChargeState(0.0f);
        myText.text = "...";
    }

    /// <summary>
    /// Shakes a rectTransform for a certain duration
    /// </summary>
    public void Shake(float duration, float magnitude)
    {
        _decayingShakeMagnitude = magnitude;
        InvokeRepeating("RepeatingShake", duration, 0.033f);
    }

    // private method used by ShakeRect
    private void RepeatingShake()
    {
        offset.localPosition = new Vector2(Random.Range(-_decayingShakeMagnitude, _decayingShakeMagnitude), Random.Range(-_decayingShakeMagnitude, _decayingShakeMagnitude));
        _decayingShakeMagnitude /= 1.2f;
    }

    public void CrossfadeTo(float alpha, float duration)
    {
        myText.CrossFadeAlpha(alpha, alpha, false);
        myBubbleImage.CrossFadeAlpha(alpha, alpha, false);
    }

    public void SetAlpha(float alpha)
    {
        myText.canvasRenderer.SetAlpha(alpha);
        myBubbleImage.canvasRenderer.SetAlpha(alpha);
    }
    public void SetCooldownRechargeRate(float cooldownTime)
    {
        _chargeSpeed = 1.0f / cooldownTime;
    }

    private void SetChargeState(float charge)
    {
        _chargeState = charge;
    }

    public bool IsCharged()
    {
        if (_chargeState >= 1.0f) return true;
        else return false;
    }

    public void AttemptPromptUser(float duration)
    {
        // wew
        if (_chargeState >= 1.0f) // able to quip
        {
            prompting = true;
            StartCoroutine(Crt_Prompted(duration));
        }
        else // unable to quip
        {
            //DisabledBehaviour();
        }
    }

    private IEnumerator Crt_Prompted(float duration)
    {
        InvokeRepeating("PromptedBehaviour", duration, Time.fixedDeltaTime);
        yield return new WaitForSeconds(duration);
        prompting = false;
    }

    private IEnumerator Crt_UIState()
    {
        float dt = 0.033f;

        while (true)
        {
            while (_chargeState < 0.0f)
            {
                chargeBar.rectTransform.localScale = Vector3.one;
                Vector4 col = new Vector4(chargeBar.color.r, chargeBar.color.g, chargeBar.color.b, 0.0f);
                chargeBar.color = col;
                yield return new WaitForSeconds(dt);
            }

            while (_chargeState >= 0.0f && _chargeState < 1.0f)
            {
                ChargingBehaviour();

                // temp?
                SetChargeState(Mathf.Min((_chargeState + (_chargeSpeed) * Time.deltaTime), 1.0f));
                yield return new WaitForSeconds(dt);
            }

            while (_chargeState == 1.0f)
            {
                ReadyBehaviour();
                yield return new WaitForSeconds(dt);
            }
        }
    }
}

