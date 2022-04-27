using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : SpellKnowMonsters
{
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

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
