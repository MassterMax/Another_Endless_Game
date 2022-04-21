using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellKnowMonsters : MonoBehaviour, ICastable
{
    internal MonsterController monsterController;

    public abstract void CastSpell(Vector2 start, Vector2 end, Vector2 center);

    public void SetMonsterController(MonsterController monsterController)
    {
        this.monsterController = monsterController;
    }
}
