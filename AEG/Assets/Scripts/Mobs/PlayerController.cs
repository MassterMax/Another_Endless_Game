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
    float invincibilityTime = 0f;
    float invincibilityDuration = 1f;

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

        SetAttributes(playerHealth, playerDamage, true);
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
        if (Input.GetKeyDown(KeyCode.Space) && dashTimer == 0)
        {
            dashTimer = 0.25f;
        }
    }

    void Rotate()
    {
        Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

        if (spriteRenderer.flipX != Mathf.Sign(dir.x) < 0)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            FlipStick();
        }

        var angle = VectorToAngle(dir);

        //print("Angle (180): " + angle);
        //print("Angle (Pi): " + angle * Mathf.Deg2Rad);
        stick.transform.eulerAngles = new Vector3(0, 0, angle / 2 - 45);

    }

    void FlipStick()
    {
        stickSpriteRenderer.flipX = !stickSpriteRenderer.flipX;
        stick.transform.localPosition *= -1;  // change x position
        //stick.transform.Rotate(0, 0, 90 * Mathf.Sign(-stick.transform.rotation.z), Space.Self);
    }

    void HandleAnimation()
    {
        animator.SetBool("isRun", direction.sqrMagnitude != 0);
    }

    float VectorToAngle(Vector2 vector)
    {
        var angle = Vector2.Angle(Vector2.right, vector);
        if (vector.y < 0)
        {
            angle = -angle;
            if (vector.x < 0) angle = 360 + angle;
        }
        return angle;
    }

    public override void TakeDamage(float damage)
    {
        if (Time.time - invincibilityTime > invincibilityDuration)  // todo make as a parameter
        {
            invincibilityTime = Time.time;

            if (Protection)
            {
                Protection = false;
                var circle = transform.GetComponentInChildren<Circle>(); // bye-bye
                Destroy(circle.gameObject);
                return;
            }

            base.TakeDamage(damage);
            float dilation = 0.1f;
            StartCoroutine(blinking((int)(invincibilityDuration / dilation / 2), dilation));
        }
    }


    IEnumerator blinking(int times, float dilation)
    {
        for (var n = 0; n < times; n++)
        {
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(dilation);
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(dilation);
        }
        spriteRenderer.enabled = true;
    }

    private void OnCollisionStay2D(Collision2D collision)  // todo Am I using correct Rigidboy settings???
    {
        var creature = collision.gameObject.GetComponent<Creature>();
        if (!creature || creature.Friendly) return;

        TakeDamage(creature.Damage);

        //Debug.Log(collision.gameObject.name + " collided " + gameObject.name + ": " + Time.time);
    }
}
