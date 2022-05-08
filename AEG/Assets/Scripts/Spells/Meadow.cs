using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meadow : Spell
{
    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        transform.position = center;

    }

    // Start is called before the first frame update
    void Start()
    {
        DelayedDestroy(4, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.LogWarning(gameObject.name + " collided with " + collision.name + " at: " + Time.time);
        var creature = collision.gameObject.GetComponent<Creature>();
        // make player check
        if (creature != null && creature.Friendly)
        {
            //Debug.Log("apply meadow buff on " + collision.name);
            creature.ApplyBuff(typeof(MeadowHealBuff));
            //return;
        }
        //Debug.LogWarning(gameObject.name + " stop colliding with " + collision.name + " at: " + Time.time);
    }
}
