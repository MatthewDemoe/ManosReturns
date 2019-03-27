using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DialogManager : MonoBehaviour
{
    public enum QuipBattleState { QUIP, COMEBACK, FOLLOWUP, NONE}
    public enum QuipOrder { FIRST, SECOND}

    string CurrentQuip;
    public QuipBattleState quipState;
    public QuipOrder quipOrderState;

    public Enums.Player currentPlayer;
    public Enums.Player whoStartedTheQuip;
    public float startingQuipTime;
    public float quipBaseTime;
    public float currentQuipTime;
    public StatusBarController egoUI;
    public float comebackBaseTime;
    public float currentComebackTime;
    [SerializeField]
    QuipBattleContext quipContext;

    public string[] currentQuipBattle;

    [SerializeField]
    string[] manosStarts;
    [SerializeField]
    string[] chadStarts;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = Enums.Player.None;
        whoStartedTheQuip = Enums.Player.None;
        currentQuipTime = startingQuipTime;
        // spawn = bubbleSpawnerObj.GetComponent<BubbleSpawner>();

    }

    // Update is called once per frame
    void Update()
    {
        if (IsSpeaking())
            CheckTimer();

        DecreaseTimers();
        if (IsSpeaking() == false && quipOrderState == QuipOrder.SECOND)
        {
            CheckComebackTimer();
        }
        //Debug.Log(state);
        /*  if (Input.GetKeyDown(KeyCode.C))
              Quip(Enums.Player.Player1);
          if (Input.GetKeyDown(KeyCode.M))
              Quip(Player.Manos);*/

    }

    void CheckTimer()
    {
        if (currentQuipTime <= 0)
        {
            Quiet();
        }
    }

    void DecreaseTimers()
    {
        currentQuipTime = Mathf.Max(currentQuipTime - Time.deltaTime, 0.0f);
    }
    
    void CheckComebackTimer()
    {
        if (currentComebackTime <= 0)
        {
            quipOrderState = QuipOrder.FIRST;
            quipState = QuipBattleState.QUIP;
            //add Quip Prompt here
        }

        currentComebackTime = Mathf.Max(currentComebackTime - Time.deltaTime, 0.0f);
    }
    public bool Quip(Enums.Player p, float basetime, float damage = 0.2f)
    {
        bool canSpeak = CanSpeak(currentPlayer) && quipContext.GetSpeechBubble(p, quipOrderState).IsCharged();
        if (canSpeak)
        {
            // if someone else other than the last speaker spoke, then it was a comeback
          
            QuipOrder nextState;

            currentQuipTime = basetime;
            currentPlayer = p;

            // swap state
            if (quipOrderState == QuipOrder.FIRST)
            {
                whoStartedTheQuip = p;

                currentQuipTime = basetime;
                switch (currentPlayer)
                {
                    case Enums.Player.Player1:
                        {
                            currentQuipBattle = chadStarts;
                            break;
                        }
                    case Enums.Player.Manos:
                        {
                            currentQuipBattle = manosStarts;
                            break;
                        }
                }
                nextState = QuipOrder.SECOND;
                quipState = QuipBattleState.QUIP;

                currentComebackTime = comebackBaseTime;
            } else
            {
                if (whoStartedTheQuip != p)
                {
                    quipState = QuipBattleState.COMEBACK;
                } else
                {
                    quipState = QuipBattleState.FOLLOWUP;
                }
                nextState = QuipOrder.FIRST;
            }

            //select line to say
            CurrentQuip = currentQuipBattle[(int)quipState];


            AttemptSpawnQuip(damage);

            quipOrderState = nextState;
        }
        return canSpeak;

    }
    void Quiet()
    {
        //Debug.Log("Timer up");
        //add quip or comeback promt here depending on state
        currentPlayer = Enums.Player.None;
        whoStartedTheQuip = Enums.Player.None;
        RemoveQuip();//players can now speak
    }
    bool AttemptSpawnQuip(float damage)
    {
        //Debug.Log(CurrentQuip);
        //Debug.Log(state);
        //Debug.Log(GetSpeaker());

        //
        //QuipBattleContext.FIRST_LOCATION
        QuipBattleContext.QUIP_LOCATION whichQuip;
        whichQuip = 0;

        if (currentPlayer == Enums.Player.Manos)
        {
            damage *= -1.0f;
            switch (quipOrderState)
            {
                case QuipOrder.FIRST:
                    whichQuip = QuipBattleContext.QUIP_LOCATION.RIGHT_FIRST;
                    break;

                case QuipOrder.SECOND:
                    whichQuip = QuipBattleContext.QUIP_LOCATION.RIGHT_SECOND;
                    break;
            }
        }
        else if (currentPlayer == Enums.Player.Player1)
        {
            switch (quipOrderState)
            {
                case QuipOrder.FIRST:
                    whichQuip = QuipBattleContext.QUIP_LOCATION.LEFT_FIRST;
                    break;

                case QuipOrder.SECOND:
                    whichQuip = QuipBattleContext.QUIP_LOCATION.LEFT_SECOND;
                    break;
            }
        }

        bool success = quipContext.PlayFreshQuip(whichQuip, CurrentQuip, damage);
        return success;
    }
    void RemoveQuip()
    {
        //Debug.Log("ready to speak now");
    }

    public QuipOrder GetquipOrderState()
    {
        return quipOrderState;
    }

    bool CanSpeak(Enums.Player n)
    {
        if (n == Enums.Player.None && currentQuipTime <= 0)
            return true;
        else
            return false;
    }
    public Enums.Player GetSpeaker() { return currentPlayer; }
    public bool IsSpeaking()
    {
        if (currentPlayer != Enums.Player.None)
            return true;
        else
            return false;
    }
    public bool ManosSpeak()
    {
        if (currentPlayer == Enums.Player.Manos)
            return true;
        else
            return false;
    }
    public bool ChadSpeak()
    {
        if (currentPlayer == Enums.Player.Player1)
            return true;
        else
            return false;
    }
}
