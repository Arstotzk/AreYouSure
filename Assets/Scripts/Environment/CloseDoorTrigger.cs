using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoorTrigger : MonoBehaviour
{
    public AudioSource source;
    public DoorScript.Door door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            source.Play();
            door.open = false;
            door.isCanOpen = false;
        }
    }
}
