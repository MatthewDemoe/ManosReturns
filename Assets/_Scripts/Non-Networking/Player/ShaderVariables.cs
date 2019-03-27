using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderVariables : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    float minDelay = 0.1f;

    [SerializeField]
    float maxDelay = 0.5f;

    float flickerDelay = 0.25f;


    float timer = 0.0f;

    //[SerializeField]
    Renderer rend;

    Material mat;
    bool glowing = false;

    MovementManager move;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        move = player.GetComponent<MovementManager>();
        mat = rend.material;
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;

        flickerDelay = Mathf.Lerp(maxDelay, minDelay, move.GetChargePercent());

        if (move.IsChargingDash() || move.IsChargingJump())
        {
            if ((timer >= flickerDelay) && (move.GetChargePercent() > 0.25f))
                Flicker();
        }

        else if(glowing)
        {
            Flicker();
        }
    }

    public void Flicker()
    {
        if (!glowing)
        {
            rend.material.SetFloat("_Glow", 1.0F);
            glowing = !glowing;
            timer = 0.0f;
        }

        else
        {
            rend.material.SetFloat("_Glow", 0.0F);
            glowing = !glowing;
            timer = 0.0f;
        }
    }
}
