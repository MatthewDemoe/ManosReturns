using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManosTargets : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = transform.parent.GetComponent<Animator>();
    }

    public void Impulse()
    {
        anim.SetTrigger("Impulse");
    }
}
