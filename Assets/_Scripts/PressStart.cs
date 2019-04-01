using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressStart : MonoBehaviour
{
    [SerializeField]
    InputManager input;
    [SerializeField]
    GameObject playerUI;

    [SerializeField]
    GameObject startUI;

    bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (input.GetButtonDown(InputManager.Buttons.Start))
        {
            paused = !paused;
            startUI.SetActive(paused);
            playerUI.SetActive(!paused);
        }


    }
}
