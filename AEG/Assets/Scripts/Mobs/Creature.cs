using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour, IDamaging
{
    Bar healthBar;

    private float maxHealth; // todo
    private float health;
    private float damage;
    private bool set = false;
    private bool friendly;
    private bool protection = false;

    public float Health { get => health; }
    public float MaxHealth { get => maxHealth; }
    public bool Friendly { get => friendly; }
    public bool Protection { get => protection; set => protection = value; }

    public virtual float Damage { get => damage; }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetValue(health);
        if (health <= 0)
        {
            Destroy(gameObject);  // todo maybe add getter/setter
        }
    }

    protected virtual void Update()
    {
        Move();
        HandleAnimation();
    }

    protected abstract void Move();

    protected abstract void HandleAnimation();

    protected virtual void SetAttributes(float health, float maxHealth, float damage, bool friendly)
    {
        if (set)
            throw new System.Exception("creature already set");
        set = true;

        this.health = health;
        this.maxHealth = maxHealth;
        this.damage = damage;
        this.friendly = friendly;

        GameObject healthBarObject = Instantiate(Resources.Load("Prefabs/HealthCanvas"), transform.position, Quaternion.identity) as GameObject;
        healthBarObject.transform.parent = transform;
        healthBar = healthBarObject.GetComponentInChildren<Bar>();
        healthBar.Setup(health, maxHealth);
    }

    protected void SetBarStyle(string colorName = "red", int sorterOrder = 0)
    {
        healthBar.SetStyle(colorName, sorterOrder);
    }

    public float GetDamage()
    {
        if (friendly)
        {
            return 0;
        }

        return Damage;
    }
}
