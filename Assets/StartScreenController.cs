using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenController : MonoBehaviour
{
    [SerializeField]
    InputManager input;

    [SerializeField]
    GameObject resumeButtonEnabled;

    [SerializeField]
    GameObject resumeButtonDisabled;

    [SerializeField]
    GameObject restartButtonEnabled;

    [SerializeField]
    GameObject restartButtonDisabled;

    [SerializeField]
    GameObject exitButtonEnabled;

    [SerializeField]
    GameObject exitButtonDisabled;

    [SerializeField]
    PressStart start;

    int index = 1;

    int numOfItem = 3;
    // Use this for initialization
    void Start()
    {
        ChangeSelection();
    }

    // Update is called once per frame
    void Update()
    {

        getInput();
    }

    void getInput()
    {

      
        if (input.GetButtonDown(InputManager.Buttons.DPadUp))
        {
            if (index > 1)
                index--;
            else if (index == 1)
                index = 3;

            ChangeSelection();
        }

        if (input.GetButtonDown(InputManager.Buttons.DPadDown))
        {
            if (index < numOfItem)
                index++;

            else if (index == numOfItem)
                index = 1;
            ChangeSelection();
        }

        if (input.GetButtonDown(InputManager.Buttons.A))
        {
            DoSelection();
        }
    }
   

    private void OnDisable()
    {
        index = 1;
        resumeButtonEnabled.SetActive(true);
        resumeButtonDisabled.SetActive(false);
        restartButtonEnabled.SetActive(false);
        restartButtonDisabled.SetActive(true);
        exitButtonEnabled.SetActive(false);
        exitButtonDisabled.SetActive(true);
    }
    void DoSelection()
    {
        switch (index)
        {
            case 1:
                start.Pause();
                break;
            case 2:
                SceneManager.LoadScene("Title");
                break;
            case 3:
                Application.Quit(); ;
                break;
        }
    }
    void ChangeSelection()
    {
        switch (index)
        {
            case 1:
                resumeButtonEnabled.SetActive(true);
                resumeButtonDisabled.SetActive(false);
                restartButtonEnabled.SetActive(false);
                restartButtonDisabled.SetActive(true);
                exitButtonEnabled.SetActive(false);
                exitButtonDisabled.SetActive(true);
                break;
            case 2:
                resumeButtonEnabled.SetActive(false);
                resumeButtonDisabled.SetActive(true);
                restartButtonEnabled.SetActive(true);
                restartButtonDisabled.SetActive(false);
                exitButtonEnabled.SetActive(false);
                exitButtonDisabled.SetActive(true);
                break;
            case 3:
                resumeButtonEnabled.SetActive(false);
                resumeButtonDisabled.SetActive(true);
                restartButtonEnabled.SetActive(false);
                restartButtonDisabled.SetActive(true);
                exitButtonEnabled.SetActive(true);
                exitButtonDisabled.SetActive(false);
                break;
        }
    }
}
