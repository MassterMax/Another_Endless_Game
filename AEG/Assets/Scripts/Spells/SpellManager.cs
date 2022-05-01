using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    private Drawing drawingManager;
    private MonsterController monsterController;
    private PlayerController player;

    // List<GameObject> spellsGameObjects = new List<GameObject>();
    // List<Spell> spells = new List<Spell>();
    Utils.LazyCollection<Spell> spells = new Utils.LazyCollection<Spell>(); // it ruined my day =(

    // mapping from name of spell in gesture recogniser to spell prefab in resources
    private Dictionary<string, string> spellPrefixToSpellPrefab = new Dictionary<string, string>() {
        { "lightning", "lightning_1" },
        { "circle", "bubble1"},
        { "puddle", "puddle"},
    };

    // maybe use this approach with manacost or create a script with constants?
    private Dictionary<Type, Color> spellToColor = new Dictionary<Type, Color>() {
        { typeof(Lightning), Color.white},
        { typeof(Circle), new Color(0.4f, 0.6f, 1) },
        { typeof(Puddle), new Color(132f/255f, 229f/255f, 223f/255f) }
    };

    private void Awake()
    {
        drawingManager = FindObjectOfType<Drawing>();
        monsterController = FindObjectOfType<MonsterController>();
        player = FindObjectOfType<PlayerController>();
    }

    public void CastSpell(string name, float error) // todo do not cast if error is high
    {
        // todo make custom error for any spells
        if (error > 2f)
        {
            drawingManager.SetColor(Color.black);
            return;
        }

        foreach (var el in spellPrefixToSpellPrefab)
        {
            if (name.StartsWith(el.Key))
            {
                var spellPrefab = Resources.Load($"Prefabs/Spells/{el.Value}");
                float cost = (((GameObject)spellPrefab).GetComponent<Spell>().GetManaCost());
                if (!player.SpendMana(cost))
                {
                    Debug.Log("not enough mana!");
                    return;
                }

                GameObject spellObject = Instantiate(spellPrefab, drawingManager.GetLastPoint(), Quaternion.identity) as GameObject;
                Spell spell = spellObject.GetComponent<Spell>();

                Debug.Log("casting: " + spell.name);

                // we should set some requiremets befare casting
                SetSpellRequirements(spell);
                SetColorOfSpell(spell);

                spells.Add(spell);
                // spellsGameObjects.Add(spellObject);
                // spells.Add(new KeyValuePair<GameObject, Spell>(spellObject, spell));

                spell.CastSpell(drawingManager.GetFirstPoint(), drawingManager.GetLastPoint(), drawingManager.GetMeanPoint());
                return;
            }
        }
        Debug.LogWarning("unknown spell: " + name);
    }

    private void SetSpellRequirements(Spell spell)
    {
        if (spell is IKnowMonsterController)
        {
            ((IKnowMonsterController)spell).SetMonsterController(monsterController);
        }
        if (spell is IKnowPlayerController)
        {
            ((IKnowPlayerController)spell).SetPlayerController(player);
        }
        if (spell is IKnowSpellManager)
        {
            ((IKnowSpellManager)spell).SetSpellManager(this);
        }
    }

    private void SetColorOfSpell(Spell spell)
    {
        Type spellType = spell.GetType();
        if (!spellToColor.ContainsKey(spellType))
        {
            Debug.LogError("the spell not set in colors!");
            return;
        }

        drawingManager.SetColor(spellToColor[spellType]);
    }

    // todo maybe make lazy realiztaion of this list
    public List<Spell> GetSpellsInArea(Vector2 center, float radius)
    {
        // int i = 0;
        //Debug.Log("finding all spells...");
        var spellsInArea = new List<Spell>();

        for (int i = 0; i < spells.Count(); ++i)
        {
            //Debug.Log(spells.Count());
            var spell = spells.At(i);
            //Debug.Log(spell);
            if (((Vector2)spell.gameObject.transform.position - center).magnitude <= radius)
            {
                //Debug.Log("found in area: " + spell);
                spellsInArea.Add(spell);
            }
        }

        //while (i != spellsGameObjects.Count)
        //{
        //    var spellGameObject = spellsGameObjects[i];
        //    if (spellGameObject == null)  // lazy delete monster
        //    {
        //        spellsGameObjects.RemoveAt(i);
        //        spells.RemoveAt(i);
        //    }
        //    else
        //    {
        //        if (((Vector2)spellGameObject.transform.position - center).magnitude <= radius)
        //        {
        //            spellsInArea.Add(spells[i]);
        //        }
        //        i += 1;
        //    }
        //}

        return spellsInArea;
    }

    public List<Spell> GetSpellsByType<T>()
    {
        //int i = 0;
        var foundSpells = new List<Spell>();

        for (int i = 0; i < spells.Count(); ++i)
        {
            var spell = spells.At(i);
            if (spell is T)
            {
                foundSpells.Add(spell);
            }
        }


        //while (i != spellsGameObjects.Count)
        //{
        //    var spellGameObject = spellsGameObjects[i];
        //    if (spellGameObject == null)  // lazy delete
        //    {
        //        spellsGameObjects.RemoveAt(i);
        //        spells.RemoveAt(i);
        //    }
        //    else
        //    {
        //        if (spells[i] is T)
        //        {
        //            foundSpells.Add(spells[i]);
        //        }
        //        i += 1;
        //    }
        //}

        return foundSpells;
    }
}
