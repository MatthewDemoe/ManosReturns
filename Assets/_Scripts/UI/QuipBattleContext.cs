using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuipBattleContext : MonoBehaviour
{
    public enum QUIP_LOCATION
    {
        LEFT_FIRST,
        LEFT_SECOND,
        RIGHT_FIRST,
        RIGHT_SECOND
    }

    [SerializeField]
    public RectTransform quipTarget;

    // constant properties
    //[SerializeField]
    //public static float SPEECH_FRESH_TIME = 5.0f;
    [SerializeField]
    public static float SPEECH_FADE_TIME = 0.4f;
    [SerializeField]
    public static float SPEECH_OPACITY_FRESH = 1.0f;
    [SerializeField]
    public static float SPEECH_OPACITY_STALE = 0.4f;
    
    // MUST be 4
    [SerializeField]
    private SpeechBubble2D[] _speechBubbles = new SpeechBubble2D[4];

    // Use this for initialization
    void Start ()
    {
        quipTarget = GetComponentInChildren<RectTransform>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // quip battle BEGINS 
        // first quip entered, animates
        // second quip entered, animates, 
        // result of quip battle animation plays
        // Fresh quips stick for ~10 seconds and then fade to 50% opacity
        // NEXT quip battle BEGINS
        // Previous quip battle fades out
        // Entire current quip battle moves up
        // previous quip battle text becomes new bubbles
    }

    /// <summary>
    /// Displays quip with full opacity for some time, then fades it away
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public bool PlayFreshQuip(QUIP_LOCATION whichBubble, string quiptext, float damage = 0.1f, float time = 5.0f)
    {
        return _speechBubbles[(int)whichBubble].PlayFreshQuip(quiptext, damage, time);
    }

    public SpeechBubble2D GetSpeechBubble(Enums.Player player, DialogManager.QuipOrder quipOrComeback)
    {
        QUIP_LOCATION whichBubble = 0;

        if (player == Enums.Player.Manos)
        {
            switch (quipOrComeback)
            {
                case DialogManager.QuipOrder.FIRST:
                    whichBubble = QUIP_LOCATION.RIGHT_FIRST;
                    break;

                case DialogManager.QuipOrder.SECOND:
                    whichBubble = QUIP_LOCATION.RIGHT_SECOND;
                    break;
            }
        }
        else if (player == Enums.Player.Player1)
        {
            switch (quipOrComeback)
            {
                case DialogManager.QuipOrder.FIRST:
                    whichBubble = QUIP_LOCATION.LEFT_FIRST;
                    break;

                case DialogManager.QuipOrder.SECOND:
                    whichBubble = QUIP_LOCATION.LEFT_SECOND;
                    break;
            }
        }

        return _speechBubbles[(int)whichBubble];
    }

}
