using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour, IDamaging
{
    Bar healthBar;

    private float speed;
    private float maxHealth; // todo
    private float health;
    private float damage;
    private bool set = false;
    private bool friendly;
    private bool protection = false;

    public bool Friendly { get => friendly; }
    public bool Protection { get => protection; set => protection = value; }

    public float Health { get => health; }  // under buffs??????????????????????

    // under buffs:
    public float Speed { get => CalculateField(speed, BuffTargetField.Speed); }
    public float MaxHealth { get => maxHealth; }
    public virtual float Damage { get => damage; }

    List<Buff> buffs = new List<Buff>();

    public void ApplyBuff(Type buffType)
    {
        if ((typeof(Buff).IsAssignableFrom(buffType)))
        {
            foreach(var buff in buffs)
            {
                // first we try to extend buff
                if (buff.GetType().Equals(buffType))
                {
                    //Debug.Log("extend");
                    buff.Extend();
                    return;
                }
            }

            // Debug.Log("add to buffs");
            buffs.Add((Buff)Activator.CreateInstance(buffType));
        } else
        {
            Debug.LogError("unknown buff type: " + buffType);
        }
    }

    private float CalculateField(float originalValue, BuffTargetField targetField)
    {
        float multiplier = 1f;
        float additive = 0f;

        int i = 0;
        while (i < buffs.Count)
        {
            var buff = buffs[i];
            if (Time.time > buff.applyTime + buff.Duration)
            {
                buffs.RemoveAt(i);
            }
            else
            {
                if (buff.TargetField == targetField)
                {
                    float value = buff.GetValue(this);
                    if (buff.IsMultiplier)
                    {
                        multiplier *= value;
                    }
                    else
                    {
                        additive += value;
                    }
                }
                i += 1;
            }
        }

        return (originalValue + additive) * multiplier;
    }


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

    protected virtual void SetAttributes(float health, float maxHealth, float damage, float speed, bool friendly)
    {
        if (set)
            throw new System.Exception("creature already set");
        set = true;

        this.health = health;
        this.maxHealth = maxHealth;
        this.damage = damage;
        this.friendly = friendly;
        this.speed = speed;

        GameObject healthBarObject = Instantiate(Resources.Load("Prefabs/HealthCanvas"), transform.position, Quaternion.identity) as GameObject;
        healthBarObject.transform.parent = transform;
        healthBar = healthBarObject.GetComponentInChildren<Bar>();
        healthBar.Setup(health, maxHealth);
    }

    protected void SetBarStyle(string colorName = "red", int sorterOrder = 11)
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
