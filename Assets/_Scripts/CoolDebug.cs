using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDebug : MonoBehaviour
{
    [SerializeField]
    bool enableLogging;

    public static bool hacks = false;

    Canvas hackerCam;

    public static CoolDebug Singleton;

    UnityEngine.UI.Text hackLogger;

    // Start is called before the first frame update
    void Start()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
            Destroy(gameObject);

        hackerCam = transform.GetChild(0).GetComponent<Canvas>();

        SetLoggingStatus();

        hackLogger = GetComponentsInChildren<UnityEngine.UI.Text>()[1];

        DontDestroyOnLoad(gameObject);
    }

    public static CoolDebug GetInstance()
    {
        return Singleton;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            hacks = !hacks;
            hackerCam.enabled = hacks;
        }
    }

    /// <summary>
    /// Displays text on screen
    /// Use this to show what hack you activated
    /// </summary>
    /// <param name="text"></param>
    public void LogHack(string text = "HACKED!")
    {
        hackLogger.CrossFadeAlpha(255, 0.01f, false);
        hackLogger.text = text;
        hackLogger.CrossFadeAlpha(0, 1f, false);
    }

    void SetLoggingStatus()
    {
        Debug.unityLogger.logEnabled = enableLogging;
    }

    private void OnValidate()
    {
        SetLoggingStatus();
    }
}
