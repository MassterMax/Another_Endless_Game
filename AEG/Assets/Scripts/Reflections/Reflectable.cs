using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflectable : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    SpriteRenderer reflectionSpriteRenderer;
    GameObject reflection;
    public float yOffset = -0.5f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        reflection = Instantiate(Resources.Load("Prefabs/Reflections/Reflectable"), transform) as GameObject;
        reflection.transform.localPosition = Vector3.up * yOffset;

        reflectionSpriteRenderer = reflection.GetComponent<SpriteRenderer>();
        reflectionSpriteRenderer.flipY = true;
        reflectionSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        
        reflectionSpriteRenderer.sortingOrder = 1; // todo
    }

    // Update is called once per frame
    void Update()
    {
        if (reflectionSpriteRenderer.sprite != spriteRenderer.sprite)
        {
            reflectionSpriteRenderer.sprite = spriteRenderer.sprite;
        }

        if (reflectionSpriteRenderer.flipX != spriteRenderer.flipX)
        {
            reflectionSpriteRenderer.flipX = spriteRenderer.flipX;
        }
    }
}
