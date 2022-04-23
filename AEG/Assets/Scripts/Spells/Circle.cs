using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : SpellKnowPlayer
{
    private float radius = 1f;

    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        base.CastSpell(start, end, center); // idiot check

        Vector2 playerPos = player.gameObject.transform.position;

        if ((playerPos - center).magnitude <= radius && !player.Protection)
        {
            player.Protection = true;
            transform.parent = player.transform;
            transform.localPosition = Vector3.zero;
        } else
        {
            transform.position = center;
            Destroy(gameObject, 0.5f);
        }
    }

    public override float GetManaCost()
    {
        return 35;
    }
}
