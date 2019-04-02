using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statbar : MonoBehaviour
{
    //Components
    Image bar, backBar, tween;
    RectTransform rect;
    public bool useTween;
    public bool isStamina;
    float delay = 0.25f;
    float delayTimer;
    float changeSpeed = 1;

    // Use this for initialization
    void Start()
    {
        backBar = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        if (useTween)
            tween = transform.GetChild(0).GetComponent<Image>();
        bar = transform.GetChild(1).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (useTween)
        {
            if (tween.fillAmount > bar.fillAmount)
            {
                if (delayTimer <= 0)
                {
                    tween.fillAmount -= changeSpeed * Time.deltaTime;
                }
                else if (delayTimer > 0)
                {
                    delayTimer -= Time.deltaTime;
                }
            }
        }
    }

    public void UpdateCurrentValue(float newVal)
    {
        bar.fillAmount = newVal;
        if (useTween)
        {
            if (bar.fillAmount > tween.fillAmount) tween.fillAmount = newVal;
            delayTimer = delay;
        }
    }

    public void UpdateMaxValue(float newVal)
    {
        if (isStamina)
        {
            rect.sizeDelta = new Vector2(newVal * 4, rect.sizeDelta.y);
        }
        else
        {
            rect.sizeDelta = new Vector2(newVal, rect.sizeDelta.y);
        }
    }
}
