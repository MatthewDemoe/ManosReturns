using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VikingCrewTools.UI;

class QuipBattle
{
    public string opener;
    public string followUp;
}

public class ChadSpeakSpeechBubbles : MonoBehaviour
{
    [SerializeField]
    float speechBubbleStayTime = 3.0f;

    [SerializeField]
    string[] zingers;

    //[SerializeField]
    //QuipBattle zinger;

    //[SerializeField]
    //string message = "Hello!";

    bool _isSpeaking = false;

    int _lineIdx = 0;

    AudioManager _audioMgr;

    // Start is called before the first frame update
    void Start()
    {
        _audioMgr = AudioManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            //int lineIdx = Random.Range(0, zingers.Length);
            if(!_isSpeaking)
            {
                //Speak
                string toSpeak = zingers[_lineIdx];
                _lineIdx++;
                _lineIdx = _lineIdx % zingers.Length;

                StartCoroutine(SpeakLine(toSpeak, SpeechBubbleManager.SpeechbubbleType.NORMAL));
            }
        }
    }

    IEnumerator SpeakLine(string message, SpeechBubbleManager.SpeechbubbleType bubbleType)
    {
        _isSpeaking = true;

        string myCurrentText = message;
        SpeechBubbleBehaviour bubble = SpeechBubbleManager.Instance.AddSpeechBubble(transform, myCurrentText, bubbleType, speechBubbleStayTime);
        
        float typingDelay = 0.2f;
        float totalDuration = typingDelay * message.Length;
        //Debug.Log("word! total speaking duration: " + totalDuration);

        InvokeRepeating("RepeatSpeakingSound", 0.0f, 0.7f);

        //foreach (char letter in message.ToCharArray())
        //{
        //    //Debug.Log(letter);
        //   typingDelay = SpeechBubble2DFlyweight.Instance.letterTypingPauseNormal;
        //
        //   // respond to punctuation marks
        //   if (letter == ',') typingDelay = SpeechBubble2DFlyweight.Instance.letterTypingPauseComma;
        //   else if (letter == '.') typingDelay = SpeechBubble2DFlyweight.Instance.letterTypingPausePeriod;
        //   myCurrentText += letter;
        //
        //    bubble.UpdateText(myCurrentText, speechBubbleStayTime);
        //
        //    yield return new WaitForSeconds(typingDelay);
        //}

        CancelInvoke("RepeatSpeakingSound");

        yield return new WaitForSeconds(speechBubbleStayTime);
        _isSpeaking = false;
    }

    void RepeatSpeakingSound()
    {
        //Debug.Log("word!");
        //float pitchShift = Random.Range(-0.5f, 0.5f);
        _audioMgr.PlaySoundOnce(AudioManager.Sound.ChadVoice, transform, AudioManager.Priority.Default);
    }
    
}
