using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    float dashCooldown = 1f;
    float dashDuration = 0.25f;
    float invincibilityTime = 0f;
    float invincibilityDuration = 1f;

    // todo maybe separate mana as entity
    Bar manaBar;
    float mana = 100;  // todo change
    float maxMana = 100;
    float manaRegen = 2f;

    private float totalScore = 0;
    [SerializeField] Text playerScore;

    Dictionary<KeyCode, Vector2> keyToVector = new Dictionary<KeyCode, Vector2>() {
        { KeyCode.W, Vector2.up},
        { KeyCode.S, Vector2.down },
        { KeyCode.D, Vector2.right },
        { KeyCode.A, Vector2.left }
    };

    public void ChangeScore(float value)
    {
        this.totalScore += value;
        playerScore.text = "score: " + (int)this.totalScore;
    }

    void Start()
    {
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        stick = transform.GetChild(0).gameObject;
        stickSpriteRenderer = stick.GetComponent<SpriteRenderer>();

        SetAttributes(playerHealth, playerHealth, playerDamage, playerSpeed, true, 0f);
        SetBarStyle(110);
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
        // skip if paused, todo -> make better!
        if (Time.timeScale == 0f) return;

        if (dashTimer < dashCooldown)
        {
            direction = Vector2.zero;

            foreach (var pair in keyToVector)
                if (Input.GetKey(pair.Key))
                    direction += pair.Value;

            direction = direction.normalized;

        }
        dashTimer = Mathf.Max(0, dashTimer - Time.deltaTime);

        float dashMultiplyer = dashTimer > dashCooldown ? 3f : 1f;

        transform.Translate(direction * Time.deltaTime * Speed * dashMultiplyer);
    }

    void Dash()
    {
        // skip if paused, todo -> make better!
        if (Time.timeScale == 0f) return;

        if (Input.GetKeyDown(KeyCode.Space) && dashTimer == 0)
        {
            dashTimer = dashDuration + dashCooldown;
        }
    }

    void Rotate()
    {
        // skip if paused, todo -> make better!
        if (Time.timeScale == 0f) return;

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

    // for monsters
    // private void OnTriggerStay2D(Collider2D collision)
    // {
    //     var damaging = collision.gameObject.GetComponent<IDamaging>();
    //     if (!(damaging is Monster)) return;
    //     if (damaging == null || damaging.GetDamage() == 0) return;
    //     if (((Creature)damaging).Friendly) return;

    //     TakeDamage(damaging.GetDamage());
    // }

    // for launchable things
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damaging = collision.gameObject.GetComponent<IDamaging>();
        if (!(damaging is Launchable)) return;
        if (damaging == null || damaging.GetDamage() == 0) return;

        if (((Launchable)damaging).LandInTarget(transform))
        {
            TakeDamage(damaging.GetDamage());
        }
    }

    public float GetHeight()
    {
        return playerSpriteRenderer.bounds.size.y;
    }


    protected override void OnDeath()
    {
        base.OnDeath();
        var endScore = (int)this.totalScore;
        FindObjectOfType<MenuController>().OnDeath(endScore);
    }
}