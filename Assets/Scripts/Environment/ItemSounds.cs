using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSounds : MonoBehaviour
{
    public AudioClip dropClip;
    
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDrop() 
    {
        audioSource.clip = dropClip;
        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("ItemSound OnTriggerEnder: " + other.name + " tag: " + other.tag);
        if(other.tag == "Ground")
            PlayDrop();
    }
}
