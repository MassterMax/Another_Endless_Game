using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Launchable, IFadestroyable
{
    Reflectable reflectable;
    private bool inSkeletonHands = true;

    private float height;

    void Start()
    {
        Damage = 3f;  // todo remake
        reflectable = GetComponent<Reflectable>();
        height = GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    public void Remove()
    {

    }

    void FixedUpdate()
    {
        SetPseudoY();
    }

    void SetPseudoY()
    {
        if (inFlight && reflectable != null)
        {
            //Debug.Log("set pseudo y on flight");
            reflectable.SetPseudoYOffset(currentY + height);
        }
    }

    public override void LaunchObject(Vector2 destination, float velocity)
    {
        base.LaunchObject(destination, velocity);
        Debug.LogWarning("setting reflect asix " + direction);
        reflectable.SetReflectAxis(direction);
        inSkeletonHands = false;
    }

    public void OnPickup(float y)
    {
        Debug.Log("on pickup y is " + y);
        reflectable.ResetReflectAxis();
        reflectable.SetPseudoYOffset(y);
        inSkeletonHands = true;
        reflectable.shouldHandleReflectionAngle = true;
    }

    public override bool LandInTarget(Transform target)
    {
        if (!base.LandInTarget(target))
        {
            return false;
        }

        //float distance = transform.localPosition.magnitude; // + targetHeight;
        float distance = transform.localPosition.y + transform.parent.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        //reflectable.ResetReflectAxis();
        reflectable.SetPseudoYOffset(distance);
        // TODO RETURN THIS AFTER DEBUG
        var coroutine = ((IFadestroyable)this).FadingDestroy(GetComponent<SpriteRenderer>());
        StartCoroutine(coroutine);
        reflectable.shouldHandleReflectionAngle = false;

        return true;
    }

    protected override void OnLanding()
    {
        reflectable.shouldHandleReflectionAngle = false;
        //reflectable.SetPseudoYOffset(0);
    }

    public override float GetDamage()
    {
        if (inFlight || inSkeletonHands)
        {
            return base.GetDamage();
        }
        // landed spear deal 0 damage
        return 0;
    }
}
