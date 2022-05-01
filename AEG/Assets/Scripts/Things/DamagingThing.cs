using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamagingThing : MonoBehaviour, IDamaging
{
    private float damage;

    public float Damage { get => damage; set => damage = value; }

    public float GetDamage()
    {
        return damage;
    }
}
