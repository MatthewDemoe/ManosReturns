using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Handles Ego Battle Bar
/// </summary>
public class StatusBarController : MonoBehaviour
{
    [SerializeField]
    Animator UIAnimator;
    

    [Header("Portraits")]

    [SerializeField]
    RectTransform portraitManos;

    [SerializeField]
    RectTransform portraitChad;

    [SerializeField]
    float portraitMaxScale = 2.0f;


    ///
    [Header("Ego Thresholds")]

    [SerializeField]
    float THRESH_DIRE = 0.085f;

    [SerializeField]
    float THRESH_ADVANTAGE = 0.33f;


    [Header("Ego Bar")]

    [SerializeField]
    RectTransform barMaskManos;

    [SerializeField]
    RectTransform barMaskChad;

    [SerializeField]
    RectTransform balanceMarker;

    [SerializeField]
    float egoBarScale = 5.0f;
    [SerializeField]
    float egoBarHalfLength = 250.0f;

    // how fast the bar reaches its target position, in % per second
    [SerializeField]
    float barCatchupSpeed = 0.5f;

    // target state
    public float _egoBalanceTarget = 0.5f;
    // visible state
    float balanceVisible = 0.5f;


    int _egoState = 2;

    /// <summary>
    /// 
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move bar balance value towards target
        float difference = (_egoBalanceTarget - balanceVisible) * Time.deltaTime;
        float barDistanceToTravel = barCatchupSpeed * Time.deltaTime;

        if (Mathf.Abs(difference) > Mathf.Abs(barDistanceToTravel))
        {
            balanceVisible += Mathf.Sign(difference) * barDistanceToTravel;
        } else
        {
            balanceVisible += difference;
        }

        UpdatePortraits();

        // update masks
        Vector3 maskUIScale;

        // Manos bar
        maskUIScale = barMaskManos.localScale;
        maskUIScale.x = egoBarScale * (1.0f - balanceVisible);
        barMaskManos.localScale = maskUIScale;

        // Chad bar
        maskUIScale = barMaskChad.localScale;
        maskUIScale.x = egoBarScale * balanceVisible;
        barMaskChad.localScale = maskUIScale;

        // Set the UI marker
        Vector3 balancePosition = balanceMarker.localPosition;
        balancePosition.x = egoBarHalfLength * ((_egoBalanceTarget * 2.0f) - 1.0f);
        balanceMarker.localPosition = balancePosition;


        // check for updates to new gamestate based on ego balance
        int newEgoState = 4;
        if (_egoBalanceTarget <= THRESH_DIRE) newEgoState = 0;
         else if (_egoBalanceTarget <= THRESH_ADVANTAGE) newEgoState = 1;
             else if (_egoBalanceTarget <= 1.0f - THRESH_ADVANTAGE) newEgoState = 2;
                 else if (_egoBalanceTarget <= 1.0f - THRESH_DIRE) newEgoState = 3;

        if(newEgoState != _egoState)
        {
            _egoState = newEgoState;
            Debug.Log("ego balance:" + _egoBalanceTarget + "ego state: " + newEgoState);
            UIAnimator.SetInteger("EgoState", _egoState);
        }
    }

    /// <summary>
    /// Set the state of battle from 0 to 1, 1 being Chad winning, and 0 being Manos winning
    /// </summary>
    /// <param name="egoBalance"></param>
    public void SetBalanceValue(float egoBalance)
    {
        _egoBalanceTarget = Mathf.Clamp(egoBalance, 0.0f, 1.0f);
        DamageResponse();
    }

    public void dealEgoDamage(float damage)
    {
        _egoBalanceTarget = Mathf.Clamp(_egoBalanceTarget + damage, 0.0f, 1.0f);
        DamageResponse();
    }

    public void dealEgoDamage(Enums.Player p, float damage)
    {
        if (p == Enums.Player.Player1)
            _egoBalanceTarget += damage;
        if (p == Enums.Player.Manos)
            _egoBalanceTarget -= damage;
        DamageResponse();
    }

    void DamageResponse()
    {
        balanceMarker.GetComponentInChildren<UIShake>().Shake(0.9f, 5.0f);
    }

    void UpdatePortraits()
    {
        float scaleValManos = 1.0f + ((1.0f - balanceVisible) * (portraitMaxScale - 1.0f));
        float scaleValChad = 1.0f + (balanceVisible * (portraitMaxScale - 1.0f));

        portraitManos.localScale = new Vector3(scaleValManos, scaleValManos, scaleValManos);
        portraitChad.localScale = new Vector3(scaleValChad, scaleValChad, scaleValChad);
    }
}

//<><
