using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenController : MonoBehaviour {

    [SerializeField]
    GameObject restartButtonEnabled;

    [SerializeField]
    GameObject restartButtonDisabled;

    [SerializeField]
    GameObject exitButtonEnabled;

    [SerializeField]
    GameObject exitButtonDisabled;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        getInput();
	}

    void getInput()
    {
        if (Input.GetAxis("Vertical") < 0)
        {      
            restartButtonEnabled.SetActive(false);
            restartButtonDisabled.SetActive(true);

            exitButtonEnabled.SetActive(true);
            exitButtonDisabled.SetActive(false);
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            restartButtonEnabled.SetActive(true);
            restartButtonDisabled.SetActive(false);

            exitButtonEnabled.SetActive(false);
            exitButtonDisabled.SetActive(true);
        }

        if (Input.GetButton("Fire1"))
        {
            if (restartButtonEnabled.activeInHierarchy)
                SceneManager.LoadScene("Title");

           else
               Application.Quit();;
        }
    }
}
