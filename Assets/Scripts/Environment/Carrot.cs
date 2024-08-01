using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : Item
{
    public float delay = 2f;
    private Animator animator;
    private AudioSource audioSource;
    private bool isShowed = false;
    new void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public override void ActionPickUp()
    {
        if (isMimic)
            StartCoroutine(ShowEyes(delay));
    }

    private IEnumerator ShowEyes(float sec)
    {
        yield return new WaitForSeconds(sec);
        if (!isShowed)
        {
            animator.SetBool("Show", true);
            audioSource.Play();
            isShowed = true;
        }
    }
}
