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
    bool isWorking = true;

    Vector3 prevPos;

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

        prevPos = reflection.transform.localPosition;
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

        // Set correct rotation
        //reflection.transform.localEulerAngles = Vector3.forward * ReflectionAngle();
        //reflection.transform.eulerAngles = Vector3.forward * ReflectionAngle();

        // Set correct vertical position
        //Debug.Log(gameObject.name + " : " + pseudoYOffset + " " + yOffset);
        //Debug.Log("original pos is " + gameObject.transform.position);

        if (pseudoYOffset != 0)
            reflection.transform.position = (Vector2)gameObject.transform.position + downVector() * 2 * pseudoYOffset;
        else
            reflection.transform.position = (Vector2)gameObject.transform.position + downVector() * yOffset * Mathf.Cos(gameObject.transform.rotation.z);
        // Debug.Log("psudo offset is " + 2 * pseudoYOffset + " while cosine offset is " + yOffset * Mathf.Cos(gameObject.transform.rotation.z));
        //Debug.Log("reflection pos is " + reflection.transform.position);

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
        //Debug.LogWarning("reflex axis is " + reflectAxis);
        //float relativeAngle = Utils.VectorToAngle(reflectAxis);
        //float currentAngle = gameObject.transform.eulerAngles.z;
        // float a = currentAngle * Mathf.Deg2Rad;
        // float b = relativeAngle * Mathf.Deg2Rad;

        // if (gameObject.name.Equals("Spear"))
        // {
        //     Debug.LogWarning(gameObject.name + ": ");
        //     Debug.Log("current angle: " + currentAngle);
        //     Debug.Log("axis angle: " + relativeAngle);
        //     Debug.LogWarning("absolute reflection angle: " + -Mathf.Atan(Mathf.Tan(a) - 2 * Mathf.Tan(b)) * Mathf.Rad2Deg);
        // }

        // return 180 - Mathf.Atan(Mathf.Tan(a) - 2 * Mathf.Tan(b)) * Mathf.Rad2Deg;
        //float angle = 180 - 2 * currentAngle + 2 * relativeAngle;

        if (reflectAxis == Vector2.right)
        {
            reflection.transform.localEulerAngles = Vector3.forward * (180 - 2 * gameObject.transform.eulerAngles.z);
            return;
        }
        Vector3 newPos = reflection.transform.localPosition;
        float angle = Utils.VectorToAngle(newPos - prevPos) - 90;
        prevPos = newPos;
        reflection.transform.eulerAngles = Vector3.forward * angle;
    }

    private void Destroy()
    {
        Destroy(reflection);
    }
}
