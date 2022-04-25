using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Creature
{
    // todo generalization!
    [SerializeField] float speed;
    // [SerializeField] float visibleField = 5;
    // [SerializeField] float changeDirectonLimit = 2;

    [SerializeField] float throwingRadius = 7f;
    [SerializeField] float fightingRadius = 3f;
    [SerializeField] float hurryBoost = 2f;
    LaunchItemManager manager;

    GameObject spear;
    Animator animator;
    PlayerController player;
    Vector2 direction;
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

    internal override void HandleAnimation()
    {
        // pass
    }

    internal override void Move()
    {
        float distance = (player.transform.position - transform.position).magnitude;

        if (!withSpear)
        {
            float spearDistance = (spear.transform.position - transform.position).magnitude;
            if (spearDistance <= distance) direction = (spear.transform.position - transform.position).normalized * hurryBoost;
            else direction = (player.transform.position - transform.position).normalized;
        }
        else
        {
            // move only if out of throwing range or too late to throw... >;-(
            if (distance <= fightingRadius || distance > throwingRadius)
            {
                direction = (player.transform.position - transform.position).normalized;
            }
        }


        // fight anyway, GENERALIZATION!
        //if (distance <= fightingRadius || distance > throwingRadius)
        //{
        //    direction = (player.transform.position - transform.position).normalized;
        //}
        //else if (!withSpear)
        //{
        //    direction = (spear.transform.position - transform.position).normalized;
        //    direction *= hurryBoost;
        //}
        transform.Translate(direction * Time.deltaTime * speed);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetAttributes(5, 1, false);  // todo make one class with all start values
        SetBarStyle();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();

        spear = gameObject.transform.GetChild(0).gameObject;
        manager = FindObjectOfType<LaunchItemManager>();

        defaultSpearPos = spear.transform.localPosition;
    }

    // Update is called once per frame
    internal override void Update()
    {
        base.Update();
        ThrowSpear();
    }

    internal void ThrowSpear()  // todo add dilation between throwings
    {
        Debug.Log("Inside ThrowSpear, withSpear: " + withSpear + " time: " + Time.time);
        float playerDistance = (player.transform.position - transform.position).magnitude;

        if (!withSpear)
        {
            // check if it launched
            if (!((Vector2)spear.transform.position == lastSpearTarget))
            {
                return;
            }

            float spearDistance = (spear.transform.position - transform.position).magnitude;
            // calculate only when spear on the floor
            Debug.Log("no spear man");
            Debug.Log(spear.transform.position);
            Debug.Log(transform.position);
            Debug.Log(spearDistance);


            if (spearDistance < 0.5f) // todo change
            {
                spear.transform.parent = transform;
                withSpear = true;
                spear.transform.localPosition = defaultSpearPos;
            }
        }
        else if (playerDistance <= throwingRadius && fightingRadius < playerDistance)
        {
            lastSpearTarget = player.transform.position;
            Debug.LogWarning("we realy want to throw it");
            Debug.Log("withSpear: " + withSpear + " time: " + Time.time);
            spear.transform.parent = null;
            withSpear = false;
            manager.LaunchObject(spear, lastSpearTarget, 10f);  // todo remove hardcode
        }
    }
}
