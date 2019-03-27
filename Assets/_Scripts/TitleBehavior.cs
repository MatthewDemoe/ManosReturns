using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleBehavior : MonoBehaviour {


    Animator anim;
    
    bool playing = true;
    bool buttonsEnabled = false;

    bool starting = false;

    float animationLength = 4.2f;
    float startDelay = 1.0f;
    float dt = 0.0f;

    [SerializeField]
    GameObject playButtonEnabled;

    [SerializeField]
    GameObject playButtonDisabled;

    [SerializeField]
    GameObject exitButtonEnabled;

    [SerializeField]
    GameObject exitButtonDisabled;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        dt += Time.deltaTime;

        if (dt >= animationLength) {
        playing = false;
        }
           

        if (!playing)
        {
            enableButtons();
            getInput();
        }

        if (starting)
        {
            if (dt >= startDelay) {
                SceneManager.LoadScene("ItsJustWorkingVr");
            }
                
        }

        getInput();
	}

    void enableButtons()
    {
        if (!buttonsEnabled)
        {
            playButtonEnabled.SetActive(true);

            exitButtonDisabled.SetActive(true);

            buttonsEnabled = true;
        }
    }

    void getInput()
    {
        if ((Input.GetAxis("Vertical") > 0.1f) || (Input.GetAxis("Horizontal") < -0.1f))
        {
            
            playButtonEnabled.SetActive(false);
            playButtonDisabled.SetActive(true);

            exitButtonEnabled.SetActive(true);
            exitButtonDisabled.SetActive(false);


        }

        if ((Input.GetAxis("Vertical") < -0.1f) || (Input.GetAxis("Horizontal") > 0.1f))
        {
            playButtonEnabled.SetActive(true);
            playButtonDisabled.SetActive(false);

            exitButtonEnabled.SetActive(false);
            exitButtonDisabled.SetActive(true);
        }

        if (Input.GetButton("Fire1"))
        {
            if (playButtonEnabled.activeInHierarchy)
            {
                anim.SetBool("PressedPlay", true);

                playButtonEnabled.SetActive(false);
                playButtonDisabled.SetActive(false);
                exitButtonEnabled.SetActive(false);
                exitButtonDisabled.SetActive(false);

                playing = true;
                starting = true;

                dt = 0.0f;
                //SceneManager.LoadScene("NetworkingVR");
            }

            else
            {
                Application.Quit();
            }
        }
    }
}
