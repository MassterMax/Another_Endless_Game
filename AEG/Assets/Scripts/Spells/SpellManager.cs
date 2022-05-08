using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// should be Singleton
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
        { "meadow", "Meadow" },
    };

    // mapping from two types to prefab name
    private Dictionary<List<Type>, string> combinationsList = new()
    {
        { new() { typeof(Puddle), typeof(Meadow) }, "Dirt" },
        { new() { typeof(Lightning), typeof(Meadow) }, "FireMeadow" },
    };

    // maybe use this approach with manacost or create a script with constants?
    private Dictionary<Type, Color> spellToColor = new Dictionary<Type, Color>() {
        { typeof(Lightning), Color.white},
        { typeof(Circle), new Color(0.4f, 0.6f, 1) },
        { typeof(Puddle), new Color(132f/255f, 229f/255f, 223f/255f) },
        { typeof(Meadow), new Color(153f/255f, 229f/255f, 80f/255f) },

    };

    // mapping from spell type to manacost
    private Dictionary<Type, float> manacostMapping = new Dictionary<Type, float>
    {
        { typeof(Circle), 5f },
        { typeof(Puddle), 10f },
        { typeof(Lightning), 15f },
        { typeof(Meadow), 20f },
    };

    private float defaultError = 1.5f;
    // mapping from spell name to max error user can make
    private Dictionary<string, float> errorMapping = new Dictionary<string, float>
    {
        { "circle", 2f },
        { "puddle", 1.2f },
        { "lightning", 1.4f },
        { "meadow", 3f },
    };

    private void Awake()
    {
        drawingManager = FindObjectOfType<Drawing>();
        monsterController = FindObjectOfType<MonsterController>();
        player = FindObjectOfType<PlayerController>();
    }

    public void CastSpell(string name, float error) // todo do not cast if error is high
    {
        // todo make custom error for any spells!!!!!!!!!!!!!!!
        // todo make custom error for any spells!!!!!!!!!!!!!!!
        // todo make custom error for any spells!!!!!!!!!!!!!!!
        // todo make custom error for any spells!!!!!!!!!!!!!!!
        // todo make custom error for any spells!!!!!!!!!!!!!!!
        // todo make custom error for any spells!!!!!!!!!!!!!!!
        // todo make custom error for any spells!!!!!!!!!!!!!!!
        // todo make custom error for any spells!!!!!!!!!!!!!!!
        Debug.LogWarning(name + " spell error is " + error);
        if (error > errorMapping.GetValueOrDefault(name, defaultError))
        {
            drawingManager.SetColor(Color.black);
            return;
        }

        foreach (var el in spellPrefixToSpellPrefab)
        {
            if (name.StartsWith(el.Key))
            {
                var spellPrefab = Resources.Load($"Prefabs/Spells/{el.Value}") as GameObject;
                float cost = manacostMapping[spellPrefab.GetComponent<Spell>().GetType()];

                if (!player.SpendMana(cost))
                {
                    Debug.Log("not enough mana!");
                    return;
                }

                GameObject spellObject = Instantiate(spellPrefab, drawingManager.GetLastPoint(), Quaternion.identity);
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

    private bool FindSpells(List<Type> types, Type type1, Type type2)
    {
        return type1.Equals(types[0]) && type2.Equals(types[1]) || type1.Equals(types[1]) && type2.Equals(types[0]);
    }

    public void CombineTwoSpells(Spell spell1, Spell spell2)
    {
        foreach (var spells in combinationsList)
        {
            if (FindSpells(spells.Key, spell1.GetType(), spell2.GetType()))
            {
                Vector2 newPos = (spell1.transform.position + spell2.transform.position) / 2;
                var spellPrefab = Resources.Load($"Prefabs/Spells/{spells.Value}");
                GameObject spellObject = Instantiate(spellPrefab, newPos, Quaternion.identity) as GameObject;
                CombinedSpell combinedSpell = spellObject.GetComponent<CombinedSpell>();

                Debug.Log("combination: " + combinedSpell.name);

                // we should set some requiremets befare casting TODO TODO TODO
                //SetSpellRequirements(spell);

                this.spells.Add(combinedSpell);
                // spellsGameObjects.Add(spellObject);
                // spells.Add(new KeyValuePair<GameObject, Spell>(spellObject, spell));

                combinedSpell.CastSpell(spell1.transform.position, spell2.transform.position, newPos);

                Destroy(spell1.gameObject);
                Destroy(spell2.gameObject);

                return;
            }
        }

        Debug.LogError("no such combination: " + spell1 + " and " + spell2);
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

        // Debug.Log("spells in area: ");
        // Debug.Log(string.Join(" ", spellsInArea));

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

        return foundSpells;
    }
}
