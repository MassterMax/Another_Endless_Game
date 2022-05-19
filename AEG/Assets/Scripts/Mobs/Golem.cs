using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Monster
{
    float preparedToFightRadius;
    bool isInFightPosition;
    bool canMoveInPostition = true;
    bool readyToAttack;
    float changeDirectonLimit = 2;
    float changeDirectionTimer = 0;

    protected override float AttackRange => base.AttackRange + 0.1f;

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected void PrepareToFight()
    {
        animator.SetBool("isFighting", true);
        // todo remove hard code 0.5f
        var coroutine = ((IDelayable)this).ExecuteAfterDelay(0.5f, () => { Debug.LogWarning("attack set to true " + Time.time); readyToAttack = true; });
        StartCoroutine(coroutine);
        isInFightPosition = true;
        canMoveInPostition = false;
    }

    protected void StopFight()
    {
        animator.SetBool("isFighting", false);
        isInFightPosition = false;
        readyToAttack = false;
        // todo remove hard code 0.5f -> get animation instead
        var coroutine = ((IDelayable)this).ExecuteAfterDelay(0.5f, () => { if (!isInFightPosition) canMoveInPostition = true; });
        StartCoroutine(coroutine);
    }

    protected override void MeleeAttack()
    {
        // Debug.Log(TargetInAttackRange() + " " + isInFightPosition + " " + canMoveInPostition);
        if (TargetInAttackRange() && !isInFightPosition)
        {
            Debug.LogWarning("golem trying to prepare to fight " + Time.time);
            PrepareToFight(); // if not in fight mode but can attack
        }
        else if (!TargetInAttackRange() && isInFightPosition)
        {
            Debug.LogWarning("golem trying to stop to fight");
            StopFight(); // if in fight mode but can not melee attack
        }
        else if (readyToAttack)
        {
            // todo remove hardcode
            base.MeleeAttack();
        }
        // animator.SetTrigger("isAttacking");

        // Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        // foreach (var enemy in hitEnemies)
        // {
        //     // if (enemy.)
        // }

    }

    // protected override float GetSqrDistanceBetweenAttackPointAndTarget()
    // {
    //     return (TargetPosition() - (Vector2)attackPoint.position).sqrMagnitude;
    // }


    protected override void Move()
    {
        if (!canMoveInPostition)
        {
            SetMoveDirection();
        }
        else
        {
            SetMoveDirection(ToTargetVector());
            // if (Random.value < 0.5f)
            // {
            //     SetMoveDirection();
            // }
            // else
            // {
            //     SetMoveDirection(ToTargetVector());
            // }
            //changeDirectionTimer = 0;
        }

        //changeDirectionTimer = Mathf.Min(changeDirectionTimer + Time.deltaTime, changeDirectonLimit);

        base.Move();
    }

    // todo generalize or remove rigidbody to new thing
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damaging = collision.gameObject.GetComponent<IDamaging>();
        if (!(damaging is Launchable)) return;
        if (damaging == null || damaging.GetDamage() == 0) return;

        if (((Launchable)damaging).LandInTarget(transform))
        {
            TakeDamage(damaging.GetDamage());
        }
    }
}
