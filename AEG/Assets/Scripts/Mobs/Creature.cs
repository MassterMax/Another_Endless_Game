using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    private float maxHealth; // todo
    private float health;
    private float damage;
    private bool set = false;
    HealthBar healthBar;

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            Destroy(gameObject);  // todo maybe add getter/setter
        }
    }

    internal void SetAttributes(float health, float damage)
    {
        if (set)
            throw new System.Exception("creature already set");

        set = true;
        this.health = health;
        this.maxHealth = health;
        this.damage = damage;

        GameObject healthBarObject = Instantiate(Resources.Load("Prefabs/HealthCanvas"), transform.position, Quaternion.identity) as GameObject;
        healthBarObject.transform.parent = transform;
        healthBar = healthBarObject.GetComponentInChildren<HealthBar>();
        healthBar.Setup(health, maxHealth);
    }

    public float GetHealth()  // סגמיסעגמ גלוסעמ לועמהא
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
