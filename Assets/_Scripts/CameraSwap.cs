using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwap : MonoBehaviour
{

    public enum Cameras
    {
        PlayerView,
        ManosView,
        SpectatorView,
        BehindManos,
        SplitScreen
    }

    [SerializeField]
    [Tooltip("Should follow the order of enums")]
    Camera[] cams;

    [SerializeField]
    Cameras activeCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ResetCameras();
            activeCam = Cameras.PlayerView;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ResetCameras();
            activeCam = Cameras.ManosView;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ResetCameras();
            activeCam = Cameras.SpectatorView;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ResetCameras();
            activeCam = Cameras.BehindManos;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ResetCameras();
            activeCam = Cameras.SplitScreen;
        }
        cams[(int)activeCam].depth = 1;
    }

    void ResetCameras()
    {
        foreach (Camera c in cams)
        {
            c.depth = -1;
        }
    }
}
