using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Spell, IKnowMonsterController, IKnowSpellManager
{
    float damageRadius = 1f;  // todo set as a parameter?
    float delay;
    public MonsterController MonsterController { get; set; }
    public SpellManager SpellManager { get; set; }

    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {

        // I should make delay((

        StartCoroutine(DelayedCast(end, 0.1f));

        //foreach(var monster in MonsterController.GetMostersInArea(end, damageRadius))
        //{
        //    monster.TakeDamage(1);
        //}

        //foreach(var spell in SpellManager.GetSpellsInArea(end, damageRadius))
        //{
        //    if (spell is Puddle)
        //    {
        //        // make puddle electric!!!
        //        ((Puddle)spell).ChargeByLightning();
        //    }
        //}
    }

    private IEnumerator DelayedCast(Vector2 end, float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var monster in MonsterController.GetMostersInArea(end, damageRadius))
        {
            monster.TakeDamage(1);
        }

        foreach (var spell in SpellManager.GetSpellsInArea(end, damageRadius))
        {
            if (spell is Puddle)
            {
                // make puddle electric!!!
                ((Puddle)spell).ChargeByLightning();
            }
        }

        // Destroy(gameObject);
    }

    void Start()
    {
        // float delay = this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 0.01f;
        // Destroy(gameObject, delay);
    }

    /*
    // Update is called once per frame
    void Update()
    {
        
    }*/
}
