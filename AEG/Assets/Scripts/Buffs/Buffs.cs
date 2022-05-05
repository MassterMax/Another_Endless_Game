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

public class PuddleSlowBuff : Buff
{
    public override bool IsMultiplier => true;

    public override bool IsDebuff => true;

    public override BuffTargetField TargetField => BuffTargetField.Speed;

    public override float Duration => 1f;

    public override float GetValue(Creature creature)
    {
        return 0.6f;
    }
}