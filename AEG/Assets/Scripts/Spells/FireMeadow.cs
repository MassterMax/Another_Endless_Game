using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMeadow : CombinedSpell
{
    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        transform.position = end;
    }

    // Start is called before the first frame update
    void Start()
    {
        DelayedDestroy(9, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var creature = collision.gameObject.GetComponent<Creature>();
        // Debug.Log("creature " + collision.name + " colliding with fire meadow at " + Time.time);
        if (creature && !creature.Friendly)
        {
            creature.ApplyBuff(typeof(FireMeadowBuff));
            return;
        }
    }
}
