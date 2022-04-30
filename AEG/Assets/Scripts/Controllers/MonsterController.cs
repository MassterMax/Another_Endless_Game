using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    PlayerController playerController;
    List<GameObject> monstersGameObjects = new List<GameObject>();
    List<Monster> monsters = new List<Monster>();

    Dictionary<Type, Dictionary<string, float>> monsterToStatsMapping = new Dictionary<Type, Dictionary<string, float>>()
    { { typeof(Zombie), new Dictionary<string, float>() {
        { "health", 6f },
        { "maxHealth", 6f },
        { "damage", 1f },
        { "speed", 0.3f }
       }},
      { typeof(Skeleton), new Dictionary<string, float>() {
        { "health", 4f },
        { "maxHealth", 4f },
        { "damage", 1f },
        { "speed", 0.6f }
       }}
    };

    private float GetMonsterParam(Monster monster, string paramName)
    {
        Type monsterType = monster.GetType();
        foreach (var el in monsterToStatsMapping)
        {
            if (monsterType == el.Key)
            {
                foreach (var innerEl in el.Value)
                {
                    if (innerEl.Key.Equals(paramName))
                    {
                        return innerEl.Value;
                    }
                }
            }
        }

        Debug.LogError("no such monster or key: " + monsterType + " parameter: " + paramName);
        return -1;
    }

    private void SetMonsterAttributes(Monster monster)
    {
        Type monsterType = monster.GetType();
        foreach (var el in monsterToStatsMapping)
        {
            if (monsterType.Equals(el.Key))
            {
                var stats = el.Value;
                float health = stats["health"];
                float maxHealth = stats["maxHealth"];
                float damage = stats["damage"];
                float speed = stats["speed"];

                monster.SetAttributes(health, maxHealth, damage, speed);
                return;
            }
        }

        Debug.LogError("no such monster: " + monsterType);
    }

    //public void CreatePlayer()
    //{
    //    Instantiate(Resources.Load("Prefabs/Player/Player"), UnityEngine.Random.insideUnitCircle.normalized, Quaternion.identity);
    //}

    public void CreateMonster(string monsterName, Vector2 pos)
    {
        var monsterObject = Instantiate(Resources.Load($"Prefabs/Monsters/{monsterName}"), pos, Quaternion.identity) as GameObject;
        var monster = monsterObject.GetComponent<Monster>();
        HandleMonster(monster);
    }

    private void HandleMonster(Monster monster)
    {
        print("set player for: " + monster);
        print(playerController);
        monster.Player = playerController;
        SetMonsterAttributes(monster);

        monstersGameObjects.Add(monster.gameObject);
        monsters.Add(monster);
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
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

        var monstersArray = FindObjectsOfType<Monster>();
        foreach (var monster in monstersArray)
        {
            HandleMonster(monster);
        }
    }
}
