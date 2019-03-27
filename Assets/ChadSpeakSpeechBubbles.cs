using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VikingCrewTools.UI;

public class ChadSpeakSpeechBubbles : MonoBehaviour
{
    [SerializeField]
    string message = "Hello!";


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("speaking!");

            StartCoroutine(SpeakLine(message, SpeechBubbleManager.SpeechbubbleType.NORMAL));
        }
    }

    IEnumerator SpeakLine(string message, SpeechBubbleManager.SpeechbubbleType bubbleType)
    {
        string myCurrentText = "";
        SpeechBubbleBehaviour bubble = SpeechBubbleManager.Instance.AddSpeechBubble(transform, myCurrentText, bubbleType, 3.0f);
        
        float typingDelay = 0.1f;
        foreach (char letter in message.ToCharArray())
        {
            //Debug.Log(letter);
            typingDelay = 0.005f; //SpeechBubble2DFlyweight.Instance.letterTypingPauseNormal;
        
           // respond to punctuation marks
           if (letter == ',') typingDelay = SpeechBubble2DFlyweight.Instance.letterTypingPauseComma;
           else if (letter == '.') typingDelay = SpeechBubble2DFlyweight.Instance.letterTypingPausePeriod;
           myCurrentText += letter;

            bubble.UpdateText(myCurrentText, 3.0f);

            yield return new WaitForSeconds(typingDelay);
        }
    }
    
}
