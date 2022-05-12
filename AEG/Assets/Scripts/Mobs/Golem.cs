using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Monster, IDelayable
{
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 0.2f;

    float preparedToFightRadius;
    bool isFighting;

    float changeDirectonLimit = 2;
    float changeDirectionTimer = 0;

    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.R))
        {
            PrepareToFight();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            Attack();
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            StopFight();
        }
    }

    protected void PrepareToFight()
    {
        animator.SetBool("isFighting", true);
        isFighting = true;
    }

    protected void StopFight()
    {
        animator.SetBool("isFighting", false);

        // todo remove hard code 0.5f -> get animation instead
        StartCoroutine(((IDelayable)this).ExecuteAfterDelay(0.5f, () => { isFighting = false; }));
    }

    protected void Attack()
    {
        animator.SetTrigger("isAttacking");

        // Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        // foreach (var enemy in hitEnemies)
        // {
        //     // if (enemy.)
        // }
        float distanceBetweenAttackPointAndTarget = (TargetPosition() - (Vector2)attackPoint.position).sqrMagnitude;
        if (distanceBetweenAttackPointAndTarget <= attackRange * attackRange)
        {
            Debug.Log(name + " make damage to target!!");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }


    protected override void Move()
    {
        if (isFighting)
        {
            SetMoveDirection();
        }
        else if (changeDirectionTimer >= changeDirectonLimit)
        {
            if (Random.value < 0.5f)
            {
                SetMoveDirection();
            }
            else
            {
                SetMoveDirection(Random.insideUnitCircle.normalized);
            }
            changeDirectionTimer = 0;
        }

        changeDirectionTimer = Mathf.Min(changeDirectionTimer + Time.deltaTime, changeDirectonLimit);

        base.Move();
    }
}
