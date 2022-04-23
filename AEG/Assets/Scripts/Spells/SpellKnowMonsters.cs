using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellKnowMonsters : Spell
{
    internal MonsterController monsterController;

    public void SetMonsterController(MonsterController monsterController)
    {
        this.monsterController = monsterController;
    }

    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        if (this.monsterController == null)
            throw new System.ArgumentNullException("monster controller should be set before calling cast spell");
    }
}
