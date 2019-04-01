using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbleController : MonoBehaviour
{
    [SerializeField]
    InputManager inputMan;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVibration(float left, float right)
    {
        inputMan.SetVibration(left, right);
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
