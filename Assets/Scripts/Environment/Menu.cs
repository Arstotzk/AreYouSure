using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public AudioMixer audioMixer;
    public Slider soundSlider;
    public Slider sensativitySlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ExitGame() 
    {
        Application.Quit();
    }
    public void ChangeSound()
    {
        var value = soundSlider.value;
        if (value == 0)
            value = 0.0001f;
        audioMixer.SetFloat("Master", Mathf.Log10(value) * 20);
    }
    public void ChangeSensitivity()
    {
        var value = sensativitySlider.value;
        playerMovement.sensitivity = value * 20;
    }
}
