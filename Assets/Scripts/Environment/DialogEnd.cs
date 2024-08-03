using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogEnd : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isEnd;
    public Animator endAnimator;
    public Transform placePut;
    public GameObject title;
    public GameObject healthUI;
    public AudioSource audioSource;

    public AudioClip cookingClip;
    public AudioClip doneClip;
    void Start()
    {
        isEnd = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PutPlayer(GameObject player) 
    {
        player.transform.position = placePut.position;
        player.transform.rotation = placePut.rotation;

        audioSource.clip = cookingClip;
        audioSource.Play();
        endAnimator.SetBool("HeadMove", true);
        StartCoroutine(TitleShow(audioSource.clip.length, player));
    }

    private IEnumerator TitleShow(float sec, GameObject player) 
    {
        yield return new WaitForSeconds(sec);
        audioSource.clip = doneClip;
        audioSource.Play();
        title.SetActive(true);
        healthUI.SetActive(false);
        player.GetComponent<PlayerMovement>().DisableGameplay();
    }
    
}
