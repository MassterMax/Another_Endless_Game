using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Monster : Creature, IFadestroyable
{
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    Vector2 moveDirection;
    Transform attackTarget;
    float deathDuration = 1f;
    float chaseTargetRadius = 5f;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator != null)
        {
            AnimationClip clip = animator.runtimeAnimatorController.animationClips.Where(clip => clip.name.Equals("death")).FirstOrDefault();
            if (clip != null)
            {
                deathDuration = clip.length;
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if (!isDead)
            Rotate();
    }

    public void SetMonsterTarget(Transform target)
    {
        attackTarget = target;
    }

    public virtual bool ShouldChaseTarget()
    {
        if (attackTarget is null) return false;
        return ToTargetDistance() <= chaseTargetRadius;
    }

    protected override void HandleAnimation()
    {
        if (animator == null) return;
        animator.SetBool("isRun", moveDirection.sqrMagnitude != 0);
    }

    protected override void OnDeath()
    {
        if (animator != null)
        {
            isDead = true;
            UIOff();
            animator.SetBool("isDead", true);
            DelayedDestroy(deathDuration);

            this.enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            base.OnDeath();
        }
    }

    protected override void Move()
    {
        transform.Translate(moveDirection * Time.deltaTime * Speed);
    }

    protected void SetMoveDirection()
    {
        moveDirection = Vector2.zero;
    }

    protected void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    protected virtual void Rotate()
    {
        // pass
        if (moveDirection.x == 0)
            return;

        if (spriteRenderer.flipX != Mathf.Sign(moveDirection.x) < 0)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    protected bool HasTarget()
    {
        return attackTarget != null;
    }

    protected Vector2 TargetPosition()
    {
        if (!HasTarget()) return Vector2.zero;
        return attackTarget.position;
    }

    protected Vector2 ToTargetVector()
    {
        if (!HasTarget()) return Vector2.zero;
        return attackTarget.position - transform.position;
    }

    protected float ToTargetDistance()
    {
        return ToTargetVector().magnitude;
    }

    // TODO
    // remove this cheating
    protected void BoostSpeed(float value)
    {
        moveDirection *= value;
    }

    protected void DelayedDestroy(float duration)
    {
        // Debug.LogWarning(duration + " duration of death");
        var coroutine = ((IFadestroyable)this).FadingDestroy(spriteRenderer, 0, duration, false);
        StartCoroutine(coroutine);
    }
}
