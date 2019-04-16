using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Game state timer. After a certain time, the game will enter Sudden Death
/// e.g.
/// TIME IS UP!
/// SUDDEN DEATH! ALL HEALTH REDUCED BY %!
/// </summary>

public class GameTimer : MonoBehaviour
{
    [Tooltip("All players to be affected")]
    [SerializeField]
    PlayerHealth[] combatantHealthScripts;

    [Tooltip("All listeners")]
    [SerializeField]
    TextPrompt[] textPrompters;

    [Header("light groups to change the look of combat based on game state")]
    [SerializeField]
    GameObject lightGroupNormal;
    [SerializeField]
    GameObject lightGroupSuddenDeath;

    [Tooltip("Time until Sudden Death from the start of the match")]
    [SerializeField]
    float timeTillSuddenDeath = 300.0f;

    [Tooltip("number of seconds remaining")]
    [SerializeField]
    float _timeRemaining = 10.0f;

    [Tooltip("Health percentage based damage from 0-1")]
    [SerializeField]
    float suddenDeathHealthDamagePercent = 0.8f;

    [SerializeField]
    float updateInterval = 0.1f;

    [SerializeField]
    float messageInterval = 2.0f;

    /// <summary>
    /// disabled on Sudden Death
    /// </summary>
    [SerializeField]
    GameObject healthPickups;

    //[SerializeField]
    //Text messageText;

    [SerializeField]
    Text timerText;

    [SerializeField]
    Color suddenDeathColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    bool _timerRunning = false;

    private void Start()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        _timerRunning = true;
        _timeRemaining = timeTillSuddenDeath;
        StartCoroutine(TickTimer());
    }

    IEnumerator TickTimer()
    {
        while (_timerRunning)
        {
            //if (_timerRunning)
            //{
            //Debug.Log("timer tick: " + _timeRemaining);
            _timeRemaining -= updateInterval;

            if (_timeRemaining < 0)
            {
                _timeRemaining = 0.0f;
                OnTimerEnd();
            }

            // ty http://answers.unity.com/answers/315200/view.html
            int minutes = Mathf.FloorToInt(_timeRemaining / 60F);
            int seconds = Mathf.FloorToInt(_timeRemaining - minutes * 60);
            string timerString = string.Format("{0:0}:{1:00}", minutes, seconds);

            timerText.text = timerString;
            //}
            yield return new WaitForSeconds(updateInterval);
        }
    }

    void OnTimerEnd()
    {
        StartCoroutine(EnterSuddenDeath());
        timerText.color = suddenDeathColor;
        _timerRunning = false;
    }

    void NotifyAllListeners(string message, float duration, bool flash = true)
    {
        foreach (TextPrompt promter in textPrompters)
        {
            promter.PlayPrompt(message, duration);

            if (flash)
            {
                promter.FlashTextColor(suddenDeathColor, duration);
            }
        }
    }

    IEnumerator EnterSuddenDeath()
    {
        //messageText.text = "Time has run out...";
        NotifyAllListeners("Time has run out...", messageInterval);
        yield return new WaitForSeconds(messageInterval);

        //messageText.color = timeOutColor;
        //messageText.text = "SUDDEN DEATH!";
        NotifyAllListeners("SUDDEN DEATH!", messageInterval);
        yield return new WaitForSeconds(messageInterval);
        
        NotifyAllListeners("All player health will be reduced by " + suddenDeathHealthDamagePercent * 100.0f + "%!", messageInterval);
        Destroy(healthPickups);
        // change lighting to show the change in game state
        lightGroupNormal.SetActive(false);
        yield return new WaitForSeconds(messageInterval);

        // remove all health from the scene
        NotifyAllListeners("END IT!", timeTillSuddenDeath);
        lightGroupSuddenDeath.SetActive(true);

        //All player's hitpoints will be reduced by suddenDeathHealthDamagePercent of their current health.
        foreach (PlayerHealth health in combatantHealthScripts)
        {
            float damageToDeal = health.GetHealth() * suddenDeathHealthDamagePercent;
            if (damageToDeal >= health.GetHealth())
            {
                damageToDeal -= 1.0f;
            }

            health.TakeDamage(damageToDeal);
        }
    }
}
