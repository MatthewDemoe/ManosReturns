using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleBehavior : MonoBehaviour {

    [SerializeField]
    InputManager input;

    [SerializeField]
    Animator anim;
    
    bool playing = true;
    bool buttonsEnabled = false;

    bool starting = false;

    [SerializeField]
    float animationLength = 0.3f;

    float startDelay = 1.25f;
    float dt = 0.0f;

    [SerializeField]
    GameObject playButtonEnabled;

    [SerializeField]
    GameObject playButtonDisabled;

    [SerializeField]
    GameObject exitButtonEnabled;

    [SerializeField]
    GameObject exitButtonDisabled;

    [SerializeField]
    GameObject loadingText;

    // Use this for initialization
    void Start ()
    {
        //anim = GetComponent<Animator>();
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
            loadingText.SetActive(true);

            if (dt >= startDelay) {
                SceneManager.LoadScene("Main");
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
        if ((input.GetLStick().y > 0.01f) || (input.GetLStick().x < -0.1f))
        {
            playButtonEnabled.SetActive(true);
            playButtonDisabled.SetActive(false);

            exitButtonEnabled.SetActive(false);
            exitButtonDisabled.SetActive(true);
        }

        if ((input.GetLStick().y < -0.01f) || (input.GetLStick().x > 0.1f))
        {
            playButtonEnabled.SetActive(false);
            playButtonDisabled.SetActive(true);

            exitButtonEnabled.SetActive(true);
            exitButtonDisabled.SetActive(false);
        }

        if (input.GetButtonDown(InputManager.Buttons.A))
        {
            if (playButtonEnabled.activeInHierarchy)
            {
                anim.SetTrigger("PressedPlay");

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
