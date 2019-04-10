using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSkip : MonoBehaviour
{
    [SerializeField]
    string nextScene;

    [SerializeField]
    InputManager inMan;

    [SerializeField]
    bool autoSkip = false;

    [SerializeField]
    float skipDelay = 7.5f;

    void Start()
    {
        if(autoSkip)
            StartCoroutine(ChangeAfterDelay());
    }

    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (inMan.GetButtonDown(InputManager.Buttons.Start))
        {
            LoadScene();
        }
    }

    public void LoadScene()
    {

        StopAllCoroutines();
        SceneManager.LoadScene(nextScene);
    }

    IEnumerator ChangeAfterDelay()
    {
        yield return new WaitForSeconds(skipDelay);
        LoadScene();
    }
}
