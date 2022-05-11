using UnityEngine;
using System.Linq;

public abstract class FriendlyCreature : Creature
{
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    protected Vector2 direction;  // todo rename to moveDirection

    protected virtual void Start()
    {
        SetBarStyle("green");
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();
        if (!isDead)
            Rotate();
    }

    public void SetAttributes(float health, float maxHealth, float damage, float speed)
    {
        base.SetAttributes(health, maxHealth, damage, speed, true);
    }

    protected override void HandleAnimation()
    {
        if (animator == null) return;
        animator.SetBool("isRun", direction.sqrMagnitude != 0);
    }

    protected override void OnDeath()
    {
        //base.OnDeath();

        if (animator != null)
        {
            isDead = true;
            UIOff();
            animator.SetBool("isDead", true);

            AnimationClip clip = animator.runtimeAnimatorController.animationClips.Where(clip => clip.name.Equals("death")).FirstOrDefault();
            float length = 1f;
            if (clip != null)
            {
                length = clip.length;
            }
            DelayedDestroy(length);
            this.enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            base.OnDeath();
        }
    }

    protected virtual void Rotate()
    {
        // Vector2 dir = toPlayerVector();

        // if (spriteRenderer.flipX != Mathf.Sign(dir.x) < 0)
        // {
        //     spriteRenderer.flipX = !spriteRenderer.flipX;
        // }

        if (spriteRenderer.flipX != Mathf.Sign(direction.x) < 0)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    // protected Vector2 toPlayerVector()
    // {
    //     if (player == null) return Vector2.zero;
    //     return player.transform.position - transform.position;
    // }

    protected void DelayedDestroy(float duration)
    {
        Debug.LogWarning(duration + " duration of death");
        var coroutine = ((IFadestroyable)this).FadingDestroy(GetComponent<SpriteRenderer>(), 0, duration, false);
        StartCoroutine(coroutine);
    }
}
