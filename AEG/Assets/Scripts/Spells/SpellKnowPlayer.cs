using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellKnowPlayer : MonoBehaviour, ICastable
{
    internal Creature player;
    public abstract void CastSpell(Vector2 start, Vector2 end, Vector2 center);

    public void SetPlayer(Creature player)
    {
        this.player = player;
    }
}
