using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    private Drawing drawingManager;
    private MonsterController monsterController;
    private PlayerController player;

    private Dictionary<string, string> spellPrefixToSpell = new Dictionary<string, string>() {
        { "lightning", "lightning_1" },
        { "circle", "bubble1"} };


    private void Awake()
    {
        drawingManager = FindObjectOfType<Drawing>();
        monsterController = FindObjectOfType<MonsterController>();
        player = FindObjectOfType<PlayerController>();
    }

    public void CastSpell(string name) // todo do not cast if error is high
    {
        foreach (var el in spellPrefixToSpell)
        {
            if (name.StartsWith(el.Key))
            {
                var spellPrefab = Resources.Load($"Prefabs/{el.Value}");
                float cost = (((GameObject)spellPrefab).GetComponent<Spell>().GetManaCost());
                if (!player.SpendMana(cost))
                {
                    Debug.Log("not enough mana!");
                    return;
                }
                //print(spellPrefab);

                GameObject spellObject = Instantiate(spellPrefab, drawingManager.GetLastPoint(), Quaternion.identity) as GameObject;
                Spell spell = spellObject.GetComponent<Spell>();

                // print(spell is SpellKnowMonsters);
                // print(spell is Lightning);
                SetSpellRequirements(spell);


                spell.CastSpell(drawingManager.GetFirstPoint(), drawingManager.GetLastPoint(), drawingManager.GetMeanPoint());
                return;
            }
        }
        Debug.LogWarning("unknown spell: " + name);
    }

    private void SetSpellRequirements(Spell spell)
    {
        if (spell is SpellKnowMonsters)
        {
            ((SpellKnowMonsters)spell).SetMonsterController(monsterController);
        }
        if (spell is SpellKnowPlayer)
        {
            ((SpellKnowPlayer)spell).SetPlayer(player);
        }
    }
}
