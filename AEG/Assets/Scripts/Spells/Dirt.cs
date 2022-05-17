using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : CombinedSpell
{
    private float delayedDestroyStart;
    private float spellDuration = 15f;

    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        // pass
        transform.position = end;
    }

    // Start is called before the first frame update
    void Start()
    {
        DelayedDestroy(spellDuration - 1, 1);
        delayedDestroyStart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // todo this is hardcoding, I should remove this 
    public void CallDelayedDestroy()
    {
        if (Time.time - delayedDestroyStart < spellDuration - 1)
        {
            DelayedDestroy(0, 1);
            delayedDestroyStart = Time.time - spellDuration;
        }
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
