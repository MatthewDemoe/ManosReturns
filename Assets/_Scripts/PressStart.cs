using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressStart : MonoBehaviour
{
    [SerializeField]
    InputManager input;
    [SerializeField]
    GameObject startUI;

    bool paused = false;

    void Update()
    {
        if (input.GetButtonDown(InputManager.Buttons.Start))
        {
            Pause();
        }
    }


   public void Pause()
    {
        paused = !paused;
        startUI.SetActive(paused);
    }
}
