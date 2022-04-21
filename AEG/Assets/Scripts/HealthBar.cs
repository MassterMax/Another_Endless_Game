using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //Creature parentCreature;
    Slider slider;

    public void Setup(float currentHealth, float maxHealth, bool friendly)
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

        if (friendly)
        {
            // todo this is very creepy
            slider.fillRect.gameObject.GetComponent<Image>().color = Color.green; 
            slider.GetComponentInParent<Canvas>().sortingOrder = 1;
        }
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
