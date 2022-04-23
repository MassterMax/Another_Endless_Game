using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellKnowPlayer : Spell
{
    internal Creature player;

    public void SetPlayer(Creature player)
    {
        this.player = player;
    }

    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        if (this.player == null)
            throw new System.ArgumentNullException("player should be set before calling cast spell");
    }
}
