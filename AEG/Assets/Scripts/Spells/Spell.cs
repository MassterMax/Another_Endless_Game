using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour, IFadestroyable
{
    public abstract void CastSpell(Vector2 start, Vector2 end, Vector2 center);

    protected void DelayedDestroy(float dilation, float duration=1f, bool withResize = true)
    {
        var coroutine = ((IFadestroyable)this).FadingDestroy(GetComponent<SpriteRenderer>(), dilation, duration, withResize);
        StartCoroutine(coroutine);
    }
}
