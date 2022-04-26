using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Monster
{
    [SerializeField] float throwingRadius = 8f;
    [SerializeField] float fightingRadius = 3f;
    [SerializeField] float hurryBoost = 1.5f;
    LaunchItemManager manager;

    // todo make timer because I zadolbalsya
    float throwDilation = .5f;
    float preparedToThrowTime = 0f;

    GameObject spear;
    bool withSpear = true;
    Vector2 defaultSpearPos;
    Vector2 lastSpearTarget;

    public override float Damage
    {
        get {
            if (withSpear)
            {
                return base.Damage * 3;
            }
            else
            {
                return base.Damage;
            }
        }
    }

    protected override void Move()
    {
        if (Player == null) return;

        float playerDistance = (Player.transform.position - transform.position).magnitude;

        if (!withSpear)
        {
            float spearDistance = (spear.transform.position - transform.position).magnitude;
            // go for player only if spear is far away
            if (spearDistance <= 2 * playerDistance) direction = (spear.transform.position - transform.position).normalized;
            else direction = (Player.transform.position - transform.position).normalized;

            direction *= hurryBoost;
        }
        else
        {
            // move only if out of throwing range or too late to throw... >;-(
            if (playerDistance <= fightingRadius || playerDistance > throwingRadius)
            {
                direction = (Player.transform.position - transform.position).normalized;
                preparedToThrowTime = Time.time;
            }
            else
            {
                direction = Vector2.zero;
            }
        }

        transform.Translate(direction * Time.deltaTime * speed);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        spear = gameObject.transform.GetChild(0).gameObject;
        manager = FindObjectOfType<LaunchItemManager>();  // todo maybe remove
        defaultSpearPos = spear.transform.localPosition;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        HandleSpear();
    }

    internal void HandleSpear()  // todo add dilation between throwings
    {
        // Debug.Log("Inside ThrowSpear, withSpear: " + withSpear + " time: " + Time.time);
        float playerDistance = (Player.transform.position - transform.position).magnitude;

        if (!withSpear)
        {
            // check if it launched
            if (!((Vector2)spear.transform.position == lastSpearTarget))
            {
                return;
            }

            float spearDistance = (spear.transform.position - transform.position).magnitude;

            if (spearDistance < 0.5f) // todo remove hard code
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

            //float launchAngle = manager.GetLaunchAngle(playerDistance, spearSpeed);
            //spear.transform.eulerAngles = new Vector3(0, 0, launchAngle * Mathf.Rad2Deg - 90);
            if (playerDistance <= throwingRadius && fightingRadius < playerDistance && (Time.time - preparedToThrowTime) > throwDilation)
            {
                lastSpearTarget = Player.transform.position;
                spear.transform.parent = null;
                withSpear = false;
                
                manager.LaunchObject(spear, lastSpearTarget, spearSpeed);  // todo remove hardcode
            }
        }
    }
}
