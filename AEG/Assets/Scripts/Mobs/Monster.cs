using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Creature
{
    protected Animator animator;

    protected float speed;

    private PlayerController player;

    public PlayerController Player { get => player; set => player = value; }

    protected Vector2 direction;  // todo rename to moveDirection

    protected override void Move()
    {
        throw new System.NotImplementedException();
    }

    public void SetAttributes(float health, float maxHealth, float damage, float speed)
    {
        base.SetAttributes(health, maxHealth, damage, false);
        this.speed = speed;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        SetBarStyle();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    internal void ChasePlayer()
    {

    }

    protected override void HandleAnimation()
    {
        if (animator == null) return;
        animator.SetBool("isRun", direction.sqrMagnitude != 0);
    }
}
