using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflecting : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    SpriteRenderer transparentSpriteRenderer;
    GameObject trancparentChild;
    float transparency = 0.5f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        var spriteMask = gameObject.AddComponent<SpriteMask>();
        spriteMask.sprite = spriteRenderer.sprite;

        trancparentChild = Instantiate(Resources.Load("Prefabs/Reflections/Reflecting"), transform) as GameObject;
        transparentSpriteRenderer = trancparentChild.GetComponent<SpriteRenderer>();
        transparentSpriteRenderer.color = new Color(1, 1, 1, transparency);

        spriteRenderer.sortingOrder = 0;
        transparentSpriteRenderer.sortingOrder = 2;
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
