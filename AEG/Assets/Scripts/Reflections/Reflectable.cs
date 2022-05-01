using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflectable : MonoBehaviour, IBlinkable
{
    SpriteRenderer spriteRenderer;
    SpriteRenderer reflectionSpriteRenderer;
    GameObject reflection;
    float yOffset;

    float pseudoYOffset = 0f;
    [SerializeField] Vector2 reflectAxis;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        reflection = Instantiate(Resources.Load("Prefabs/Reflections/Reflectable"), transform) as GameObject;
        //reflection.transform.localPosition = Vector3.up * yOffset;

        reflectionSpriteRenderer = reflection.GetComponent<SpriteRenderer>();
        reflectionSpriteRenderer.flipX = true;
        reflectionSpriteRenderer.flipY = false;
        reflectionSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        
        reflectionSpriteRenderer.sortingOrder = Reflecting.REFLECTION_LOWER_BOUND + spriteRenderer.sortingOrder; // hmmmmmm

        yOffset = spriteRenderer.bounds.size.y;

        ResetReflectAxis();
        //SetPseudoYOffset(transform.localPosition.y);
        //Debug.Log(gameObject.name + " : " + pseudoYOffset + " " + spriteRenderer.bounds);
        //Debug.Log(spriteRenderer.bounds.center + " " + spriteRenderer.bounds.extents + " " + spriteRenderer.bounds.size);
        //Debug.Log(Camera.main.ScreenToWorldPoint(spriteRenderer.sprite.pivot) + " " + spriteRenderer.sprite.pivot);
        //float pixel2units = spriteRenderer.sprite.rect.width / spriteRenderer.sprite.bounds.size.x;


        //Debug.Log(spriteRenderer.sprite.pixelsPerUnit);
        //Debug.Log(transform.position);
        //Debug.Log(spriteRenderer.sprite.pivot.y);
        //Debug.Log(spriteRenderer.sprite.pivot.y / spriteRenderer.sprite.pixelsPerUnit * 2);
        //Debug.Log(yOffset);

        // PROPER OFFSET: (not just height of sprite)
        yOffset = spriteRenderer.sprite.pivot.y / spriteRenderer.sprite.pixelsPerUnit * 2;
    }

    public void BlinkReflection(int times, float dilation)
    {
        StartCoroutine(((IBlinkable)this).Blinking(reflectionSpriteRenderer, times, dilation));
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
        reflection.transform.localEulerAngles = Vector3.forward * ReflectionAngle();

        // Set correct vertical position
        //Debug.Log(gameObject.name + " : " + pseudoYOffset + " " + yOffset);
        reflection.transform.position = (Vector2)gameObject.transform.position +
            downVector() * (2 * pseudoYOffset + yOffset * Mathf.Cos(gameObject.transform.rotation.z));
    }

    private Vector2 downVector()
    {
        return Vector2.down;

        //return Utils.RotateVector(reflectAxis, -90);
    }

    public void SetPseudoYOffset(float value)
    {
        pseudoYOffset = value;
    }

    public void ResetReflectAxis()
    {
        reflectAxis = Vector2.right;
    }

    public void SetReflectAxis(Vector2 newAxis)
    {
        reflectAxis = newAxis;
    }

    private float ReflectionAngle()
    {
        float relativeAngle = Utils.VectorToAngle(reflectAxis);
        float currentAngle = gameObject.transform.eulerAngles.z;

        //Debug.LogWarning(gameObject.name + " current n relative angles:");
        //Debug.Log(currentAngle);
        //Debug.Log(relativeAngle);

        return 180 - 2 * currentAngle + 2 * relativeAngle;
    }
}
