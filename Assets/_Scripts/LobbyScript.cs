using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class LobbyScript : MonoBehaviour
{
    [SerializeField]
    bool chadReady = false;

    [SerializeField]
    bool manosReady = false;

    [SerializeField]
    float timetoReady = 2.0f;

    [SerializeField]
    float fadeTime = 1.0f;

    [SerializeField]
    List<GameObject> islands;

    [SerializeField]
    GameObject readyButton;

    FadeOut fadeOut;

    float chadReadyTimer = 0.0f;
    float manosReadyTimer = 0.0f;

    InputManager inMan;

    GameObject chad;
    GameObject tp;

    Text tt;
    Text skipText;
    Text countdown;
    Text manos_countdown;

    GameObject tutorialTracker;
    GameObject waypoint;

    [SerializeField]
    int currentIsland = 0;

    //[SerializeField]
    //SteamVR_Action_Boolean readyAction;

    [SerializeField]
    GameObject SkipButtonOne;

    [SerializeField]
    GameObject SkipButtonTwo;

    [SerializeField]
    GameObject ButtonsOne;

    [SerializeField]
    GameObject ButtonsTwo;

    [SerializeField]
    GameObject ButtonsThree;

    [SerializeField]
    GameObject ButtonsFour;

    [SerializeField]
    GameObject TutorialB;

    [SerializeField]
    GameObject trainingEnvironment;

    [SerializeField]
    GameObject[] manosTrainingRemotes;

    [SerializeField]
    GameObject HPUI;

    void Start()
    {

        SkipButtonOne.SetActive(true);
        SkipButtonTwo.SetActive(false);
        ButtonsOne.SetActive(true);
        ButtonsTwo.SetActive(false);
        ButtonsThree.SetActive(false);
        ButtonsFour.SetActive(false);
        inMan = GetComponent<InputManager>();
        chad = GameObject.Find("Chad");
        tp = GameObject.Find("TeleportPoint");
        tt = GameObject.Find("TutorialText").GetComponent<Text>() ;
        skipText = GameObject.Find("SkipText").GetComponent<Text>();

        countdown = GameObject.Find("Timer").GetComponent<Text>();
        manos_countdown = GameObject.Find("Manos_CountDown").GetComponent<Text>();
        manos_countdown.enabled = false;
        countdown.enabled = false;

        fadeOut = GetComponent<FadeOut>();
        fadeTime = fadeOut.GetFadeTime();

        tutorialTracker = GameObject.Find("TutorialTracker");
        waypoint = GameObject.Find("TutorialWaypoint");

        tt.text = "Press         or         to <color=red>jump</color>";
        skipText.text = "Hold         to <color=red>skip</color> tutorial";

        // disables HP UI when players are training
        HPUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        if (chadReadyTimer >= timetoReady)
        {
            chadReady = true;
            skipText.text = "Ready... Waiting for <color=red>Manos</color>";
            SkipButtonOne.SetActive(false);
            SkipButtonTwo.SetActive(false);
        }

        if (manosReadyTimer >= timetoReady)
        {
            manosReady = true;
            manos_countdown.enabled = true;
            GameObject.Find("Manos 2.3").GetComponentInChildren<ManosHand>().SetTraining(false);
        }

        if (chadReady && manosReady)
        {
            PlayersReady();
        }
    }

    public void SetManosReady()
    {
        manosReady = true;
        manos_countdown.enabled = true;
        GameObject.Find("Manos 2.3").GetComponentInChildren<ManosHand>().SetTraining(false);
    }

    void GetInput()
    {
        if (inMan.GetButton(InputManager.Buttons.X))
        {
            chadReadyTimer += Time.deltaTime;
            
        }

        if (inMan.GetButtonUp(InputManager.Buttons.X))
        {
            chadReadyTimer = 0.0f;
        }

        //if (readyAction.GetState(SteamVR_Input_Sources.Any) )
        //{
        //    manosReadyTimer += Time.deltaTime;
        //}
        //else if (readyAction.GetStateUp(SteamVR_Input_Sources.Any))
        //{
        //    manosReadyTimer = 0;
        //}
    }

    public IEnumerator JonWaalerTeleportMP3()
    {
        fadeOut.BeginFadeOut();

        yield return new WaitForSeconds(fadeTime);

        GameObject.Find("ManosControls").SetActive(false);

        readyButton.SetActive(false);

        chad.GetComponent<CharacterController>().enabled = false;
        chad.transform.position = tp.transform.position;
        chad.GetComponent<CharacterController>().enabled = true;

        //GameObject.Find("FakeChad").SetActive(false);

        HPUI.SetActive(true);

        StartCoroutine("BeginCountdown");

    }

    IEnumerator BeginCountdown()
    {
        string tx;
        tx = "Get ready... \n\n3";
        manos_countdown.enabled = true;
        countdown.enabled = true;

        manos_countdown.text = tx;

        yield return new WaitForSeconds(1.0f);
        tx = "Get ready... \n\n2";
        countdown.text = tx;
        manos_countdown.text = tx;

        yield return new WaitForSeconds(1.0f);
        tx = "Get ready... \n\n1";
        countdown.text = tx;
        manos_countdown.text = tx;

        //Destroy(trainingEnvironment);

        AudioManager.GetInstance().PlayMusic(AudioManager.Music.Skeletons);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");

        //yield return new WaitForSeconds(1.0f);

        //countdown.enabled = false;

        //manos_countdown.enabled = false;

        //GetComponent<FadeOut>().BeginFadeIn();

        //GetComponent<FootballFactory>().BeginSpawning();

        //enabled = false;
    }

    IEnumerator BeginTransition()
    {
        yield return new WaitForSeconds(0.0f);
    }

    public void ReachedWaypoint()
    {

        if (currentIsland < islands.Count)
        {
            currentIsland++;

            waypoint.transform.position = islands[currentIsland].transform.position + new Vector3(0.0f, 5.0f, 0.0f);
        }

        switch (currentIsland)
        {
            case 1:
                tt.text = "Press         or         in the air to <color=red>double jump</color>";
                skipText.text = (chadReady) ? "Ready... Waiting for <color=red>Manos</color>" : "Hold         to <color=red>skip</color> tutorial";
                break;

            case 2:
                tt.text = "Hold         down to charge a <color=red>stronger jump</color>";
                ButtonsOne.SetActive(false);
                ButtonsTwo.SetActive(true);
                break;

            case 3:
                tt.text = "Hold         on the ground or in the air to charge a <color=red>dash attack.</color>\n\nTry chaining it with your jumps!\nCharging longer increases <color=red>distance</color> and <color=red>damage.</color>";
                ButtonsTwo.SetActive(false);
                ButtonsThree.SetActive(true);
                break;

            case 4:
                tt.text = "You can use your dash attack to <color=red>kick Manos.</color> \nGive it a try on the target";
                ButtonsThree.SetActive(false);
                break;

            case 5:
                tt.text = "Hold         to start throwing an <color=red>exploding football. </color> \n\nHold the button to throw it farther. \nYou have a limited amount, but more are available around the level.";
                ButtonsThree.SetActive(false);
                ButtonsFour.SetActive(true);
                break;

            case 6:
                tt.text = "Press         while moving to <color=red>dab away from danger. </color>\n\nYou have a short period of <color=red>invincibility</color> while dabbing";
                skipText.text = (chadReady) ? "You have nothing more to learn. Ready... Waiting for <color=red>Manos</color>" : "You have nothing more to learn. Hold         to <color=red>ready up.</color>";
                ButtonsFour.SetActive(false);
                TutorialB.SetActive(true);

                if(chadReady)
                {
                    SkipButtonOne.SetActive(false);
                    SkipButtonTwo.SetActive(false);
                }
                else
                {
                    SkipButtonOne.SetActive(false);
                    SkipButtonTwo.SetActive(true);
                }
                
                Destroy(waypoint);
                break;
        }

        tutorialTracker.transform.position = chad.transform.position;
    }

    public void ResetPosition()
    {
        chad.GetComponent<CharacterController>().enabled = false;

        chad.transform.position = tutorialTracker.transform.position;

        chad.GetComponent<CharacterController>().enabled = true;
    }

    public void PlayersReady()
    {
        Debug.Log("Memes");

        chadReady = false;
        manosReady = false;
        chadReadyTimer = 0.0f;
        StartCoroutine("JonWaalerTeleportMP3");
        tt.gameObject.SetActive(false);
        GameObject.Find("SkipText").SetActive(false);
        SkipButtonOne.SetActive(false);
        SkipButtonTwo.SetActive(false);
        ButtonsOne.SetActive(false);
        ButtonsTwo.SetActive(false);
        ButtonsThree.SetActive(false);
        ButtonsFour.SetActive(false);

        chad.GetComponentInChildren<ThrowTrigger>().PickUpFootball(10);

        foreach(GameObject g in manosTrainingRemotes)
        {
            g.SetActive(false);
        }
    }
}
