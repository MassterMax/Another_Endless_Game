using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Monster
{
    float changeDirectonLimit = 2;
    float changeDirectionTimer = 0;

    // todo remove!!
    // void Awake()
    // {
    //     SetAttributes(2, 2, 1, 0.3f, true);
    // }

    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Move()
    {
        if (changeDirectionTimer >= changeDirectonLimit)
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
