using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangeSceneAfterDuration : MonoBehaviour
{
    [SerializeField]
    string sceneName;

    [SerializeField]
    float duration = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("changeScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator changeScene()
    {
        yield return new WaitForSeconds(duration);

        SceneManager.LoadScene(sceneName);
    }
}
