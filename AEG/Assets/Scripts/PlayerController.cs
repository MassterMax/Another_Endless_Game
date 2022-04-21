using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Creature
{
    [SerializeField] float playerSpeed;
    [SerializeField] float playerHealth;
    [SerializeField] float playerDamage;

    SpriteRenderer spriteRenderer;
    Animator animator;
    Vector2 direction;
    float dashTimer = 0f;

    SpriteRenderer stickSpriteRenderer;
    GameObject stick;


    Dictionary<KeyCode, Vector2> keyToVector = new Dictionary<KeyCode, Vector2>() {
        { KeyCode.W, Vector2.up},
        { KeyCode.S, Vector2.down },
        { KeyCode.D, Vector2.right },
        { KeyCode.A, Vector2.left }
    };

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        stick = transform.GetChild(0).gameObject;
        stickSpriteRenderer = stick.GetComponent<SpriteRenderer>();

        SetAttributes(playerHealth, playerDamage);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Dash();
        Rotate();
        // Attack();
        HandleAnimation();
    }


    void Move()
    {
        if (dashTimer == 0)
        {
            direction = Vector2.zero;

            foreach (var pair in keyToVector)
                if (Input.GetKey(pair.Key))
                    direction += pair.Value;

            direction = direction.normalized;

        }
        else
        {
            dashTimer = Mathf.Max(0, dashTimer - Time.deltaTime);
        }

        float dashMultiplyer = dashTimer > 0 ? 3f : 1f;

        transform.Translate(direction * Time.deltaTime * playerSpeed * dashMultiplyer);
    }

    void Dash()
    {
        // if can dash check
        if (Input.GetKeyDown(KeyCode.Space) && dashTimer == 0)
        {
            dashTimer = 0.25f;
        }
    }
    /*
    void Attack()
    {
        var dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        dir.z = 0;
        //Debug.Log("after" + dir);

        dir = dir.normalized;
        float a = Mathf.Acos(dir.x);
        if (dir.y < 0) a = -a;
        // if (dir.x < 0) a = a - Mathf.PI;

        sword.gameObject.transform.rotation = new Quaternion();

        sword.gameObject.transform.localPosition = dir * swordDistance + Vector3.up * swordOffset + dir * sword.getAnimationBias();
        sword.gameObject.transform.Rotate(new Vector3(0, 0, -90 + a * Mathf.Rad2Deg), Space.Self);
        //sword.gameObject.transform.RotateAround(newSwordPos - dir * 0.5f, new Vector3(0, 0, 1), -45); 


        if (Input.GetKey(KeyCode.Mouse0))
        {
            sword.OnAttack();
        }
    }*/

    void Rotate()
    {
        Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

        //transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);

        if (spriteRenderer.flipX != Mathf.Sign(dir.x) < 0)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            RotateStick();
        }

        //dir = dir.normalized;
        //print(dir);
        var angle = Vector2.Angle(Vector2.right, dir);
        //print("Angle (Pi)2: " + Vector2.Angle(dir, transform.right));
        if (dir.y < 0)
        {
            angle = -angle;
            if (dir.x < 0) angle = 360 + angle;
        }

        print("Angle (180): " + angle);
        print("Angle (Pi): " + angle * Mathf.Deg2Rad);
        stick.transform.eulerAngles = new Vector3(0, 0, angle / 2 - 45);

    }

    void RotateStick()
    {
        stickSpriteRenderer.flipX = !stickSpriteRenderer.flipX;
        stick.transform.localPosition *= -1;  // change x position
        //stick.transform.Rotate(0, 0, 90 * Mathf.Sign(-stick.transform.rotation.z), Space.Self);
    }

    void HandleAnimation()
    {
        animator.SetBool("isRun", direction.sqrMagnitude != 0);
    }
}
