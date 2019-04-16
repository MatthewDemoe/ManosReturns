using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    Text playText;

    [SerializeField]
    Text exitText;

    [SerializeField]
    int selectedSize = 150;

    [SerializeField]
    bool playSelected = true;

    [SerializeField]
    GameObject loadingText;

    [SerializeField]
    string nextScene;

    // Use this for initialization
    void Start ()
    {
        //anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        dt += Time.deltaTime;

        if (dt >= animationLength)
        {
            if (playing)
            {
                playing = false;
            }
        }           

        if (!playing)
        {
            enableButtons();
            getInput();
        }

        if (starting)
        {
            if (dt >= startDelay)
            {
                loadingText.gameObject.SetActive(true);

                SceneManager.LoadScene(nextScene);
            }
                
        }
	}

    void enableButtons()
    {
        if (!buttonsEnabled)
        {
            playText.gameObject.SetActive(true);
        
            exitText.gameObject.SetActive(true);
        
            buttonsEnabled = true;
        }
    }

    void getInput()
    {
        if (input.GetButtonDown(InputManager.Buttons.DPadUp) || input.GetButtonDown(InputManager.Buttons.DPadRight))
        {
            //playButtonEnabled.SetActive(true);
            //playButtonDisabled.SetActive(false);
            //
            //exitButtonEnabled.SetActive(false);
            //exitButtonDisabled.SetActive(true);

            playSelected = !playSelected;

            SwitchButtons();
        }

        else if (input.GetButtonDown(InputManager.Buttons.DPadDown) || input.GetButtonDown(InputManager.Buttons.DPadLeft))
        {
            //playButtonEnabled.SetActive(false);
            //playButtonDisabled.SetActive(true);
            //
            //exitButtonEnabled.SetActive(true);
            //exitButtonDisabled.SetActive(false);
            playSelected = !playSelected;

            SwitchButtons();
        }

        if (input.GetButtonDown(InputManager.Buttons.A))
        {

            if (playSelected)
            {
                anim.SetTrigger("PressedPlay");

                playText.gameObject.SetActive(false);
                exitText.gameObject.SetActive(false);

                playing = true;
                starting = true;
            
                dt = 0.0f;
            }

            else if (!starting)
            {
                Application.Quit();
            }
        }


    }

    void SwitchButtons()
    {
        if (playSelected)
        {
            playText.fontSize = selectedSize;
            exitText.fontSize = selectedSize / 2;
        }

        else
        {
            exitText.fontSize = selectedSize;
            playText.fontSize = selectedSize / 2;
        }
    }
}
