using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManosBody : MonoBehaviour
{
    [SerializeField]
    Manos manos;

    [SerializeField]
    ManosHand leftHand;

    [SerializeField]
    ManosHand rightHand;

    [SerializeField]
    Camera head;

    [SerializeField]
    Transform chest;

    [SerializeField]
    Transform gauntletL;

    [SerializeField]
    Transform gauntletR;

    public static ManosBody Singleton;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
            Destroy(this);
    }

    public static ManosBody GetInstance()
    {
        return Singleton;
    }

    public Manos GetManos()
    {
        return manos;
    }

    public ManosHand GetHand(Enums.Hand h)
    {
        switch (h)
        {
            case Enums.Hand.Left:
                return leftHand;
            case Enums.Hand.Right:
                return rightHand;
        }
        return null;
    }

    public Camera GetEye()
    {
        return head;
    }

    public Transform GetChest()
    {
        return chest;
    }

    public Transform GetGauntlet(Enums.Hand h)
    {
        switch (h)
        {
            case Enums.Hand.Left:
                return gauntletL;
            case Enums.Hand.Right:
                return gauntletR;
        }
        return null;
    }
}