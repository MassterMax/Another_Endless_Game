using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : Spell, IKnowPlayerController
{
    private float radius = 1f;

    public PlayerController PlayerController { get; set; }

    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        //base.CastSpell(start, end, center); // idiot check

        Vector2 playerPos = PlayerController.gameObject.transform.position;

        if ((playerPos - center).magnitude <= radius && !PlayerController.Protection)
        {
            PlayerController.Protection = true;
            transform.parent = PlayerController.transform;
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
