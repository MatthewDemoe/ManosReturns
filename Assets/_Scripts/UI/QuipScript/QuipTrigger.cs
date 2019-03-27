using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuipTrigger : MonoBehaviour
{

    [SerializeField]
    Enums.Player player;
    [SerializeField]
    DialogManager dialogManager;
    [SerializeField]
    float quipTimer = 1;

    // Start is called before the first frame update
    void Start()
    {
        dialogManager = FindObjectOfType<DialogManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Quip"))
        {
            dialogManager.Quip(player, quipTimer);
        }

        getChadInput();
        getManosInput();
    }

    bool getChadInput()
    {


        if (player == Enums.Player.Player1 &&( Input.GetButtonDown("A Button") || Input.GetKeyDown(KeyCode.C)))
        {
            dialogManager.Quip(player, quipTimer);

           //if (dialogManager.GetquipOrderState()==DialogManager.QuipOrder.FIRST)
           //dialogManager.Quip(player, quipTimer);
           //
           //if (dialogManager.GetquipOrderState() == DialogManager.QuipOrder.SECOND)
           //    dialogManager.Quip(player, quipTimer);
           return true;
        }
   
        return false;
    }
    bool getManosInput()
    {
        if (player== Enums.Player.Manos && Input.GetKeyDown(KeyCode.M))
        {
            dialogManager.Quip(player, quipTimer);
            //if (dialogManager.GetquipOrderState() == DialogManager.QuipOrder.FIRST)
            //    dialogManager.Quip(player, quipTimer);
            //
            //if (dialogManager.GetquipOrderState() == DialogManager.QuipOrder.SECOND)
            //    dialogManager.Quip(player, quipTimer);
            return true;
        }
   
            return false;
    }

}
