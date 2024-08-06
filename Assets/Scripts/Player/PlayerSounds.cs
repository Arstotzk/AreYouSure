using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public List<AudioClip> walkClips;
    public float walkDelay = 0.3f;
    private AudioSource moveAudio;
    private bool isMoveCoroutineStart;
    // Start is called before the first frame update
    void Start()
    {
        moveAudio = GetComponent<AudioSource>();
        isMoveCoroutineStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TryPlayMoveSound() 
    {
        if(!isMoveCoroutineStart)
            StartCoroutine(PlayMoveSound(walkDelay, GetWalkClip()));
    }
    private IEnumerator PlayMoveSound(float time, AudioClip clip)
    {
        isMoveCoroutineStart = true;
        moveAudio.clip = clip;
        moveAudio.Play();
        yield return new WaitForSeconds(time);
        isMoveCoroutineStart = false;
    }
    public AudioClip GetWalkClip()
    {
        return walkClips[Random.Range(0, walkClips.Count)];
    }
}
