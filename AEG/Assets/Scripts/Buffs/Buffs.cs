using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffTargetField
{
    MaxHealth,
    Health,
    Damage,
    Speed
}

public abstract class Buff
{
    public float applyTime;
    public abstract BuffTargetField TargetField { get; }
    public abstract bool IsDebuff { get; }
    public abstract bool IsMultiplier { get; }
    public abstract float GetValue(Creature creature);
    public abstract float Duration { get; }

    public void Extend()
    {
        applyTime = Time.time;
    }

    public Buff()
    {
        Extend();
    }
}

public interface ISpecialApplicable
{
    public abstract void SpecialApply(Creature creature);
}

public interface ISpecialConditionable
{
    public abstract bool CanApply(Creature creature);
}

public abstract class CoroutineBuff : Buff
{
    public abstract IEnumerator StartBuff(Creature creature);
}

public class MeadowHealBuff : CoroutineBuff
{
    public override BuffTargetField TargetField => BuffTargetField.Health;

    public override bool IsDebuff => false;

    public override bool IsMultiplier => false;

    public override float Duration => 3;

    public override float GetValue(Creature creature)
    {
        return 1;
    }

    public override IEnumerator StartBuff(Creature creature)
    {
        while (Time.time < applyTime + Duration)
        {
            creature.Heal(GetValue(creature));
            yield return new WaitForSeconds(1);
        }

        creature.RemoveBuff(this.GetType());
    }
}

public class FireMeadowBuff : CoroutineBuff, ISpecialConditionable
{
    public override BuffTargetField TargetField => BuffTargetField.Health;

    public override bool IsDebuff => true;

    public override bool IsMultiplier => false;

    public override float Duration => 2;

    public bool CanApply(Creature creature)
    {
        return !creature.HasBuff(typeof(PuddleSlowBuff));
    }

    public override float GetValue(Creature creature)
    {
        return 1;
    }

    public void SpecialApply(Creature creature)
    {
        if (creature.HasBuff(typeof(PuddleSlowBuff)))
        {
            creature.RemoveBuff(this.GetType());
        }
    }

    public override IEnumerator StartBuff(Creature creature)
    {
        //Debug.LogWarning("Fire Meadow Starts");
        //Debug.Log("Start time: " + Time.time);
        //Debug.Log("Apply time: " + applyTime);
        while (Time.time < applyTime + Duration)
        {
            //Debug.Log("Take damage at: " + Time.time);
            //Debug.Log("Apply time: " + applyTime);
            creature.TakeDamage(GetValue(creature));
            yield return new WaitForSeconds(1);
        }

        creature.RemoveBuff(this.GetType());
    }
}

public class ElectricityDamageBuff : CoroutineBuff
{
    public override BuffTargetField TargetField => BuffTargetField.Health;

    public override bool IsDebuff => true;

    public override bool IsMultiplier => false;

    public override float Duration => 0.5f;

    public override float GetValue(Creature creature)
    {
        return 0.25f;
    }

    public override IEnumerator StartBuff(Creature creature)
    {
        while (Time.time < applyTime + Duration)
        {
            creature.TakeDamage(GetValue(creature));
            yield return new WaitForSeconds(Duration + 0.001f);
        }

        creature.RemoveBuff(this.GetType());
    }
}

public class PuddleSlowBuff : Buff, ISpecialApplicable
{
    public override bool IsMultiplier => true;

    public override bool IsDebuff => true;

    public override BuffTargetField TargetField => BuffTargetField.Speed;

    public override float Duration => 1f;

    public override float GetValue(Creature creature)
    {
        return 0.9f;
    }

    public void SpecialApply(Creature creature)
    {
        if (creature.HasBuff(typeof(DirtSlowBuff)))
        {
            creature.RemoveBuff(typeof(DirtSlowBuff));
        }
        if (creature.HasBuff(typeof(FireMeadowBuff)))
        {
            creature.RemoveBuff(typeof(FireMeadowBuff));
        }
    }
}

public class DirtSlowBuff : Buff, ISpecialConditionable
{
    public override bool IsMultiplier => true;

    public override bool IsDebuff => true;

    public override BuffTargetField TargetField => BuffTargetField.Speed;

    public override float Duration => 2f;

    public override float GetValue(Creature creature)
    {
        return 0.6f;
    }

    public bool CanApply(Creature creature)
    {
        return !creature.HasBuff(typeof(PuddleSlowBuff));
    }
}