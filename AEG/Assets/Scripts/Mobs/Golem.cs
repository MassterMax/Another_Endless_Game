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

    protected override void Start()
    {
        base.Start();
        attackRange = 0.4f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     PrepareToFight();
        // }
        // else if (Input.GetKeyDown(KeyCode.T))
        // {
        //     MeleeAttack();
        // }
        // else if (Input.GetKeyDown(KeyCode.Y))
        // {
        //     StopFight();
        // }
        // Debug.LogWarning(Time.time + " current clip is " + animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
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
}
