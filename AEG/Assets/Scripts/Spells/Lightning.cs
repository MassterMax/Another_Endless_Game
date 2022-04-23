using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : SpellKnowMonsters
{
    float damageRadius = 1f;  // todo set as a parameter?

    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        base.CastSpell(start, end, center);
        foreach(var monster in monsterController.GetMostersInArea(end, damageRadius))
        {
            monster.TakeDamage(1);
        }
    }

    public override float GetManaCost()
    {
        return 10;
    }

    void Start()
    {
        Destroy(gameObject, 1);
    }

    /*
    // Update is called once per frame
    void Update()
    {
        
    }*/
}
