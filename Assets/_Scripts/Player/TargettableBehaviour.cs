using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargettableBehaviour : MonoBehaviour
{
    TargetLockOn targetter;

    public void DesignateTargetter(TargetLockOn t)
    {
        targetter = t;
    }
    
    private void OnDisable()
    {
        if (targetter != null)
        {
            targetter.RemoveEnemy(transform);
        }
    }

    private void OnDestroy()
    {
        if (targetter != null)
        {
            targetter.RemoveEnemy(transform);
        }
    }
}
