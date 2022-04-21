using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    List<GameObject> monstersGameObjects = new List<GameObject>();
    List<Creature> monsters = new List<Creature>();

    public void CreateMonster(string monsterName, Vector2 pos)
    {
        var monster = Instantiate(Resources.Load($"Prefabs/Monsters/{monsterName}"), pos, Quaternion.identity) as GameObject;
        monstersGameObjects.Add(monster);
        monsters.Add(monster.GetComponent<Creature>());
    }

    public List<Creature> GetMostersInArea(Vector2 center, float radius)
    {
        int i = 0;
        var monstersInArea = new List<Creature>();

        while (i != monstersGameObjects.Count)
        {
            var monsterGameObject = monstersGameObjects[i];
            if (monsterGameObject == null)  // lazy delete monster
            {
                monstersGameObjects.RemoveAt(i);
                monsters.RemoveAt(i);
            }
            else
            {
                if (((Vector2)monsterGameObject.transform.position - center).magnitude <= radius)
                {
                    monstersInArea.Add(monsters[i]);
                }
                i += 1;
            }
        }

        return monstersInArea;
    }

    // todo remove?
    private void Start()
    {
        var creatures = FindObjectsOfType<Creature>();
        foreach (var creature in creatures)
        {
            if (!creature.Friendly)
            {
                monstersGameObjects.Add(creature.gameObject);
                monsters.Add(creature);
            }
        }

    }
}
