using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : Spell, IKnowSpellManager
{
    public SpellManager SpellManager { get; set; }

    bool charged = false;

    GameObject lightningChild;

    public override float GetManaCost()
    {
        return 15;
    }

    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        //base.CastSpell(start, end, center);
        //foreach (var monster in monsterController.GetMostersInArea(end, damageRadius))
        //{
        //    monster.TakeDamage(1);
        //}

        transform.position = center;
    }

    public void ChargeByLightning()
    {
        if (charged) return;
        charged = true;
        Debug.Log(gameObject.name + " is charged!!!");
        lightningChild.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        lightningChild = gameObject.transform.GetChild(0).gameObject;
        // lightningChild.transform.localPosition = Vector3.zero;
        lightningChild.SetActive(false);
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}