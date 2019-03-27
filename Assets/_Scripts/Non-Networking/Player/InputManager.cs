using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

class InputManager : MonoBehaviour
{
    //State of first controller
    GamePadState state;

    //Controller sticks
    Vector2 _lStick, _rStick;
    bool Last_A = false, Last_B = false, Last_X = false, Last_Y = false, Last_L_Trig = false, Last_R_Trig = false, Last_LB = false, Last_RB = false, Last_Select = false, Last_Start = false;

    public enum Buttons { A, B, X, Y, LB, RB, Select, Start };

    // Start is called before the first frame update
    void Start()
    {
        state = GamePad.GetState(PlayerIndex.One);
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    public void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetComponent<LobbyScript>().PlayersReady();
        }

        if (GetTriggerL() >= 0.25f)
            Last_L_Trig = true;

        else
            Last_L_Trig = false;

        if (GetTriggerR() >= 0.25f)
            Last_R_Trig = true;

        else
            Last_R_Trig = false;

        //Store the previous state of the buttons to check up/down
        if (state.Buttons.A == ButtonState.Pressed)
            Last_A = true;

        else
            Last_A = false;

        if (state.Buttons.B == ButtonState.Pressed)
            Last_B = true;

        else
            Last_B = false;

        if (state.Buttons.X == ButtonState.Pressed)
            Last_X = true;

        else
            Last_X = false;

        if (state.Buttons.Y == ButtonState.Pressed)
            Last_Y = true;

        else
            Last_Y = false;

        if (state.Buttons.LeftShoulder == ButtonState.Pressed)
            Last_LB = true;

        else
            Last_LB = false;

        if (state.Buttons.RightShoulder == ButtonState.Pressed)
            Last_RB = true;

        else
            Last_RB = false;

        if (state.Buttons.Back == ButtonState.Pressed)
            Last_Select = true;

        else
            Last_Select = false;

        if (state.Buttons.Start == ButtonState.Pressed)
            Last_Start = true;

        else
            Last_Start = false;

        //Check new states
        state = GamePad.GetState(PlayerIndex.One);

        _lStick.x = state.ThumbSticks.Left.X;
        _lStick.y = state.ThumbSticks.Left.Y;

        _rStick.x = state.ThumbSticks.Right.X;
        _rStick.y = state.ThumbSticks.Right.Y;

    }

    public Vector2 GetLStick()
    {
        return _lStick;
    }

    public Vector2 GetRStick()
    {
        return _rStick;
    }

    //Checking current Button state against previous
    public bool GetButtonUp(Buttons button)
    {
        switch (button)
        {
            case (Buttons.A):
                if (Last_A)
                {
                    if (state.Buttons.A == ButtonState.Released)
                    {
                        return true;
                    }
                }

                return false;

            case (Buttons.B):
                if (Last_B)
                {
                    if (state.Buttons.B == ButtonState.Released)
                        return true;
                }

                return false;

            case (Buttons.X):
                if (Last_X)
                {
                    if (state.Buttons.X == ButtonState.Released)
                        return true;
                }

                return false;

            case (Buttons.Y):
                if (Last_Y)
                {
                    if (state.Buttons.Y == ButtonState.Released)
                        return true;
                }

                return false;

            case (Buttons.LB):
                if (Last_LB)
                {
                    if (state.Buttons.LeftShoulder == ButtonState.Released)
                        return true;
                }

                return false;

            case (Buttons.RB):
                if (Last_RB)
                {
                    if (state.Buttons.RightShoulder == ButtonState.Released)
                        return true;
                }

                return false;

            case (Buttons.Select):
                if (Last_Select)
                {
                    if (state.Buttons.Back == ButtonState.Released)
                        return true;
                }

                return false;

            case (Buttons.Start):
                if (Last_Start)
                {
                    if (state.Buttons.Start == ButtonState.Released)
                        return true;
                }

                return false;
        }

        return false;
    }

    //Checking current Button state against previous
    public bool GetButtonDown(Buttons button) 
    {
        switch (button)
        {
            case (Buttons.A):
                if (!Last_A)
                {
                    if (state.Buttons.A == ButtonState.Pressed)
                    {
                        return true;
                    }
                }

                return false;

            case (Buttons.B):
                if (!Last_B)
                {
                    if (state.Buttons.B == ButtonState.Pressed)
                        return true;
                }

                return false;

            case (Buttons.X):
                if (!Last_X)
                {
                    if (state.Buttons.X == ButtonState.Pressed)
                        return true;
                }

                return false;

            case (Buttons.Y):
                if (!Last_Y)
                {
                    if (state.Buttons.Y == ButtonState.Pressed)
                        return true;
                }

                return false;

            case (Buttons.LB):
                if (!Last_LB)
                {
                    if (state.Buttons.LeftShoulder == ButtonState.Pressed)
                        return true;
                }

                return false;

            case (Buttons.RB):
                if (!Last_RB)
                {
                    if (state.Buttons.RightShoulder == ButtonState.Pressed)
                        return true;
                }

                return false;

            case (Buttons.Select):
                if (!Last_Select)
                {
                    if (state.Buttons.Back == ButtonState.Pressed)
                        return true;
                }

                return false;

            case (Buttons.Start):
                if (!Last_Start)
                {
                    if (state.Buttons.Start == ButtonState.Pressed)
                        return true;
                }

                return false;
        }

        return false;
    }

    //Return current state of button
    //Stupid XInput plugin uses Enums :(
    public bool GetButton(Buttons button)
    {
        switch (button)
        {
            case (Buttons.A):
                if (state.Buttons.A == ButtonState.Pressed)
                    return true;

                return false;

            case (Buttons.B):
                if (state.Buttons.B == ButtonState.Pressed)
                    return true;

                return false;

            case (Buttons.X):
                if (state.Buttons.X == ButtonState.Pressed)
                    return true;

                return false;

            case (Buttons.Y):
                if (state.Buttons.Y == ButtonState.Pressed)
                    return true;

                return false;

            case (Buttons.LB):
                if (state.Buttons.LeftShoulder == ButtonState.Pressed)
                    return true;

                return false;

            case (Buttons.RB):
                if (state.Buttons.RightShoulder == ButtonState.Pressed)
                    return true;

                return false;

            case (Buttons.Select):
                if (state.Buttons.Guide == ButtonState.Pressed)
                    return true;

                return false;

            case (Buttons.Start):
                if (state.Buttons.Start == ButtonState.Pressed)
                    return true;

                return false;
        }

        return false;
    }

    public float GetTriggerL()
    {
        return state.Triggers.Left;
    }

    public float GetTriggerR()
    {
        return state.Triggers.Right;
    }

    public bool LTrigDown()
    {
        if (GetTriggerL() >= 0.25f)
        {
            if (Last_L_Trig == false)
                return true;
        }

        return false;
    }

    public bool RTrigDown()
    {
        if (GetTriggerR() >= 0.25f)
        {
            if (Last_R_Trig == false)
                return true;
        }

        return false;
    }

    public bool LTrigUp()
    {
        if (GetTriggerL() < 0.25f)
        {
            if (Last_L_Trig == true)
                return true;
        }

        return false;
    }

    public bool RTrigUp()
    {
        if (GetTriggerR() < 0.25f)
        {
            if (Last_R_Trig == true)
                return true;
        }

        return false;
    }
}
