using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Monster
{
    [SerializeField] float visibleField = 5;
    [SerializeField] float changeDirectonLimit = 2;
    float changeDirectionTimer = 0;
    float stayProbability = 0.2f;

    protected override void Start()
    {
        base.Start();

        changeDirectionTimer = changeDirectonLimit;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Move()
    {
        if (!HasTarget()) return;

        if (ToTargetDistance() < visibleField)  // if zombie sees the target todo remove
        {
            if (TargetInAttackRange())
                SetMoveDirection();
            else
                SetMoveDirection(ToTargetVector());
        }
        else if (changeDirectionTimer >= changeDirectonLimit)
        {
            if (Random.value < stayProbability)  // zombie stays
            {
                SetMoveDirection();  // reset
            }
            else
            {
                // with 20% prob random direction or target
                if (Random.value < 0.2)
                    SetMoveDirection(Random.insideUnitCircle.normalized);
                else
                    SetMoveDirection(ToTargetVector());
            }

            changeDirectionTimer = 0;
        }

        changeDirectionTimer = Mathf.Min(changeDirectionTimer + Time.deltaTime, changeDirectonLimit);
        base.Move();
    }
}
