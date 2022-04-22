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
}
