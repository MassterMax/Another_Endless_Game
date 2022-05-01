using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflecting : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    SpriteRenderer transparentSpriteRenderer;
    GameObject trancparentChild;
    float transparency = 0.5f;

    public const int REFLECTION_LOWER_BOUND = -999;

    void Start()
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
        spriteRenderer.sortingOrder = REFLECTION_LOWER_BOUND;
        transparentSpriteRenderer.sortingOrder = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (transparentSpriteRenderer.sprite != spriteRenderer.sprite)
        {
            transparentSpriteRenderer.sprite = spriteRenderer.sprite;
        }
    }
}
