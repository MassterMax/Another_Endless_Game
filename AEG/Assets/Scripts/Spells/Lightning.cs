using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Spell, IKnowMonsterController, IKnowSpellManager
{
    float damageRadius = 1f;  // todo set as a parameter?

    public MonsterController MonsterController { get; set; }
    public SpellManager SpellManager { get; set; }

    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        foreach(var monster in MonsterController.GetMostersInArea(end, damageRadius))
        {
            monster.TakeDamage(1);
        }

        foreach(var spell in SpellManager.GetSpellsInArea(end, damageRadius))
        {
            if (spell is Puddle)
            {
                // make puddle electric!!!
                ((Puddle)spell).ChargeByLightning();
            }
        }
    }

    // todo get in spell manager
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
