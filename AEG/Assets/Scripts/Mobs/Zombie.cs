using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Creature
{
    [SerializeField] float speed;
    [SerializeField] float visibleField = 5;
    [SerializeField] float changeDirectonLimit = 2;
    float changeDirectionTimer = 0;
    float stayProbability = 0.7f;

    Animator animator;
    PlayerController player;
    Vector2 direction;

    void Start()
    {
        SetAttributes(10, 1, false);  // todo make one class with all start values
        SetBarStyle();
        player = FindObjectOfType<PlayerController>();
        changeDirectionTimer = changeDirectonLimit;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        HandleAnimation();
    }

    void Move()
    {
        if (player == null) return;

        if ((player.transform.position - transform.position).magnitude < visibleField)  // if zombie see the player
        {
            direction = (player.transform.position - transform.position);
            direction = direction.normalized;
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

    // todo maybe make all creatures to have Move() and HandleAnimation() ???
    void HandleAnimation()
    {
        animator.SetBool("isRun", direction.sqrMagnitude != 0);
    }
}
