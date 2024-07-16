using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject focusPoint;
    private bool dialogStarted;
    void Start()
    {
        dialogStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartDialog(GameObject player) 
    {
        dialogStarted = true;
        player.GetComponent<PlayerMovement>().OnDialog(focusPoint, this);
    }
    public void ExitDialog(GameObject player) 
    {
        player.GetComponent<PlayerMovement>().ExitDialog();
    }
    public void Interact(GameObject player) 
    {
        if (dialogStarted == false)
            StartDialog(player);

        if (dialogStarted == true)
            ExitDialog(player);

    }
}
