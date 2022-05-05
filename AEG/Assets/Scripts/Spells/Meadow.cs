using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meadow : Spell
{
    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        transform.position = center;
    }

    public override float GetManaCost()
    {
        return 5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
