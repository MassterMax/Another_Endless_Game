using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : GroundedSpell, IKnowSpellManager
{
    public SpellManager SpellManager { get; set; }

    bool charged = false;
    GameObject lightningChild;
    private Collider2D objectCollider;

    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        transform.position = center;

        foreach (var spell in SpellManager.GetSpellsInArea(center, 1f))  // todo remove hard code
        {
            if (spell is Meadow)
            {
                // turn meadow into dirt
                SpellManager.CombineTwoSpells(this, spell);
                break;
            }
        }
    }

    public void ChargeByLightning()
    {
        if (charged) return;

        charged = true;
        lightningChild.SetActive(true);

        foreach (var spell in SpellManager.GetSpellsByType<Puddle>())
        {
            var puddle = (Puddle)spell;

            if (objectCollider.IsTouching(puddle.objectCollider))
            {
                puddle.ChargeByLightning();
            }
        }
    }

    void Start()
    {
        Debug.Log("puddle start!");
        objectCollider = GetComponent<Collider2D>();
        lightningChild = gameObject.transform.GetChild(0).gameObject;
        lightningChild.SetActive(false);

        DelayedDestroy(9, 1);
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (charged) return;
        var puddle = collision.gameObject.GetComponent<Puddle>();
        if (puddle)
        {
            if (puddle.charged) ChargeByLightning();
            return;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Debug.Log("trying to find creature.....");
        var creature = collision.gameObject.GetComponent<Creature>();
        // make player check
        if (creature && !creature.Friendly)
        {
            // Debug.Log("apply Buff.....");
            creature.ApplyBuff(typeof(PuddleSlowBuff));
            return;
        }
    }
}