﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerStateManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerUI;

    GameObject _chad;
    GameObject _manos;

    public int activePlayers;
    public int playersAlive;
    bool[] isDead;

    // Use this for initialization
    void Start()
    {
        _chad = GameObject.Find("Chad");
        _manos = GameObject.Find("Manos 2.3");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RegisterPlayerDeath(GameObject player)
    {

        if (player.name.Equals("Chad"))
        {
            _chad.GetComponent<Animator>().SetTrigger("DeathTrigger");
            _chad.GetComponent<FlashFeedback>().ChadDeath();
            
            GameObject.Find("OverlordController").GetComponent<FadeOut>().BeginFadeOut(0.0f);
            GameObject.Find("OverlordController").GetComponent<InputManager>().enabled = false;

        }

        else
        {
            _manos.GetComponent<FlashFeedback>().ManosDeath();
            GameObject.Find("OverlordController").GetComponent<FadeOut>().BeginFadeOut(0.0f);
        }
    }
}