using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VikingCrewTools.UI;

public class ChadSpeakSpeechBubbles : MonoBehaviour
{
    ChadSpeakSpeechBubbles Instance;

    [SerializeField]
    float speechBubbleStayTime = 3.0f;

    [SerializeField]
    string[] randomComments;

    //[SerializeField]
    //QuipBattle zinger;

    //[SerializeField]
    //string message = "Hello!";

    [SerializeField]
    Vector2 timeBetweenRandomComments = new Vector2(10.0f, 15.0f);

    // time until next random comment
    float _randomCommentTimer;

    public bool randomCommentsEnabled = true;

    bool _isSpeaking = false;

    int _lineIdx = 0;

    AudioManager _audioMgr;

    // Start is called before the first frame update
    void Start()
    {
        _audioMgr = AudioManager.GetInstance();
        _randomCommentTimer = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(randomCommentsEnabled)
        {
            _randomCommentTimer -= Time.deltaTime;
            if(_randomCommentTimer < 0.0f)
            {
                AttemptSpeak();
            }
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            //int lineIdx = Random.Range(0, zingers.Length);
            AttemptSpeak();
        }
    }

    void AttemptSpeak()
    {
        if (!_isSpeaking)
        {
            //next line
            if(_lineIdx < randomComments.Length)
            {
                string toSpeak = randomComments[_lineIdx];
                _lineIdx++;
                //_lineIdx = _lineIdx % randomComments.Length;

                StartCoroutine(SpeakLine(toSpeak, SpeechBubbleManager.SpeechbubbleType.NORMAL));
                _randomCommentTimer = Random.Range(timeBetweenRandomComments.x, timeBetweenRandomComments.y);
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
