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
    bool Last_A = false, Last_B = false, Last_X = false, Last_Y = false, Last_L_Trig = false, Last_R_Trig = false, Last_LB = false, Last_RB = false, Last_Select = false, Last_Start = false, Last_DPadUp = false, Last_DPadDown = false, Last_DPadLeft = false, Last_DPadRight = false, Last_StickLeft = false, Last_StickRight = false;

    bool leftStickFlicked = false, rightStickFlicked = false;
    bool leftFlickOnce = false, rightFlickOnce = false;

    public enum Buttons { A, B, X, Y, LB, RB, Select, Start, DPadUp, DPadDown, DPadLeft, DPadRight, StickLeft, StickRight };

    public enum StickDirection
    {
        Centered,
        Left,
        Right,
        Up,
        Down
    }

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
        /* should be like this */

        Last_L_Trig = (GetTriggerL() >= 0.25f) ? true : false;

        Last_R_Trig = (GetTriggerR() >= 0.25f) ? true : false;

        /* ALL OF THIS SHIT BELOW MUST GO */

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
        // DPadUp, DPadDown, DPadLeft, DPadRight
        if (state.DPad.Up == ButtonState.Pressed)
            Last_DPadUp = true;

        else
            Last_DPadUp = false;

        if (state.DPad.Down == ButtonState.Pressed)
            Last_DPadDown = true;

        else
            Last_DPadDown = false;

        if (state.DPad.Left == ButtonState.Pressed)
            Last_DPadLeft = true;

        else
            Last_DPadLeft = false;

        if (state.DPad.Right == ButtonState.Pressed)
            Last_DPadRight = true;

        else
            Last_DPadRight = false;

        if (state.Buttons.LeftStick == ButtonState.Pressed)
            Last_StickLeft = true;

        else
            Last_StickLeft = false;

        if (state.Buttons.RightStick == ButtonState.Pressed)
            Last_StickRight = true;

        else
            Last_StickRight = false;

        if (leftStickFlicked)
        {
            leftStickFlicked = false;
        }
            

        if (rightStickFlicked)
        {
            rightStickFlicked = false;
        }
            

        //Check new states
        state = GamePad.GetState(PlayerIndex.One);

        _lStick.x = state.ThumbSticks.Left.X;
        _lStick.y = state.ThumbSticks.Left.Y;

        _rStick.x = state.ThumbSticks.Right.X;
        _rStick.y = state.ThumbSticks.Right.Y;

        // Check if you flicked a stick
        if (!IsLeftStick(StickDirection.Centered) && !leftFlickOnce)
        {
            leftStickFlicked = true;
            leftFlickOnce = true;
        }
        else if (IsLeftStick(StickDirection.Centered)){
            leftFlickOnce = false;
        }

        if (!IsRightStick(StickDirection.Centered) && !rightFlickOnce)
        {
            rightStickFlicked = true;
            rightFlickOnce = true;
        }
        else if (IsRightStick(StickDirection.Centered))
        {
            rightFlickOnce = false;
        }
    }

    public bool IsLeftStick(StickDirection d)
    {
        switch (d)
        {
            case StickDirection.Centered:
                if (_lStick.magnitude < 0.75f) return true;
                break;
            case StickDirection.Left:
                if (_lStick.x < -0.8) return true;
                break;
            case StickDirection.Right:
                if (_lStick.x > 0.8) return true;
                break;
            case StickDirection.Up:
                if (_lStick.y > 0.8) return true;
                break;
            case StickDirection.Down:
                if (_lStick.y < -0.8) return true;
                break;
        }
        return false;
    }

    public bool IsRightStick(StickDirection d)
    {
        switch (d)
        {
            case StickDirection.Centered:
                if (_rStick.magnitude < 0.75f) return true;
                break;
            case StickDirection.Left:
                if (_rStick.x < -0.8) return true;
                break;
            case StickDirection.Right:
                if (_rStick.x > 0.8) return true;
                break;
            case StickDirection.Up:
                if (_rStick.y > 0.8) return true;
                break;
            case StickDirection.Down:
                if (_rStick.y < -0.8) return true;
                break;
        }
        return false;
    }

    public Vector2 GetLStick()
    {
        return _lStick;
    }

    public Vector2 GetRStick()
    {
        return _rStick;
    }

    public bool LeftStickFlicked()
    {
        return leftStickFlicked;
    }

    public bool RightStickFlicked()
    {
        return rightStickFlicked;
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

            case (Buttons.DPadUp):
                if (Last_DPadUp)
                {
                    if (state.DPad.Up == ButtonState.Released)
                        return true;
                }

                return false;

            case (Buttons.DPadDown):
                if (Last_DPadDown)
                {
                    if (state.DPad.Down == ButtonState.Released)
                        return true;
                }

                return false;

            case (Buttons.DPadLeft):
                if (Last_DPadLeft)
                {
                    if (state.DPad.Left == ButtonState.Released)
                        return true;
                }

                return false;

            case (Buttons.DPadRight):
                if (Last_DPadRight)
                {
                    if (state.DPad.Right == ButtonState.Released)
                        return true;
                }

                return false;

            case (Buttons.StickLeft):
                if (Last_StickLeft)
                {
                    if (state.Buttons.LeftStick == ButtonState.Released)
                        return true;
                }

                return false;

            case (Buttons.StickRight):
                if (Last_StickRight)
                {
                    if (state.Buttons.RightStick == ButtonState.Released)
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


            case (Buttons.DPadUp):
                if (!Last_DPadUp)
                {
                    if (state.DPad.Up == ButtonState.Pressed)
                        return true;
                }

                return false;

            case (Buttons.DPadDown):
                if (!Last_DPadDown)
                {
                    if (state.DPad.Down == ButtonState.Pressed)
                        return true;
                }

                return false;

            case (Buttons.DPadLeft):
                if (!Last_DPadLeft)
                {
                    if (state.DPad.Left == ButtonState.Pressed)
                        return true;
                }

                return false;

            case (Buttons.DPadRight):
                if (!Last_DPadRight)
                {
                    if (state.DPad.Right == ButtonState.Pressed)
                        return true;
                }

                return false;

            case (Buttons.StickLeft):
                if (!Last_StickLeft)
                {
                    if (state.Buttons.LeftStick == ButtonState.Pressed)
                        return true;
                }

                return false;

            case (Buttons.StickRight):
                if (!Last_StickRight)
                {
                    if (state.Buttons.RightStick == ButtonState.Pressed)
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


            case (Buttons.DPadUp):
             
                    if (state.DPad.Up == ButtonState.Pressed)
                        return true;
                

                return false;

            case (Buttons.DPadDown):
            
                    if (state.DPad.Down == ButtonState.Pressed)
                        return true;
           

                return false;

            case (Buttons.DPadLeft):
            
                    if (state.DPad.Left == ButtonState.Pressed)
                        return true;
                

                return false;

            case (Buttons.DPadRight):
              
                    if (state.DPad.Right == ButtonState.Pressed)
                        return true;
                

                return false;

            case (Buttons.StickLeft):

                if (state.Buttons.LeftStick == ButtonState.Pressed)
                    return true;


                return false;

            case (Buttons.StickRight):

                if (state.Buttons.RightStick == ButtonState.Pressed)
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

    public void SetVibration(float left, float right)
    {
        GamePad.SetVibration(PlayerIndex.One, left, right);
    }
}
