using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    // Start is called before the first frame update
    public Image health1;
    public Image health2;
    public Image health3;
    public Animator healthAnimator;

    public Sprite healthFull;
    public Sprite healthZero;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage() 
    {
        playerHealth.Damage();
    }
    public void GetNewDamage()
    {
        if (playerHealth.health == 2)
        {
            healthAnimator.SetBool("Show", true);
            healthAnimator.SetBool("Damage", true);
        }
        else
        {
            healthAnimator.SetBool("Damage", true);
        }
    }
    public void HealthToZero() 
    {
        if (playerHealth.health == 2)
            health3.sprite = healthZero;
        else if (playerHealth.health == 1)
            health2.sprite = healthZero;
        else if (playerHealth.health == 0)
            health1.sprite = healthZero;
    }
    public void HealthToFull()
    {
        if (playerHealth.health == 2)
            health3.sprite = healthFull;
        else if (playerHealth.health == 1)
            health2.sprite = healthFull;
        else if (playerHealth.health == 0)
            health1.sprite = healthFull;
    }
    public void HealthCompleteLose() 
    {
        healthAnimator.SetBool("Damage", false);
    }
}
