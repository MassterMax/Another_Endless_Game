using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflecting : MonoBehaviour
{
    public const int REFLECTION_LOWER_BOUND = -999;

    [SerializeField] float transparency = 0.6f;
    [SerializeField] int reflectionLowerBound = REFLECTION_LOWER_BOUND;
    [SerializeField] int transparentSortingOrder;

    SpriteRenderer spriteRenderer;
    SpriteRenderer transparentSpriteRenderer;
    GameObject trancparentChild;
    bool isWorking = true;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        var spriteMask = gameObject.AddComponent<SpriteMask>();
        spriteMask.sprite = spriteRenderer.sprite;

        trancparentChild = Instantiate(Resources.Load("Prefabs/Reflections/Reflecting"), transform) as GameObject;
        transparentSpriteRenderer = trancparentChild.GetComponent<SpriteRenderer>();
        transparentSpriteRenderer.color = new Color(1, 1, 1, transparency);

        // I AM NOT SURE
        // I AM NOT SURE
        // I AM NOT SURE
        // borders should have order = -1000
        spriteRenderer.sortingOrder = reflectionLowerBound;
        transparentSpriteRenderer.sortingOrder = transparentSortingOrder;

        Turn(MenuController.ReflectionsTurnedOn);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWorking) return;
        if (transparentSpriteRenderer.sprite != spriteRenderer.sprite)
        {
            transparentSpriteRenderer.sprite = spriteRenderer.sprite;
        }

        //if (spriteRenderer.color.a != 1)
        //{
        //    transparentSpriteRenderer.color = new Color(1, 1, 1, transparency * spriteRenderer.color.a);
        //}
    }

    public void Turn(bool on)
    {
        isWorking = on;
        trancparentChild.SetActive(on);
    }
}
