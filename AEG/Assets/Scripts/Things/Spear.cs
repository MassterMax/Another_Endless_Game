using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Launchable, IFadestroyable
{
    Reflectable reflectable;
    private bool inSkeletonHands = true;

    void Start()
    {
        Damage = 3f;  // todo remake
        reflectable = GetComponent<Reflectable>();
    }

    public void Remove()
    {

    }

    void Update()
    {
        SetPseudoY();
    }

    void SetPseudoY()
    {
        if (inFlight && reflectable != null)
        {
            reflectable.SetPseudoYOffset(currentY);

        }
    }

    public override void LaunchObject(Vector2 destination, float velocity)
    {
        base.LaunchObject(destination, velocity);
        reflectable.SetReflectAxis(direction);
        inSkeletonHands = false;
    }

    public void OnPickup(float y)
    {
        reflectable.ResetReflectAxis();
        reflectable.SetPseudoYOffset(-y / 2);
        inSkeletonHands = true;
    }

    public override bool LandInTarget(Transform target, float targetHeight = 0)
    {
        if (!base.LandInTarget(target))
        {
            return false;
        }

        float distance = transform.localPosition.magnitude; // + targetHeight;
        //reflectable.ResetReflectAxis();
        reflectable.SetPseudoYOffset(-distance / 2);
        var coroutine = ((IFadestroyable)this).FadingDestroy(GetComponent<SpriteRenderer>());
        StartCoroutine(coroutine);
        return true;
    }

    //protected override void OnLanding()
    //{
    //    // reflectable.ResetReflectAxis();
    //    reflectable.SetPseudoYOffset(0);
    //}

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
