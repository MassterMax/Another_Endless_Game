using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Spell, IKnowMonsterController, IKnowSpellManager
{
    float damageRadius = 1f;  // todo set as a parameter?
    float damage = 2f;
    float delay;
    public MonsterController MonsterController { get; set; }
    public SpellManager SpellManager { get; set; }

    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        var coroutine = DelayedCast(end, 0.15f);
        StartCoroutine(coroutine);
    }

    private IEnumerator DelayedCast(Vector2 end, float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var monster in MonsterController.GetMonstersInArea(end, damageRadius))
        {
            monster.TakeDamage(damage);
        }

        foreach (var spell in SpellManager.GetSpellsInArea(end, damageRadius))
        {
            switch (spell)
            {
                case Puddle puddle:
                    puddle.ChargeByLightning();
                    break;
                case Meadow:
                    SpellManager.CombineTwoSpells(this, spell, false, true);
                    break;
                case Dirt dirt:
                    SpellManager.CombineTwoSpells(this, spell, false, false);
                    dirt.CallDelayedDestroy();
                    break;
            }
            // if (spell is Puddle)
            // {
            //     // make puddle electric!!!
            //     ((Puddle)spell).ChargeByLightning();
            // }
            // else if (spell is Meadow)
            // {
            //     // fire the meadow
            //     SpellManager.CombineTwoSpells(this, spell, false, true);
            // }
        }
    }
}
