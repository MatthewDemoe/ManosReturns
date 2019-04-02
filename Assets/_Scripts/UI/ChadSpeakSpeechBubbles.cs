using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VikingCrewTools.UI;

public class ChadSpeakSpeechBubbles : MonoBehaviour
{
    static ChadSpeakSpeechBubbles _instance = null;

    [SerializeField]
    float speechBubbleStayTime = 3.0f;

    [SerializeField]
    string[] randomComments;

    /// <summary>
    /// When chad has no footballs
    /// </summary>
    [SerializeField]
    [Tooltip("When chad has no footballs")]
    string[] noFootballs;

    [SerializeField]
    [Tooltip("When chad takes damage")]
    string[] tookDamage;

    [SerializeField]
    [Tooltip("When chad lands a kick")]
    string[] dealtDamage;

    [SerializeField]
    [Tooltip("When chad Dabs")]
    string[] onDab;

    [SerializeField]
    float chanceOfDabLine = 0.5f;

    SpeechBubbleBehaviour feedbackBubble;

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

    public static ChadSpeakSpeechBubbles Instance()
    {
        return _instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        _audioMgr = AudioManager.GetInstance();
        _randomCommentTimer = 1.0f;

        if(!_instance)
        {
            _instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(randomCommentsEnabled)
        {
            _randomCommentTimer -= Time.deltaTime;
            if(_randomCommentTimer < 0.0f)
            {
                AttemptSpeakRandom();
            }
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            //int lineIdx = Random.Range(0, zingers.Length);
            AttemptSpeakRandom();
        }
    }

    void AttemptSpeakRandom()
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

    void SpeakFeedback(string toSpeak)
    {
        StartCoroutine(SpeakLine(toSpeak, SpeechBubbleManager.SpeechbubbleType.NORMAL));
    }

    IEnumerator SpeakLine(string message, SpeechBubbleManager.SpeechbubbleType bubbleType)
    {
        if(feedbackBubble)
        {
            feedbackBubble.Clear();
        }

        _isSpeaking = true;

        string myCurrentText = message;
        feedbackBubble = SpeechBubbleManager.Instance.AddSpeechBubble(transform, myCurrentText, bubbleType, speechBubbleStayTime);
        
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
        feedbackBubble = null;
    }

    void RepeatSpeakingSound()
    {
        //Debug.Log("word!");
        //float pitchShift = Random.Range(-0.5f, 0.5f);
        _audioMgr.PlaySoundOnce(AudioManager.Sound.ChadVoice, transform, AudioManager.Priority.Default);
    }


    public void OnDab()
    {
        //if(Random.value <= chanceOfDabLine)
        //{
            int randLine = Random.Range(0, onDab.Length);
            StartCoroutine(SpeakLine(onDab[randLine], SpeechBubbleManager.SpeechbubbleType.NORMAL));
        //}
    }

    //void OnDab()
    //{
    //    int randLine = Random.Range(0, t.Length);
    //    StartCoroutine(SpeakLine(onDab[randLine], SpeechBubbleManager.SpeechbubbleType.NORMAL));
    //}
}
