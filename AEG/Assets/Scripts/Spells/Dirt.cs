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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Debug.Log("trying to find creature.....");
        var creature = collision.gameObject.GetComponent<Creature>();
        // make player check
        if (creature && !creature.Friendly)
        {
            // Debug.Log("apply Buff.....");
            creature.ApplyBuff(typeof(DirtSlowBuff));
            return;
        }
    }
}
