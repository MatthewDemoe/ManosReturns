using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundJustOnce : MonoBehaviour
{
    /// <summary>
    /// Because Joss wanted it
    /// </summary>

    [SerializeField]
    AudioManager.Sound theSound;

    private void OnEnable()
    {
        AudioManager.GetInstance().PlaySoundOnce(theSound, transform);
        enabled = false;
    }

}
