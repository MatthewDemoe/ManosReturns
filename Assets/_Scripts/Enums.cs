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

    /// <summary>
    /// Arms refer to both Vambrace and Hand
    /// </summary>
    public enum ManosParts
    {
        None = -1,
        Head,
        LeftHand,
        RightHand,
        LeftVambrace,
        RightVambrace,
        Chest,
        LeftArm,
        RightArm
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
        ChargingThrow,
        Dabbing
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
