using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using TMPro;

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
    List<Transform> wayPoints;

    [SerializeField]
    GameObject readyButton;

    FadeOut fadeOut;

    float chadReadyTimer = 0.0f;
    float manosReadyTimer = 0.0f;

    InputManager inMan;

    GameObject chad;
    GameObject tp;

    TextMeshPro tt;
    Text skipText;
    Text countdown;
    //Text manos_countdown;

    GameObject tutorialTracker;
    GameObject waypoint;

    //[SerializeField]
    int currentWayPoint = 0;

    //[SerializeField]
    //SteamVR_Action_Boolean readyAction;

    [SerializeField]
    GameObject SkipButtonOne;

    [SerializeField]
    GameObject SkipButtonTwo;

  /*  [SerializeField]
    GameObject ButtonsOne;

    [SerializeField]
    GameObject ButtonsTwo;

    [SerializeField]
    GameObject ButtonsThree;

    [SerializeField]
    GameObject ButtonsFour;

    [SerializeField]
    GameObject FootballButton;*/

    [SerializeField]
    GameObject trainingEnvironment;

    [SerializeField]
    GameObject[] manosTrainingRemotes;

    [SerializeField]
    GameObject ManosHPBar;

    // portal opens when one player is ready
    [SerializeField]
    GameObject portal;

    [SerializeField]
    string nextScene = "Post_Title";


    void Start()
    {

        SkipButtonOne.SetActive(true);
        SkipButtonTwo.SetActive(false);
       /* ButtonsOne.SetActive(true);
        ButtonsTwo.SetActive(false);
        ButtonsThree.SetActive(false);
        ButtonsFour.SetActive(false);*/
        inMan = GetComponent<InputManager>();
        chad = GameObject.Find("Chad");
        tp = GameObject.Find("TeleportPoint");
        tt = GameObject.Find("TutorialText").GetComponent<TextMeshPro>() ;
        skipText = GameObject.Find("SkipText").GetComponent<Text>();

        countdown = GameObject.Find("Timer").GetComponent<Text>();

        //manos_countdown = GameObject.Find("Manos_CountDown").GetComponent<Text>();
        //manos_countdown.enabled = false;
        countdown.enabled = false;

        fadeOut = GetComponent<FadeOut>();
        fadeTime = fadeOut.GetFadeTime();

        tutorialTracker = GameObject.Find("TutorialTracker");
        waypoint = GameObject.Find("TutorialWaypoint");

        tt.text = "Press <sprite name=\"A_Button\"> to <color=red>jump</color>";
        skipText.text = "Hold         to <color=red>skip</color> tutorial";

        // disables HP UI for Chad when players are training
        ManosHPBar.SetActive(false);

       //if(chad.GetComponent<PlayerHealth>() != null)
       //{
       //    // start chad off with less HP
       //    chad.GetComponent<PlayerHealth>().TakeDamage(300);
       //}
        
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
            //manos_countdown.enabled = true;
            GameObject.Find("Manos 2.3").GetComponentInChildren<ManosHand>().SetTraining(false);
        }
        
        portal.SetActive(manosReady);

        if (chadReady && manosReady)
        {
            PlayersReady();
        }

        
    }

    public void SetManosReady()
    {
        manosReady = true;
        //manos_countdown.enabled = true;
        GameObject.Find("Manos 2.3").GetComponentInChildren<ManosHand>().SetTraining(false);
    }

    void GetInput()
    {
        if (inMan.GetButton(InputManager.Buttons.X))
        {
            chadReadyTimer += Time.deltaTime;
            Color pressedColor = new Color(0.8f, 0.8f, 0.8f);

            SkipButtonOne.GetComponent<Image>().color = pressedColor;
            SkipButtonTwo.GetComponent<Image>().color = pressedColor;
        } else
        {
            SkipButtonOne.GetComponent<Image>().color = Color.white;
            SkipButtonTwo.GetComponent<Image>().color = Color.white;
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

        yield return new WaitForEndOfFrame();

        if (GameObject.Find("ManosControls") != null)
        {
            GameObject.Find("ManosControls").SetActive(false);
        }

        readyButton.SetActive(false);

        //chad.GetComponent<CharacterController>().enabled = false;
        //chad.transform.position = tp.transform.position;
        //chad.GetComponent<CharacterController>().enabled = true;

        //GameObject.Find("FakeChad").SetActive(false);

        ManosHPBar.SetActive(true);

        StartCoroutine("BeginCountdown");

    }

    IEnumerator BeginCountdown()
    {
        fadeOut.BeginFadeOut();

        string tx;
        tx = "Get ready... \n\n3";
        //manos_countdown.enabled = true;
        countdown.enabled = true;

        //manos_countdown.text = tx;

        yield return new WaitForSeconds(1.0f);
        tx = "Get ready... \n\n2";
        countdown.text = tx;
        //manos_countdown.text = tx;

        yield return new WaitForSeconds(1.0f);
        tx = "Get ready... \n\n1";
        countdown.text = tx;
        //manos_countdown.text = tx;

        yield return new WaitForSeconds(1.0f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
    }

    IEnumerator BeginTransition()
    {
        yield return new WaitForSeconds(0.0f);
    }

    public void ReachedWaypoint()
    {
        if (currentWayPoint >= 0)
        {
            wayPoints[currentWayPoint].gameObject.SetActive(false);
            tutorialTracker.transform.position = wayPoints[currentWayPoint].position;
        }

        if (currentWayPoint < wayPoints.Count)
        {
            //Debug.Log("Way Point: " + currentWayPoint);

            switch (currentWayPoint)
            {
                case 0:
                    tt.text = "Press <sprite name=\"A_Button\"> in the air to <color=red>double jump</color>";
                    skipText.text = (chadReady) ? "Ready... Waiting for <color=red>Manos</color>" : "Hold         to <color=red>skip</color> tutorial";
                    break;

                case 1:
                    tt.text = "Hold <sprite name=\"A_Button\"> down to charge a <color=red>SUPERJUMP!</color>";
                 /*   ButtonsOne.SetActive(false);
                    ButtonsTwo.SetActive(true);*/
                    break;

                case 2:
                    tt.text = "Press <sprite name=\"RT_Button\"> to use your <color=red>SUPERKICK!</color>\n\n Holding down the trigger increases <color=red>distance</color> and <color=red>damage.</color>\n\nYou can even charge in the air!";
                  /*  ButtonsTwo.SetActive(false);
                    ButtonsThree.SetActive(true);*/
                    break;

                case 3:
                    tt.text = "Click <sprite name=\"Right_Stick\"> to <color=red>Lock onto targets</color>\n\nTap the stick to the left or right to swap targets.";
                  //  ButtonsThree.SetActive(false);
                    break;

                case 4:
                    tt.text = "Press <sprite name=\"B_Button\"> while moving to <color=red>DAB away from danger.</color>\n\nYou can combine this with your <color=red>SUPERJUMP</color>\n\n and <color=red>SUPERKICK</color> to traverse long distances!";
                  //  ButtonsThree.SetActive(false);
                    //ButtonsFour.SetActive(true);
                    break;

                case 5:
                    tt.text = "Press <sprite name=\"LT_Button\"> to throw a <color=red>SUPERBOWL SPECIAL.</color> \n\nHold down the trigger to throw farther. ";
                    //skipText.text = (chadReady) ? "Ready... Waiting for <color=red>Manos</color>" : "You have nothing more to learn. Hold         to <color=red>ready up.</color>";
                 //   FootballButton.SetActive(true);
                   // ButtonsFour.SetActive(false);
                    break;

                case 6:
                    tt.text = "";
                    skipText.text =  "You have nothing more to learn. Hop into the  <color=purple>portal</color> when you are ready!.";
                    //   FootballButton.SetActive(false);
                    //  chadReady = true;
                   // manosReady = true;

                  /*  if (chadReady)
                    {
                        SkipButtonOne.SetActive(false);
                        SkipButtonTwo.SetActive(false);
                    }
                    else
                    {
                        SkipButtonOne.SetActive(false);
                        SkipButtonTwo.SetActive(true);
                    }*/
                    break;
            }


            // next waypoint
            currentWayPoint++;

            // end of waypoints?
            if(currentWayPoint < wayPoints.Count)
            {
                var newWayPoint = wayPoints[currentWayPoint];
                waypoint.transform.position = newWayPoint.position;
            } else
            {
                Destroy(waypoint);
            }
        }
    }

    public void ResetPosition()
    {
        chad.GetComponent<CharacterController>().enabled = false;

        chad.transform.position = tutorialTracker.transform.position;

        chad.GetComponent<CharacterController>().enabled = true;
    }

    public void PlayersReady()
    {
        chadReady = false;
        manosReady = false;
        chadReadyTimer = 0.0f;
        StartCoroutine("JonWaalerTeleportMP3");
        tt.gameObject.SetActive(false);
        GameObject.Find("SkipText").SetActive(false);
        SkipButtonOne.SetActive(false);
        SkipButtonTwo.SetActive(false);
      /*  ButtonsOne.SetActive(false);
        ButtonsTwo.SetActive(false);
        ButtonsThree.SetActive(false);
        ButtonsFour.SetActive(false);*/

        chad.GetComponentInChildren<ThrowTrigger>().PickUpFootball(10);

        foreach(GameObject g in manosTrainingRemotes)
        {
            g.SetActive(false);
        }
    }
}
