using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Creature
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
}
