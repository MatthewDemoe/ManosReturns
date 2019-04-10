using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbleController : MonoBehaviour
{
    [SerializeField]
    InputManager inputMan;

    [SerializeField]
    float rumbleDelay = 0.1f;

    float rumbleTimer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (rumbleTimer > 0.0f)
            rumbleTimer -= Time.deltaTime;        

        else
            SetVibration(0.0f, 0.0f);
    }

    public void SetVibration(float left, float right)
    {
        inputMan.SetVibration(left, right);
        rumbleTimer = rumbleDelay;
    }

    public void RumbleForDuration(float left, float right, float duration)
    {
        StartCoroutine(Rumble(left, right, duration));
    }

    IEnumerator Rumble(float left, float right, float duration)
    {
        inputMan.SetVibration(left, right);

        yield return new WaitForSeconds(duration);

        inputMan.SetVibration(0.0f, 0.0f);
    }
        
}
