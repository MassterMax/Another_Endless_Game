using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    HealthBar healthBar;

    private float maxHealth; // todo
    private float health;
    private float damage;
    private bool set = false;
    private bool friendly;
    private bool protection = false;

    public float Health { get => health; }
    public float MaxHealth { get => maxHealth; }
    public bool Friendly { get => friendly; }
    public float Damage { get => damage; }
    public bool Protection { get => protection; set => protection = value; }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            Destroy(gameObject);  // todo maybe add getter/setter
        }
    }

    internal void SetAttributes(float health, float damage, bool friendly)
    {
        if (set)
            throw new System.Exception("creature already set");
        set = true;

        this.health = health;
        this.maxHealth = health;
        this.damage = damage;
        this.friendly = friendly;

        GameObject healthBarObject = Instantiate(Resources.Load("Prefabs/HealthCanvas"), transform.position, Quaternion.identity) as GameObject;
        healthBarObject.transform.parent = transform;
        healthBar = healthBarObject.GetComponentInChildren<HealthBar>();
        healthBar.Setup(health, maxHealth, friendly);
    }

    /*
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.name + " triggered " + gameObject.name + ": " + Time.time);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name + " collided " + gameObject.name + ": " + Time.time);
    }*/
}
