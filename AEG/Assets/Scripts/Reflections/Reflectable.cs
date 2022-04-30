using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflectable : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    SpriteRenderer reflectionSpriteRenderer;
    GameObject reflection;
    [SerializeField] float yOffset = 0.5f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        reflection = Instantiate(Resources.Load("Prefabs/Reflections/Reflectable"), transform) as GameObject;
        reflection.transform.localPosition = Vector3.up * yOffset;

        reflectionSpriteRenderer = reflection.GetComponent<SpriteRenderer>();
        reflectionSpriteRenderer.flipX = true;
        reflectionSpriteRenderer.flipY = false;
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

        if (reflectionSpriteRenderer.flipX == spriteRenderer.flipX)
        {
            reflectionSpriteRenderer.flipX = !spriteRenderer.flipX;
        }

        // Set correct rotation
        reflection.transform.localEulerAngles = Vector3.forward * (-180 - 2 * gameObject.transform.eulerAngles.z);

        // Set correct vertical position
        reflection.transform.position = gameObject.transform.position + Vector3.down * yOffset; // * Mathf.Cos(gameObject.transform.rotation.z);
    }
}
