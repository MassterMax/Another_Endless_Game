using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : CombinedSpell
{
    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        // pass
        transform.position = end;
    }

    // Start is called before the first frame update
    void Start()
    {
        DelayedDestroy(14, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var creature = collision.gameObject.GetComponent<Creature>();
        if (creature && !creature.Friendly)
        {
            creature.ApplyBuff(typeof(DirtSlowBuff));
            return;
        }
    }
}
