using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : Spell, IKnowPlayerController
{
    private float radius = 1f;

    public PlayerController PlayerController { get; set; }
    public Reflectable reflection;

    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        //base.CastSpell(start, end, center); // idiot check

        Vector2 playerPos = PlayerController.gameObject.transform.position;

        // set defense bubble to player
        if ((playerPos - center).magnitude <= radius && !PlayerController.Protection)
        {
            PlayerController.Protection = true;
            transform.parent = PlayerController.transform;
            transform.localPosition = Vector3.zero;

            SetReflectionPosition();
        } else
        {
            // just destroy =(
            transform.position = center;
            Destroy(gameObject, 0.5f);
        }
    }

    private void SetReflectionPosition()
    {
        reflection = GetComponent<Reflectable>();
        if (reflection == null) return;

        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return;

        float circleHeight = spriteRenderer.bounds.size.y;

        reflection.SetPseudoYOffset((PlayerController.GetHeight() - circleHeight) / 2);
    }
}
