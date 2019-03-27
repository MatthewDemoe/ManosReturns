using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManosBeam : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim.SetTrigger("FireBeam");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
