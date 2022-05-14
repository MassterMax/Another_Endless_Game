using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Monster
{
    [SerializeField] GameObject boxOfSpears;
    SpriteRenderer boxOfSpearsSpriteRenderer;
    bool withBoxOfSpears = true;

    float spearSpeed;
    float throwingRadius = 4;
    float fightingRadius = 2f;
    float hurryBoost = 2f;

    // todo make timer because I zadolbalsya
    float throwDilation = .5f;
    float preparedToThrowTime = 0f;

    float spearPreferedCoef = 0.9f;  // if toSpearDistance < toPlayerDistance * coef; => prefer to obtain spear

    Spear spear;
    bool withSpear = true;
    bool preparing = false;
    Vector2 defaultSpearPos;
    Vector2 lastSpearTarget;
    float pseudoSpearDistance;

    public override float Damage { get => withSpear ? base.Damage * 3 : base.Damage; }
    // todo spear has own collider and maybe I should separate it as entity

    protected override void Start()
    {
        base.Start();

        spearSpeed = Mathf.Ceil(Mathf.Sqrt(throwingRadius * 9.81f));

        spear = GetComponentInChildren<Spear>();
        // manager = FindObjectOfType<LaunchItemManager>();  // todo maybe remove
        defaultSpearPos = spear.transform.localPosition;

        // for reflections
        pseudoSpearDistance = spear.transform.localPosition.y + spriteRenderer.bounds.size.y / 2;
        // Debug.LogWarning("pseudo disnatnce is " + pseudoSpearDistance);
        spear.OnPickup(pseudoSpearDistance);

        boxOfSpearsSpriteRenderer = boxOfSpears.GetComponent<SpriteRenderer>();
        boxOfSpears.SetActive(withBoxOfSpears);
    }

    protected override void Update()
    {
        base.Update();
        if (!isDead)
            HandleSpear();
    }

    protected override void Move()
    {
        if (!HasTarget()) return;
        if (preparing) return;

        float toTargetDistance = ToTargetDistance();

        if (!withSpear)
        {
            // TODO: 
            // Change hurryBoost to buff!
            // 
            // go for player only if spear is far away
            if (spear != null && !spear.InTarget && !spear.InFlight && ToSpearDistance() <= spearPreferedCoef * toTargetDistance)
                SetMoveDirection(ToSpearVector());
            else
                SetMoveDirection(ToTargetVector());
            BoostSpeed(hurryBoost);
        }
        else
        {
            // move only if out of throwing range or too late to throw... >;-(
            if (toTargetDistance <= fightingRadius || toTargetDistance > throwingRadius)
            {
                SetMoveDirection(ToTargetVector());
                preparedToThrowTime = Time.time;
            }
            else
            {
                SetMoveDirection();
                preparing = true;
            }
        }

        base.Move();
    }

    protected override void HandleAnimation()
    {
        if (animator == null) return;

        base.HandleAnimation();
        animator.SetBool("withSpear", withSpear);
    }

    protected override void Rotate()
    {
        if (preparing)
        {
            if (spriteRenderer.flipX != Mathf.Sign(ToTargetVector().x) < 0)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        }
        else
        {
            base.Rotate();
        }

        if (boxOfSpearsSpriteRenderer.flipX != spriteRenderer.flipX)
        {
            boxOfSpearsSpriteRenderer.flipX = spriteRenderer.flipX;
        }
    }

    internal void HandleSpear() // todo add extra conditions if skeleton has box of spears
    {
        if (!HasTarget()) return;
        if (spear == null) return;
        // if spear in target
        if (spear.InTarget) return;

        float toTargetDistance = ToTargetDistance();

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
                spear.OnPickup(pseudoSpearDistance);
            }
        }
        else
        {
            var spearDirection = TargetPosition() - (Vector2)spear.transform.position;
            spear.transform.eulerAngles = new Vector3(0, 0, Utils.VectorToAngle(spearDirection) - 90);

            if ((preparing || toTargetDistance <= throwingRadius && fightingRadius < toTargetDistance)
                && (Time.time - preparedToThrowTime) > throwDilation)
            {
                lastSpearTarget = TargetPosition();
                spear.transform.parent = null;
                withSpear = false;
                preparing = false;

                spear.LaunchObject(lastSpearTarget, spearSpeed);  // todo remove hardcode
            }
        }
    }

    public override bool ShouldChaseTarget()
    {
        if (preparing) return true;
        return base.ShouldChaseTarget();
    }
    protected float ToSpearDistance()
    {
        return ToSpearVector().magnitude;
    }

    protected Vector2 ToSpearVector()
    {
        return spear.transform.position - transform.position;
    }

    // todo maybe it will be better to create spear script with handling this case (and other possible cases)
    private void OnDestroy()
    {
        if (spear != null && spear.gameObject.activeSelf)
        {
            spear.StartFadingDestroy();
        }
    }
}
