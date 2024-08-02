using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingDoor : MonoBehaviour
{
    public GameObject door;
    public Renderer doorRenderer;
    public GameObject wall;
    private Collider collider;
    public AudioSource audioSource;

    void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !doorRenderer.isVisible)
        {
            VanishDoor();
        }
    }

    private void VanishDoor() 
    {
        wall.SetActive(true);
        door.SetActive(false);
        collider.enabled = false;
        audioSource.Play();
    }
}
