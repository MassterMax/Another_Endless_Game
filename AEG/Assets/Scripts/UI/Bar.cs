using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    Slider slider;
    Dictionary<string, Color> colorMapping = new Dictionary<string, Color> { { "red", Color.red }, { "green", Color.green } };

    public void Setup(float currentValue, float maxValue)
    {
        slider = GetComponent<Slider>();
        slider.maxValue = maxValue;
        slider.value = currentValue;
    }

    public void SetStyle(string colorName = "red", int sorterOrder = 11)
    {
        // todo remove:
        sorterOrder = 1001;
        slider.fillRect.gameObject.GetComponent<Image>().color = GetColorByName(colorName);
        slider.GetComponentInParent<Canvas>().sortingOrder = sorterOrder;
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }

    private Color GetColorByName(string name)
    {
        if (!colorMapping.ContainsKey(name))
        {
            Debug.LogError("no such color: " + name);
            return Color.black;
        }

        return colorMapping[name];
    }
}
