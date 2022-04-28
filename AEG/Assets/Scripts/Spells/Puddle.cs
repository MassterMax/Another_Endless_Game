using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : Spell, IKnowSpellManager
{
    public SpellManager SpellManager { get; set; }

    bool charged = false;
    GameObject lightningChild;
    private Collider2D objectCollider;

    public override float GetManaCost()
    {
        return 15;
    }

    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        transform.position = center;
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

    void Awake()
    {
        objectCollider = GetComponent<Collider2D>();
        lightningChild = gameObject.transform.GetChild(0).gameObject;
        lightningChild.SetActive(false);

        Destroy(gameObject, 10);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (charged) return;

        var puddle = collision.gameObject.GetComponent<Puddle>();
        if (!puddle) return;

        if (puddle.charged) ChargeByLightning();
    }
}