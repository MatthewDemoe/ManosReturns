using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerStateManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerUI;

    [SerializeField]
    GameObject bullseyeDeathParticles;

    [SerializeField]
    GameObject chadWinText;

    InputManager _inputManager;

    GameObject _chad;
    GameObject _manos;

    bool _manosDead = false;

    public int activePlayers;
    public int playersAlive;
    bool[] isDead;

    // Use this for initialization
    void Start()
    {
        _chad = GameObject.Find("Chad");
        _manos = GameObject.Find("Manos 2.3");

        _inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_manosDead)
        {
            GetInput();
        }

    }

    public void RegisterPlayerDeath(GameObject player)
    {
        if (player.name.Equals("Chad"))
        {
            _chad.GetComponent<Animator>().SetTrigger("DeathTrigger");
            _chad.GetComponent<Animator>().SetBool("Dead", true);
            _chad.GetComponent<PlayerManager>().SetAlive(false);

            _chad.GetComponent<FlashFeedback>().ChadDeath();
            
            GameObject.Find("OverlordController").GetComponent<FadeOut>().BeginFadeOut(0.0f);
            GameObject.Find("OverlordController").GetComponent<InputManager>().enabled = false;
        }

        else if(player.name.Equals("Target"))
        {
            Instantiate(bullseyeDeathParticles, player.transform.position, Quaternion.identity);
            Destroy(player);
        }

        else if(player.name.Equals("Manos 2.3"))
        {
            //StartCoroutine("ManosDeath");

            _manos.GetComponent<FlashFeedback>().ManosDeath();
            float delay = _manos.GetComponent<FlashFeedback>().GetDeathDuration() + 1.0f;
            _manos.GetComponent<Death>().Detonate(delay);
            StartCoroutine("ShowChadWinText", delay);
        
            _manosDead = true;
        }
    }

    void GetInput()
    {
        if (_inputManager.GetButtonDown(InputManager.Buttons.B))
        {
            StartCoroutine("StartChadWinScene");
        }
    }

    IEnumerator ShowChadWinText(float delay)
    {
        yield return new WaitForSeconds(delay);

        chadWinText.SetActive(true);
    }

    IEnumerator StartChadWinScene()
    {
        yield return new WaitForSeconds(1.0f);

        GameObject.Find("OverlordController").GetComponent<FadeOut>().BeginFadeOut(0.0f);
        _manosDead = true;

        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene("ChadWins");
    }
}