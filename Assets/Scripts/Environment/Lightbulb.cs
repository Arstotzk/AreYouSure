using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightbulb : Item
{
    public float delay = 1f;
    public AudioClip turnOnClip;
    public AudioClip lightingClip;
    private Animator animator;
    private AudioSource audioSource;
    private bool isOn = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActionPickUp()
    {
        if (isMimic)
            StartCoroutine(TurnLightOn(delay));
    }

    private IEnumerator TurnLightOn(float sec)
    {
        yield return new WaitForSeconds(sec);
        if (!isOn)
        {
            animator.SetBool("IsOn", true);
            isOn = true;
        }
    }
    public override void Drop()
    {
        animator.SetBool("IsOn", false);
        audioSource.Stop();
        isOn = false;
    }

    public void PlaySound() 
    {
        audioSource.clip = turnOnClip;
        audioSource.Play();
    }
    public void PlaySoundLighting() 
    {
        audioSource.clip = lightingClip;
        audioSource.Play();
    }
}
