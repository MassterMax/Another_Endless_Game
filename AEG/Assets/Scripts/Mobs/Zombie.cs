using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Creature
{
    [SerializeField] float speed;
    [SerializeField] float visibleField = 5;
    [SerializeField] float changeDirectonLimit = 2;
    float changeDirectionTimer = 0;

    PlayerController player;
    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        SetAttributes(10, 1);
        player = FindObjectOfType<PlayerController>();
        changeDirectionTimer = changeDirectonLimit;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (player == null) return;

        if ((player.transform.position - transform.position).magnitude < visibleField)
        {
            direction = (player.transform.position - transform.position);
            direction = direction.normalized;
        } else if (changeDirectionTimer >= changeDirectonLimit)
        {
            direction = Random.insideUnitCircle.normalized;
            changeDirectionTimer = 0;
        }

        changeDirectionTimer = Mathf.Min(changeDirectionTimer + Time.deltaTime, changeDirectonLimit);
        transform.Translate(direction * Time.deltaTime * speed);
    }
}
