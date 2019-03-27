using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton class containing parameters for SpeechBubble2D script
/// </summary>
public class SpeechBubble2DFlyweight : MonoBehaviour
{
    [SerializeField]
    public RectTransform quipTarget;

    [SerializeField]
    public Vector3 quipTargetFixed;

    [SerializeField]
    public StatusBarController egoBar;

    [Header("Typing pause times")]
    [SerializeField]
    public float letterTypingPauseNormal = 0.005f;
    [SerializeField]
    public float letterTypingPauseComma = 0.15f;
    [SerializeField]
    public float letterTypingPausePeriod = 0.2f;
    [SerializeField]
    public float readyFlashFrequency = 2.0f;
    [SerializeField]
    public float promptedFlashFrequency = 8.0f;

    //
    static SpeechBubble2DFlyweight mInstance;
    public static SpeechBubble2DFlyweight Instance
    {
        get
        {
            return mInstance;
        }
    }

    void Awake()
    {
        SpeechBubble2DFlyweight.mInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
