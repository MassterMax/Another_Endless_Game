using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    private Drawing drawingManager;
    private Dictionary<string, string> spellPrefixToSpell = new Dictionary<string, string>() {
        { "lightning", "lightning_1" },
        { "circle", "bubble1"} };


    private void Awake()
    {
        drawingManager = FindObjectOfType<Drawing>();
    }

    public void CastSpell(string name)
    {
        foreach (var el in spellPrefixToSpell)
        {
            if (name.StartsWith(el.Key))
            {
                var coords = drawingManager.coords;  // todo maybe method that give us last coord
                //Lightning lightning = 
                GameObject spell = Instantiate(Resources.Load($"Prefabs/{el.Value}"), coords[coords.Count - 1], Quaternion.identity) as GameObject;
                //print(spell);
                //print(spell == null);
                spell.GetComponent<ICastable>().CastSpell(coords[0], coords[coords.Count - 1], (coords[0] + coords[coords.Count - 1]) / 2);
                //spell.CastSpell(coords[0], coords[coords.Count - 1], (coords[0] + coords[coords.Count - 1]) / 2);
                return;
            }
        }
        Debug.LogWarning("unknown spell: " + name);
    }
}
