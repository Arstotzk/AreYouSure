using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    private int _health = 3;
    public int health 
    { 
        get 
        {
            return _health;
        }
        set 
        {
            _health = value;
            healthUI.GetNewDamage();
            healthDamage.Play();
            damageShowStrenght = 1f;
        }
    }

    public HealthUI healthUI;
    public AudioSource healthDamage;
    public Volume damageVolume;
    private float damageShowStrenght;
    public float showStrengperFrame = 0.05f;
    void Start()
    {
        damageShowStrenght = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (damageShowStrenght > 0)
        {
            damageShowStrenght -= showStrengperFrame;
        }
        if (damageShowStrenght < 0)
            damageShowStrenght = 0f;

        damageVolume.weight = damageShowStrenght;
    }

    public void Damage()
    {
        health--;
    }
}
