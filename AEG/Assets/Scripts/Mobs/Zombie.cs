using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Monster
{
    [SerializeField] float visibleField = 5;
    [SerializeField] float changeDirectonLimit = 2;
    float changeDirectionTimer = 0;
    float stayProbability = 0.7f;

    protected override void Start()
    {
        base.Start();

        changeDirectionTimer = changeDirectonLimit;
    }

    protected override void Update()
    {
        base.Update();
    }

    // todo maybe make all creatures to have Move()
    protected override void Move()
    {
        if (Player == null) return;

        if (toPlayerDistance() < visibleField)  // if zombie see the player
        {
            direction = toPlayerVector().normalized;
        } else if (changeDirectionTimer >= changeDirectonLimit)
        {
            if (Random.value < stayProbability)  // zombie stays
            {
                direction = Vector2.zero;
            }
            else
            {
                direction = Random.insideUnitCircle.normalized;
            }

            changeDirectionTimer = 0;
        }

        changeDirectionTimer = Mathf.Min(changeDirectionTimer + Time.deltaTime, changeDirectonLimit);
        transform.Translate(direction * Time.deltaTime * speed);
    }
}
