using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Launchable
{
    Reflectable reflectable;

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
    }

    public void OnPickup(float y)
    {
        //reflectable.ResetReflectAxis();
        reflectable.SetPseudoYOffset(-y/2);
    }

    public override void LandInTarget(Transform target)
    {
        base.LandInTarget(target);
        OnPickup(transform.localPosition.y);

        StartCoroutine(FadingDestroy());
    }

    //protected override void OnLanding()
    //{
    //    // reflectable.ResetReflectAxis();
    //    reflectable.SetPseudoYOffset(0);
    //}

    public override float GetDamage()
    {
        if (inFlight)
        {
            return base.GetDamage();
        }
        // landed spear deal 0 damage
        return 0;
    }
}
