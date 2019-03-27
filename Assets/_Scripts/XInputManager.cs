//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using XInputDotNetPure;

//public class XInputManager : MonoBehaviour
//{

//    public bool[] upCheckedLeft = new bool[4];
//    public bool[] upCheckedRight = new bool[4];

//    public bool[] upReadyLeft = new bool[4];
//    public bool[] upReadyRight = new bool[4];

//    public bool[] readyShoulderLeft = new bool[4];
//    public bool[] readyShoulderRight = new bool[4];
//    public bool[] upShoulderLeft = new bool[4];
//    public bool[] upShoulderRight = new bool[4];

//    void Update()
//    {
//        for (int i = 0; i < 4; i++)
//        {
//            if (upCheckedLeft[i])
//            {
//                upCheckedLeft[i] = false;
//            }
//            if (upCheckedRight[i])
//            {
//                upCheckedRight[i] = false;
//            }

//            if (upShoulderLeft[i])
//            {
//                upShoulderLeft[i] = false;
//            }
//            if (upShoulderRight[i])
//            {
//                upShoulderRight[i] = false;
//            }
//        }

//        //for (PlayerIndex i = 0; (int)i < GameStateManager.activePlayers; i++)
//        for (PlayerIndex i = 0; (int)i < 4; i++)
//        {
//            if (GamePad.GetState(i).Triggers.Left == 0 && upReadyLeft[(int)i])
//            {
//                upCheckedLeft[(int)i] = true;
//                upReadyLeft[(int)i] = false;
//            }
//            else if (GamePad.GetState(i).Triggers.Left > 0)
//            {
//                upReadyLeft[(int)i] = true;
//            }
//            if (GamePad.GetState(i).Triggers.Right == 0 && upReadyRight[(int)i])
//            {
//                upCheckedRight[(int)i] = true;
//                upReadyRight[(int)i] = false;
//            }
//            else if (GamePad.GetState(i).Triggers.Right > 0)
//            {
//                upReadyRight[(int)i] = true;
//            }

//            if (GamePad.GetState(i).Buttons.LeftShoulder == ButtonState.Released && readyShoulderLeft[(int)i])
//            {
//                upShoulderLeft[(int)i] = true;
//                readyShoulderLeft[(int)i] = false;
//            }
//            else if (GamePad.GetState(i).Buttons.LeftShoulder == ButtonState.Pressed)
//            {
//                readyShoulderLeft[(int)i] = true;
//            }
//            if (GamePad.GetState(i).Buttons.RightShoulder == ButtonState.Released && readyShoulderRight[(int)i])
//            {
//                upShoulderRight[(int)i] = true;
//                readyShoulderRight[(int)i] = false;
//            }
//            else if (GamePad.GetState(i).Buttons.RightShoulder == ButtonState.Pressed)
//            {
//                readyShoulderRight[(int)i] = true;
//            }
//        }
//    }

//    public bool IsTriggerUpLeft(PlayerIndex player)
//    {
//        return upCheckedLeft[(int)player];
//    }

//    public bool IsTriggerUpRight(PlayerIndex player)
//    {
//        return upCheckedRight[(int)player];
//    }

//    public bool IsShoulderUpLeft(PlayerIndex player)
//    {
//        return upShoulderLeft[(int)player];
//    }

//    public bool IsShoulderUpRight(PlayerIndex player)
//    {
//        return upShoulderRight[(int)player];
//    }
//}