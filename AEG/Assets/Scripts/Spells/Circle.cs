using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : SpellKnowPlayer
{
    private float radius = 1f;

    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        if (player == null) return; // idiot check

        if (((Vector2)player.gameObject.transform.position - center).magnitude <= radius && !player.Protection)
        {
            player.Protection = true;
            transform.parent = player.transform;
            transform.localPosition = Vector3.zero;
        } else
        {
            transform.position = center;
            Destroy(gameObject, 2);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
