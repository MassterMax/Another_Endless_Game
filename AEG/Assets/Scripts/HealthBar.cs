using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //Creature parentCreature;
    Slider slider;

    public void Setup(float currentHealth, float maxHealth)
    {
        //var canvas = transform.parent;
        //canvas.GetComponent<Canvas>().worldCamera = FindObjectOfType<Camera>(); // todo wtf

        /*
        parentCreature = canvas.parent.gameObject.GetComponent<Creature>();

        slider.maxValue = parentCreature.GetMaxHealth();
        slider.value = parentCreature.GetHealth();*/
        slider = GetComponent<Slider>();
        slider.value = currentHealth;
        slider.maxValue = maxHealth;
    }

    public void SetHealth(float value)
    {
        slider.value = value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
