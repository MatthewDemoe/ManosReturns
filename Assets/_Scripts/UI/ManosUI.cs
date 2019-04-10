using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManosUI : MonoBehaviour
{
    [SerializeField]
    Image handVisibleL;
    Image eyeVL;
    [SerializeField]
    Image handVisibleR;
    Image eyeVR;

    [SerializeField]
    Image handHiddenL;
    Image eyeHL;
    [SerializeField]
    Image handHiddenR;
    Image eyeHR;

    [SerializeField]
    float crossFadeTime = 1.5f;

    bool visibleL;
    bool visibleR;

    // Start is called before the first frame update
    void Start()
    {
        eyeVL = handVisibleL.transform.GetChild(0).GetComponent<Image>();
        eyeVR = handVisibleR.transform.GetChild(0).GetComponent<Image>();
        eyeHL = handHiddenL.transform.GetChild(0).GetComponent<Image>();
        eyeHR = handHiddenR.transform.GetChild(0).GetComponent<Image>();

        handVisibleL.CrossFadeAlpha(0, Time.deltaTime, true);
        handHiddenL.CrossFadeAlpha(0, Time.deltaTime, true);
        handVisibleR.CrossFadeAlpha(0, Time.deltaTime, true);
        handHiddenR.CrossFadeAlpha(0, Time.deltaTime, true);

        eyeVL.CrossFadeAlpha(0, Time.deltaTime, true);
        eyeHL.CrossFadeAlpha(0, Time.deltaTime, true);
        eyeVR.CrossFadeAlpha(0, Time.deltaTime, true);
        eyeHR.CrossFadeAlpha(0, Time.deltaTime, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHandStatus(Enums.Hand hand, bool b)
    {
        switch (hand)
        {
            case Enums.Hand.Left:
                if (visibleL != b)
                {
                    AnimateHand(hand, b);
                }
                visibleL = b;
                break;
            case Enums.Hand.Right:
                if (visibleR != b)
                {
                    AnimateHand(hand, b);
                }
                visibleR = b;
                break;
        }
    }

    /// <summary>
    /// Animates GUI based on the visibility of your hands
    /// </summary>
    /// <param name="hand"></param>
    /// <param name="b"></param>
    public void AnimateHand(Enums.Hand hand, bool b)
    {
        switch (hand)
        {
            case Enums.Hand.Left:
                if (b)
                {
                    handVisibleL.CrossFadeAlpha(150, Time.deltaTime, true);
                    eyeVL.CrossFadeAlpha(255, Time.deltaTime, true);
                    handVisibleL.CrossFadeAlpha(0, crossFadeTime, true);
                    eyeVL.CrossFadeAlpha(0, crossFadeTime, true);
                }
                else
                {
                    handHiddenL.CrossFadeAlpha(150, Time.deltaTime, true);
                    eyeHL.CrossFadeAlpha(255, Time.deltaTime, true);
                    handHiddenL.CrossFadeAlpha(0, crossFadeTime, true);
                    eyeHL.CrossFadeAlpha(0, crossFadeTime, true);
                }
                break;
            case Enums.Hand.Right:
                if (b)
                {
                    handVisibleR.CrossFadeAlpha(150, Time.deltaTime, true);
                    eyeVR.CrossFadeAlpha(255, Time.deltaTime, true);
                    handVisibleR.CrossFadeAlpha(0, crossFadeTime, true);
                    eyeVR.CrossFadeAlpha(0, crossFadeTime, true);
                }
                else
                {
                    handHiddenR.CrossFadeAlpha(150, Time.deltaTime, true);
                    eyeHR.CrossFadeAlpha(255, Time.deltaTime, true);
                    handHiddenR.CrossFadeAlpha(0, crossFadeTime, true);
                    eyeHR.CrossFadeAlpha(0, crossFadeTime, true);
                }
                break;
        }
    }
}
