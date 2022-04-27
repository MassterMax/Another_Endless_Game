using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Monster
{
    [SerializeField] float throwingRadius = 10f;
    [SerializeField] float fightingRadius = 5f;
    [SerializeField] float hurryBoost = 1.5f;
    LaunchItemManager manager;

    // todo make timer because I zadolbalsya
    float throwDilation = .5f;
    float preparedToThrowTime = 0f;

    float spearPreferedCoef = 2f;  // if tpSpearDistance < toPlayerDistance * coef; => prefer to obtain spear

    GameObject spear;
    bool withSpear = true;
    bool preparing = false;
    Vector2 defaultSpearPos;
    Vector2 lastSpearTarget;

    public override float Damage { get => withSpear ? base.Damage * 3 : base.Damage; }  
    // todo spear has own collider and maybe I should separate it as entity

    protected override void Start()
    {
        base.Start();

        spear = GetComponentInChildren<Spear>().gameObject;
        manager = FindObjectOfType<LaunchItemManager>();  // todo maybe remove
        defaultSpearPos = spear.transform.localPosition;
    }

    protected override void Update()
    {
        base.Update();
        HandleSpear();
    }

    protected override void Move()
    {
        if (Player == null) return;
        if (preparing) return;

        float playerDistance = toPlayerDistance();

        if (!withSpear)
        {
            // go for player only if spear is far away
            if (toSpearDistance() <= spearPreferedCoef * playerDistance) direction = toSpearVector().normalized;
            else direction = toPlayerVector().normalized;

            direction *= hurryBoost;
        }
        else
        {
            // move only if out of throwing range or too late to throw... >;-(
            if (playerDistance <= fightingRadius || playerDistance > throwingRadius)
            {
                direction = toPlayerVector().normalized;
                preparedToThrowTime = Time.time;
            }
            else
            {
                direction = Vector2.zero;
                preparing = true;
            }
        }

        transform.Translate(direction * Time.deltaTime * speed);
    }

    protected override void HandleAnimation()
    {
        if (animator == null) return;

        base.HandleAnimation();
        animator.SetBool("withSpear", withSpear);
    }

    protected override void Rotate()
    {
        if (!withSpear && toSpearDistance() <= spearPreferedCoef * toPlayerDistance())
        {
            if (spriteRenderer.flipX != Mathf.Sign(toSpearVector().x) < 0)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        }
        else
        {
            base.Rotate();
        }
    }

    internal void HandleSpear()  // todo add dilation between throwings
    {
        // Debug.Log("Inside ThrowSpear, withSpear: " + withSpear + " time: " + Time.time);
        if (Player == null) return;

        float playerDistance = toPlayerDistance();

        if (!withSpear)
        {
            // check if it launched
            if (!((Vector2)spear.transform.position == lastSpearTarget))
            {
                return;
            }

            float spearDistance = (spear.transform.position - transform.position).magnitude;

            // obtain spear
            if (spearDistance < 0.3f) // todo remove hard code
            {
                spear.transform.parent = transform;
                withSpear = true;
                spear.transform.localPosition = defaultSpearPos;
                preparedToThrowTime = Time.time;
            }
        }
        else
        {
            float spearSpeed = 11f;

            var spearDirection = (Player.transform.position - spear.transform.position);
            spear.transform.eulerAngles = new Vector3(0, 0, PlayerController.VectorToAngle(spearDirection) - 90);

            if ((preparing || playerDistance <= throwingRadius && fightingRadius < playerDistance) 
                && (Time.time - preparedToThrowTime) > throwDilation)
            {
                lastSpearTarget = Player.transform.position;
                spear.transform.parent = null;
                withSpear = false;
                preparing = false;

                manager.LaunchObject(spear, lastSpearTarget, spearSpeed);  // todo remove hardcode
            }
        }
    }

    protected float toSpearDistance()
    {
        return toSpearVector().magnitude;
    }

    protected Vector2 toSpearVector()
    {
        return spear.transform.position - transform.position;
    }

    // todo better create spear script with handling this case (and other possible cases)
    private void OnDestroy()
    {
        Destroy(spear.gameObject);
    }
}
