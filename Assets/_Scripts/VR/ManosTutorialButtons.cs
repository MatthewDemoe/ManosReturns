using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

enum Hand
{
    Left,
    Right
}

enum Control
{
    Base,
    Body,
    Enter_Button,
    Grip,
    Handgrip,
    Thumbstick,
    Tip,
    Trigger,
    A_Button,
    B_Button,
    Control_Num
}

public class ManosTutorialButtons : MonoBehaviour
{
    [SerializeField]
    SteamVR_Behaviour_Skeleton leftSkelly;

    [SerializeField]
    SteamVR_Behaviour_Skeleton rightSkelly;

    [SerializeField]
    Transform leftControllerModel;

    [SerializeField]
    Transform rightControllerModel;

    [Header("Controls")]
    [SerializeField]
    MeshRenderer[] grip;

    [SerializeField]
    MeshRenderer[] trigger;

    [SerializeField]
    MeshRenderer[] buttonA;

    [SerializeField]
    MeshRenderer[] buttonB;

    [SerializeField]
    Material defaultMat;

    [SerializeField]
    Material activeMat;

    [SerializeField]
    SteamVR_Action_Boolean fistAction;

    [SerializeField]
    SteamVR_Action_Boolean gunAction;

    [SerializeField]
    SteamVR_Action_Boolean triggerAction;

    [SerializeField]
    SteamVR_Action_Boolean tempHolder;

    // Start is called before the first frame update
    void Start()
    {
        tempHolder = fistAction;

        fistAction = null;

        leftControllerModel.gameObject.SetActive(true);
        rightControllerModel.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(fistAction != null)
        {
            if (fistAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                print("Left Fist Stating!");
            }
        }
        

        if(leftControllerModel.GetComponentsInChildren<MeshRenderer>().Length != 0)
        {
            if(!grip[(int)Hand.Left])
            {
                // get Mesh Renderers for Left Controller Controls
                grip[(int)Hand.Left] = leftControllerModel.GetChild((int)Control.Grip).GetComponent<MeshRenderer>();
                trigger[(int)Hand.Left] = leftControllerModel.GetChild((int)Control.Trigger).GetComponent<MeshRenderer>();
                buttonA[(int)Hand.Left] = leftControllerModel.GetChild((int)Control.A_Button).GetComponent<MeshRenderer>();
                buttonB[(int)Hand.Left] = leftControllerModel.GetChild((int)Control.B_Button).GetComponent<MeshRenderer>();

                // get Mesh Renderers for Right Controller Controls
                grip[(int)Hand.Right] = rightControllerModel.GetChild((int)Control.Grip).GetComponent<MeshRenderer>();
                trigger[(int)Hand.Right] = rightControllerModel.GetChild((int)Control.Trigger).GetComponent<MeshRenderer>();
                buttonA[(int)Hand.Right] = rightControllerModel.GetChild((int)Control.A_Button).GetComponent<MeshRenderer>();
                buttonB[(int)Hand.Right] = rightControllerModel.GetChild((int)Control.B_Button).GetComponent<MeshRenderer>();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && CoolDebug.hacks)
        {
            grip[(int)Hand.Left].material = activeMat;
            trigger[(int)Hand.Left].material = activeMat;
            buttonA[(int)Hand.Left].material = activeMat;
            buttonB[(int)Hand.Left].material = activeMat;

            grip[(int)Hand.Right].material = activeMat;
            trigger[(int)Hand.Right].material = activeMat;
            buttonA[(int)Hand.Right].material = activeMat;
            buttonB[(int)Hand.Right].material = activeMat;
        }
    }

    /// <summary>
    /// Incomplete sample method
    /// </summary>
    void ToggleRangeOfMotion()
    {
        leftSkelly.rangeOfMotion = EVRSkeletalMotionRange.WithController;
        rightSkelly.rangeOfMotion = EVRSkeletalMotionRange.WithController;
    }
}
