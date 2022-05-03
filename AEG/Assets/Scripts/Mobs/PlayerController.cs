using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Creature, IBlinkable
{
    [SerializeField] float playerSpeed;
    [SerializeField] float playerHealth;
    [SerializeField] float playerDamage;

    SpriteRenderer playerSpriteRenderer;
    Animator animator;
    Vector2 direction;

    SpriteRenderer stickSpriteRenderer;
    GameObject stick;

    float dashTimer = 0f;
    float invincibilityTime = 0f;
    float invincibilityDuration = 1f;

    // todo maybe separate mana as entity
    Bar manaBar;
    float mana = 100;  // todo change
    float maxMana = 100;
    float manaRegen = 5f;

    Dictionary<KeyCode, Vector2> keyToVector = new Dictionary<KeyCode, Vector2>() {
        { KeyCode.W, Vector2.up},
        { KeyCode.S, Vector2.down },
        { KeyCode.D, Vector2.right },
        { KeyCode.A, Vector2.left }
    };

    void Start()
    {
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        stick = transform.GetChild(0).gameObject;
        stickSpriteRenderer = stick.GetComponent<SpriteRenderer>();

        SetAttributes(playerHealth, playerHealth, playerDamage, playerSpeed, true);
        SetBarStyle("green", 110);
        SetupManaBar();
    }

    protected override void Update()
    {
        base.Update();
        Dash();
        Rotate();
        RegenMana();
    }

    void SetupManaBar()
    {
        var bars = FindObjectsOfType<Bar>();
        foreach (var bar in bars)
        {
            if (bar.name.Equals("ManaBar"))  // :(
            {
                manaBar = bar;
                break;
            }
        }
        manaBar.Setup(mana, maxMana);
    }

    void RegenMana()
    {
        mana = Mathf.Min(mana + manaRegen * Time.deltaTime, maxMana);
        manaBar.SetValue(mana);
    }


    protected override void Move()
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

        transform.Translate(direction * Time.deltaTime * Speed * dashMultiplyer);
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

        if (playerSpriteRenderer.flipX != Mathf.Sign(dir.x) < 0)
        {
            playerSpriteRenderer.flipX = !playerSpriteRenderer.flipX;
            FlipStick();
        }

        var angle = Utils.VectorToAngle(dir);
        stick.transform.eulerAngles = new Vector3(0, 0, angle / 2 - 45);
    }

    void FlipStick()
    {
        stickSpriteRenderer.flipX = !stickSpriteRenderer.flipX;
        stick.transform.localPosition *= -1;  // change x position
    }

    protected override void HandleAnimation()
    {
        animator.SetBool("isRun", direction.sqrMagnitude != 0);
    }

    public bool SpendMana(float value)
    {
        if (mana >= value)
        {
            mana -= value;
            manaBar.SetValue(mana);
            return true;
        }
        return false;
    }

    public override void TakeDamage(float damage)
    {
        float dilation = 0.1f;
        int times = (int)(invincibilityDuration / dilation / 2);

        if (Time.time - invincibilityTime > invincibilityDuration)
        {
            invincibilityTime = Time.time;

            if (Protection)
            {
                Protection = false;
                var circle = transform.GetComponentInChildren<Circle>(); // bye-bye
                StartCoroutine(((IBlinkable)this).Blinking(circle.gameObject.GetComponent<SpriteRenderer>(), times, dilation));

                // HANDLE REFLECTIONS
                circle.reflection.BlinkReflection(times, dilation);

                Destroy(circle.gameObject, invincibilityDuration);
                return;
            }

            base.TakeDamage(damage);

            StartCoroutine(((IBlinkable)this).Blinking(playerSpriteRenderer, times, dilation));
            StartCoroutine(((IBlinkable)this).Blinking(stickSpriteRenderer, times, dilation));

            // HANDLE REFLECTIONS
            var reflection = GetComponent<Reflectable>();
            if (reflection != null) reflection.BlinkReflection(times, dilation);
            var stickReflection = stick.GetComponent<Reflectable>();
            if (stickReflection != null) stickReflection.BlinkReflection(times, dilation);
        }
    }

    //public IEnumerator Blinking(SpriteRenderer spriteRenderer, int times, float dilation)
    //{
    //    for (var n = 0; n < times; n++)
    //    {
    //        spriteRenderer.enabled = true;
    //        yield return new WaitForSeconds(dilation);
    //        spriteRenderer.enabled = false;
    //        yield return new WaitForSeconds(dilation);
    //    }
    //    spriteRenderer.enabled = true;
    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        var damaging = collision.gameObject.GetComponent<IDamaging>();
        if (damaging == null || damaging.GetDamage() == 0) return;

        TakeDamage(damaging.GetDamage());

        if (damaging is Launchable)
        {
            ((Launchable)damaging).LandInTarget(transform);
        }
    }

    public float GetHeight()
    {
        return playerSpriteRenderer.bounds.size.y;
    }
}