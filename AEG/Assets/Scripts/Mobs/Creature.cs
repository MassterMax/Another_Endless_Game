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
    public float MaxHealth { get => maxHealth; } // todo
    public virtual float Damage { get => damage; }  // todo

    Dictionary<Type, Buff> buffs = new();

    public void RemoveBuff(Type buffType)
    {
        if ((typeof(Buff).IsAssignableFrom(buffType)))
        {
            if (buffs.ContainsKey(buffType))
            {
                buffs[buffType].applyTime = float.MinValue;
                buffs.Remove(buffType);
            }
        }
    }

    public void ApplyBuff(Type buffType)
    {
        if ((typeof(Buff).IsAssignableFrom(buffType)))
        {
            if (buffs.ContainsKey(buffType))
            {
                buffs[buffType].Extend();
                return;
            }

            //int i = 0;
            //while (i < buffs.Count)
            //{
            //    var buff = buffs[i];
            //    // expired buff should be removed
            //    if (Time.time > buff.applyTime + buff.Duration)
            //    {
            //        buffs.RemoveAt(i);
            //    }
            //    else if (buff.GetType().Equals(buffType))
            //    {
            //        //Debug.Log("extend");
            //        buff.Extend();
            //        return;
            //    }
            //}

            var appliedBuff = (Buff)Activator.CreateInstance(buffType);
            buffs[buffType] = appliedBuff;
            if (appliedBuff is CoroutineBuff)
            {
                StartCoroutine(((CoroutineBuff)appliedBuff).StartBuff(this));
            }
        } else
        {
            Debug.LogError("unknown buff type: " + buffType);
        }
    }

    private float CalculateField(float originalValue, BuffTargetField targetField)
    {
        float multiplier = 1f;
        float additive = 0f;

        List<Type> removeBuffs = new();

        foreach(var el in buffs)
        {
            var buff = el.Value;
            if (Time.time > buff.applyTime + buff.Duration)
            {
                removeBuffs.Add(el.Key);
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
            }

        }

        foreach (var key in removeBuffs)
            buffs.Remove(key);
        //while (i < buffs.Count)
        //{
        //    var buff = buffs[i];
        //    if (Time.time > buff.applyTime + buff.Duration)
        //    {
        //        buffs.RemoveAt(i);
        //    }
        //    else
        //    {
        //        if (buff.TargetField == targetField)
        //        {
        //            float value = buff.GetValue(this);
        //            if (buff.IsMultiplier)
        //            {
        //                multiplier *= value;
        //            }
        //            else
        //            {
        //                additive += value;
        //            }
        //        }
        //        i += 1;
        //    }
        //}

        return (originalValue + additive) * multiplier;
    }

    public virtual void Heal(float value)
    {
        value = Mathf.Min(value, maxHealth - health);
        Debug.Log("Heal: " + value);
        health += value;
        healthBar.SetValue(health);
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
