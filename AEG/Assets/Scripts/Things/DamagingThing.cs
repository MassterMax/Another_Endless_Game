using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamagingThing : MonoBehaviour, IDamaging
{
    private float damage;

    public float Damage { get => damage; set => damage = value; }

    public virtual float GetDamage()
    {
        return damage;
    }

    protected IEnumerator FadingDestroy(float time = 1f)
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) yield break;

        float start = Time.time;
        var colorStep = new Color(0, 0, 0, spriteRenderer.color.a);
        while (Time.time - start < time)
        {
            spriteRenderer.color -= colorStep * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        Destroy(this);
    }
}
