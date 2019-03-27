using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Enums {

    public enum Poses
    {
        None,
        Punch,
        Grab,
        Swipe,
        Gun
    }

    public enum Hand
    {
        None,
        Left,
        Right,
        Both
    }

    public enum Player
    {
        None,
        Manos,
        Player1,
    }

    public enum ChargingState
    {
        None = 1, 
        ChargingJump, 
        ChargingDash,
        Dashing,
        Flip, 
        ChargingThrow
    }

    public enum PlayerState
    {
        Grounded = 1, 
        JumpUp,
        Falling, 
        Grabbed, 
        Knocked
    }
}
