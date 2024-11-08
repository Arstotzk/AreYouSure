using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogItem : Dialog
{
    public override void Interact(GameObject player) 
    {
        if (dialogStarted == false)
        {
            StartDialog(player);
        }

        if (phase == DialogPhase.Welcome)
        {
            WelcomeDialog();
        }

        if (dialogStarted == true && dialogNeedToEnd == true)
        {
            ExitDialog(player);
            SetItemActive();
            dialogStarted = false;
            dialogNeedToEnd = false;
        }
    }

    private void SetItemActive() 
    {
        var item = GetComponent<Item>();
        item.enabled = true;
        enabled = false;
    }
}
