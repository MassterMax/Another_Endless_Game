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
}
