using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenController : MonoBehaviour {

    InputManager input;

    [SerializeField]
    GameObject restartButtonEnabled;

    [SerializeField]
    GameObject restartButtonDisabled;

    [SerializeField]
    GameObject exitButtonEnabled;

    [SerializeField]
    GameObject exitButtonDisabled;


    // Use this for initialization
    void Start () {
        input = GetComponent<InputManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {

        getInput();
	}

    void getInput()
    {
        if (input.GetLStick().y>0.0f)
        {      
            restartButtonEnabled.SetActive(false);
            restartButtonDisabled.SetActive(true);

            exitButtonEnabled.SetActive(true);
            exitButtonDisabled.SetActive(false);
        }

        if (input.GetLStick().y < 0.0f)
        {
            restartButtonEnabled.SetActive(true);
            restartButtonDisabled.SetActive(false);

            exitButtonEnabled.SetActive(false);
            exitButtonDisabled.SetActive(true);
        }

        if (input.GetButtonDown(InputManager.Buttons.A))
        {
            if (restartButtonEnabled.activeInHierarchy)
                SceneManager.LoadScene("Title");

           else
               Application.Quit();;
        }
    }
}
