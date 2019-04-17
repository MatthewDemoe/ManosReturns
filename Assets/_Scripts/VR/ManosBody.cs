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

    private void Update()
    {
        if (CoolDebug.hacks)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                manos.transform.position = new Vector3(manos.transform.position.x, manos.transform.position.y + 1, manos.transform.position.z);
                CoolDebug.GetInstance().LogHack("Moved Manos Up!");
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                manos.transform.position = new Vector3(manos.transform.position.x, manos.transform.position.y - 1, manos.transform.position.z);
                CoolDebug.GetInstance().LogHack("Moved Manos Down!");
            }
        }
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