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
    Vector2 reflectAxis;
    bool isWorking = true;

    [HideInInspector] public bool shouldHandleReflectionAngle = true;

    Vector3 prevPos;
    float skipedTime = 1;
    float rotationAngle;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        reflection = Instantiate(Resources.Load("Prefabs/Reflections/Reflectable"), transform) as GameObject;
        //reflection.transform.localPosition = Vector3.up * yOffset;

        reflectionSpriteRenderer = reflection.GetComponent<SpriteRenderer>();
        reflectionSpriteRenderer.flipX = true;
        reflectionSpriteRenderer.flipY = false;
        reflectionSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        reflectionSpriteRenderer.sortingOrder = Reflecting.REFLECTION_LOWER_BOUND + spriteRenderer.sortingOrder; // hmmmmmm

        //yOffset = spriteRenderer.bounds.size.y;

        prevPos = reflection.transform.position;
        ResetReflectAxis();

        yOffset = spriteRenderer.sprite.pivot.y / spriteRenderer.sprite.pixelsPerUnit * 2;

        Turn(MenuController.ReflectionsTurnedOn);
    }

    public void BlinkReflection(int times, float dilation)
    {
        StartCoroutine(((IBlinkable)this).Blinking(reflectionSpriteRenderer, times, dilation));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWorking) return;

        if (reflectionSpriteRenderer.sprite != spriteRenderer.sprite)
        {
            reflectionSpriteRenderer.sprite = spriteRenderer.sprite;
        }

        if (reflectionSpriteRenderer.flipX == spriteRenderer.flipX)
        {
            reflectionSpriteRenderer.flipX = !spriteRenderer.flipX;
        }

        if (reflectionSpriteRenderer.color != spriteRenderer.color)
        {
            reflectionSpriteRenderer.color = spriteRenderer.color;
        }

        // reflection.transform.position = (Vector2)gameObject.transform.position + downVector() * yOffset * Mathf.Cos(gameObject.transform.rotation.z) + downVector() * 2 * pseudoYOffset;

        if (pseudoYOffset != 0)
            reflection.transform.position = (Vector2)gameObject.transform.position + downVector() * 2 * pseudoYOffset;
        else
        {
            reflection.transform.position = (Vector2)gameObject.transform.position + downVector() * yOffset * Mathf.Cos(gameObject.transform.rotation.z * Mathf.Deg2Rad);
            if (gameObject.name.Equals("Spear"))
            {
                Debug.Log("new spear pos is " + reflection.transform.position);
            }
        }
        SetReflectionAngle();
    }

    public void Turn(bool on)
    {
        isWorking = on;
        reflection.SetActive(on);
    }

    private Vector2 downVector()
    {
        return Vector2.down;

        //return Utils.RotateVector(reflectAxis, -90);
    }

    public void SetPseudoYOffset(float value)
    {
        //Debug.LogWarning("set pseudo offset: " + value);
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

    private void SetReflectionAngle()
    {
        if (!shouldHandleReflectionAngle) return;

        if (reflectAxis == Vector2.right)
        {
            reflection.transform.localEulerAngles = Vector3.forward * (180 - 2 * gameObject.transform.eulerAngles.z);
            return;
        }

        if (skipedTime < Time.deltaTime)
        {
            skipedTime += Time.deltaTime;
        }
        else
        {
            Vector3 newPos = reflection.transform.position;

            // skip
            if (newPos.x == prevPos.x || newPos.y == prevPos.y)
            {
                //Debug.Log("skip");
                return;
            }

            // Debug.LogWarning("newPos " + newPos);
            // Debug.Log("vector is " + (newPos - prevPos).normalized);
            rotationAngle = Utils.VectorToAngle(newPos - prevPos) - 90;
            //Debug.Log("angle is " + (angle + 90));
            prevPos = newPos;
            skipedTime = 0;
        }

        reflection.transform.eulerAngles = Vector3.forward * rotationAngle;
        return;
    }

    private void Destroy()
    {
        Destroy(reflection);
    }
}
