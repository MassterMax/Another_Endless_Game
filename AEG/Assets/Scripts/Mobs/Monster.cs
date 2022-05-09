using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Monster : Creature, IFadestroyable
{
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    private PlayerController player;

    public PlayerController Player { get => player; set => player = value; }

    protected Vector2 direction;  // todo rename to moveDirection

    protected virtual void Start()
    {
        SetBarStyle();
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
        base.SetAttributes(health, maxHealth, damage, speed, false);
    }

    // todo maybe implement
    protected override void Move()
    {
        throw new System.NotImplementedException();
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

            //Debug.Log("next: " + animator.GetNextAnimatorStateInfo(0).length);
            //Debug.Log("current: " + animator.GetCurrentAnimatorStateInfo(0).length);

            AnimationClip clip = animator.runtimeAnimatorController.animationClips.Where(clip=>clip.name.Equals("death")).FirstOrDefault();
            float length = 1f;
            if (clip != null)
            {
                length = clip.length;
            }
            Debug.Log("LENGTH IS " + length);
            DelayedDestroy(length);
            //animator.Sta
            //animator.Get
            Debug.LogWarning("death with animation");
        } else
        {
            base.OnDeath();
        }
    }

    protected virtual void Rotate()
    {
        Vector2 dir = toPlayerVector();

        if (spriteRenderer.flipX != Mathf.Sign(dir.x) < 0)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    protected float toPlayerDistance()
    {
        return toPlayerVector().magnitude;
    }

    protected Vector2 toPlayerVector()
    {
        if (player == null) return Vector2.zero;
        return player.transform.position - transform.position;
    }

    protected void DelayedDestroy(float duration)
    {
        Debug.LogWarning(duration + " duration of death");
        var coroutine = ((IFadestroyable)this).FadingDestroy(GetComponent<SpriteRenderer>(), 0, duration, false);
        StartCoroutine(coroutine);
    }
}
