using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICastable
{
    void CastSpell(Vector2 start, Vector2 end, Vector2 center);
}
