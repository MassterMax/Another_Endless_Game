using System.Linq;
using UnityEngine;

public abstract class Monster : Creature, IFadestroyable
{
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    Vector2 moveDirection;
    Creature attackTarget;
    float deathDuration = 1f;
    float chaseTargetRadius = 2f;
    protected float attackInterval = 2f;
    protected float lastAttackTime;
    private float attackRange = 0.3f;
    protected virtual float AttackRange => attackRange;

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
        {
            Rotate();
            MeleeAttack();
        }
    }

    public void SetMonsterTarget(Creature target)
    {
        //Debug.Log(name + " set target: " + target.name);
        attackTarget = target;
    }

    protected virtual void MeleeAttack()
    {
        if (!CanMeleeAttack()) return;
        //Debug.LogWarning(name + " actually attacks!");
        Debug.Log(name + " make melee attack " + this.GetDamage() + " " + attackTarget.name);
        animator.SetTrigger("isAttacking");
        lastAttackTime = Time.time;
        // attackTarget.TakeDamage(this.GetDamage()); changed to handled with animation
    }

    // assign this in pre-last frame of attack animation
    protected void MakeDamageToTarget()
    {
        Debug.Log(name + " make damage to target " + this.GetDamage() + " " + attackTarget.name);
        attackTarget.TakeDamage(this.GetDamage());
    }

    // protected virtual void MeleeAttack(float delay)
    // {
    //     if (!CanMeleeAttack()) return;
    //     //Debug.LogWarning(name + " actually attacks with dilation! " + Time.time);
    //     animator.SetTrigger("isAttacking");
    //     lastAttackTime = Time.time;
    //     attackTarget.TakeDamage(GetDamage(), delay);
    // }

    protected bool TargetInAttackRange()
    {
        // Debug.Log(name + " target is " + attackTarget);
        // Debug.Log(attackTarget is null);
        // Debug.Log(attackTarget == null);

        if (attackTarget == null) return false;
        //Debug.Log(GetSqrDistanceBetweenAttackPointAndTarget());
        return GetSqrDistanceBetweenAttackPointAndTarget() <= AttackRange * AttackRange;
    }
    protected virtual bool CanMeleeAttack()
    {
        if (!TargetInAttackRange())
            return false;
        return Time.time - lastAttackTime >= attackInterval;
    }

    protected virtual float GetSqrDistanceBetweenAttackPointAndTarget()
    {
        return ToTargetDistance(true);
    }

    public virtual bool ShouldChaseTarget()
    {
        //Debug.Log(name + " should chase target?");
        float toTargetDistance = ToTargetDistance();
        if (attackTarget == null) return false;
        //Debug.Log(name + " should chase target: " + attackTarget);
        return toTargetDistance <= chaseTargetRadius;
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
            var collider = GetComponent<Collider2D>();
            if (collider != null)
                collider.enabled = false;
        }
        else
        {
            Debug.LogWarning("monster should have animator!");
            base.OnDeath();
        }
    }

    protected override void Move()
    {
        if (ToTargetDistance(true) >= AttackRange * AttackRange)
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
        return attackTarget.transform.position;
    }

    protected Vector2 ToTargetVector()
    {
        if (!HasTarget()) return Vector2.zero;
        return attackTarget.transform.position - transform.position;
    }

    protected float ToTargetDistance(bool sqrMagnitude = false)
    {
        if (sqrMagnitude) return ToTargetVector().sqrMagnitude;
        return ToTargetVector().magnitude;
    }

    // TODO
    // remove this cheating
    protected void BoostSpeed(float value)
    {
        moveDirection *= value;
    }

    protected virtual void DelayedDestroy(float duration)
    {
        // Debug.LogWarning(duration + " duration of death");
        var coroutine = ((IFadestroyable)this).FadingDestroy(spriteRenderer, null, 0, duration, false);
        StartCoroutine(coroutine);
    }
}
