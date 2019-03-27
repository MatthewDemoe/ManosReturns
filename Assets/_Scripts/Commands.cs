using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commands : MonoBehaviour {

}

[System.Serializable]
abstract class Command
{

    public string sign;
    public abstract void execute();

    public GameObject guy;

}

[System.Serializable]
class NullCommand : Command
{
    public NullCommand()
    {

    }

    public override void execute()
    {

    }
}

[System.Serializable]
class ChargeJumpCmd : Command
{
    public ChargeJumpCmd(GameObject go)
    {
        guy = go;
    }

    public override void execute()
    {
        guy.GetComponent<MovementManager>().BeginChargeJump();
    }
}

[System.Serializable]
class JumpCmd : Command
{
    public JumpCmd(GameObject go)
    {
        guy = go;
    }

    public override void execute()
    {
        guy.GetComponent<MovementManager>().Jump();
    }
}

[System.Serializable]
class DashCmd : Command
{
    public DashCmd(GameObject go)
    {
        guy = go;
    }

    public override void execute()
    {
        guy.GetComponent<MovementManager>().Dash();
    }
}

[System.Serializable]
class ChargeDashCmd : Command
{
    public ChargeDashCmd(GameObject go)
    {
        guy = go;
    }

    public override void execute()
    {
        guy.GetComponent<MovementManager>().BeginChargeDash();
    }
}

[System.Serializable]
class CancelCmd : Command
{
    public CancelCmd (GameObject go)
    {
        guy = go;
    }

    public override void execute()
    {
        guy.GetComponent<MovementManager>().Cancel();
    }
}



