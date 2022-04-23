using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public abstract float GetManaCost();

    public abstract void CastSpell(Vector2 start, Vector2 end, Vector2 center);
}
