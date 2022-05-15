using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    PlayerController playerController;
    Utils.LazyCollection<Monster> monsters = new();
    Utils.LazyCollection<Monster> friendlyCreatures = new();

    Dictionary<Type, Dictionary<string, float>> monsterToStatsMapping = new Dictionary<Type, Dictionary<string, float>>()
    { { typeof(Zombie), new Dictionary<string, float>() {
        { "health", 7f },
        { "maxHealth", 7f },
        { "damage", 1.5f },
        { "speed", 0.4f },
        { "friendly", 0},
       }},
      { typeof(Skeleton), new Dictionary<string, float>() {
        { "health", 5f },
        { "maxHealth", 5f },
        { "damage", 1f },
        { "speed", 0.5f },
        { "friendly", 0},
       }},
        { typeof(Golem), new Dictionary<string, float>() {
        { "health", 8f },
        { "maxHealth", 8f },
        { "damage", 2.35f },
        { "speed", 0.25f },
        { "friendly", 1},
       }},
    };

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

        var monstersArray = FindObjectsOfType<Monster>();
        foreach (var monster in monstersArray)
        {
            HandleMonster(monster);
        }
    }

    private void Update()
    {
        SetAttackTargets();
    }

    // TODO maybe use k-d tree instead!!!
    private void SetAttackTargets()
    {
        MakeSomeActionWithCreatures(monsters, (Monster monster) =>
        {
            if (!monster.ShouldChaseTarget()) monster.SetMonsterTarget(GetNearestMonster(monster.transform.position, true));
        });
        MakeSomeActionWithCreatures(friendlyCreatures, (Monster monster) =>
        {
            if (!monster.ShouldChaseTarget()) monster.SetMonsterTarget(GetNearestMonster(monster.transform.position, false));
        }
        );
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
                bool friendly = stats["friendly"] > 0;

                monster.SetAttributes(health, maxHealth, damage, speed, friendly);
                return;
            }
        }

        //Debug.LogError("no such monster: " + monsterType);
    }

    private void HandleMonster(Monster monster)
    {
        SetMonsterAttributes(monster);
        if (monster.Friendly)
            friendlyCreatures.Add(monster);
        else
            monsters.Add(monster);
    }

    public void CreateMonster(string monsterName, Vector2 pos)
    {
        var monsterObject = Instantiate(Resources.Load($"Prefabs/Monsters/{monsterName}"), pos, Quaternion.identity) as GameObject;
        var monster = monsterObject.GetComponent<Monster>();
        HandleMonster(monster);
    }

    public List<Creature> GetMonstersInArea(Vector2 center, float radius, bool enemies = true, bool friendly = false)
    {
        var monstersInArea = new List<Creature>();

        if (!enemies & !friendly)
            Debug.LogWarning("asking for no monsters in monster controller!");
        if (enemies)
            FillWithMonstersInArea(center, radius, monsters, monstersInArea);
        if (friendly)
            FillWithMonstersInArea(center, radius, friendlyCreatures, monstersInArea);

        return monstersInArea;
    }

    private Creature GetNearestMonster(Vector2 position, bool isFriendly)
    {
        //Debug.Log("get nearest target for someone with pos " + position);
        if (isFriendly)
        {
            var friendlyCreature = GetNearest(position, friendlyCreatures);
            if (friendlyCreature == null)
                return playerController;
            if (playerController == null)
                return friendlyCreature;
            float toPlayerDistance = ((Vector2)playerController.transform.position - position).sqrMagnitude;
            if (toPlayerDistance <= ((Vector2)friendlyCreature.transform.position - position).sqrMagnitude)
            {
                return playerController;
            }
            return friendlyCreature;
        }
        else
        {
            return GetNearest(position, monsters);
        }
    }

    private Creature GetNearest(Vector2 position, Utils.LazyCollection<Monster> creatures)
    {
        float minPos = float.MaxValue;
        Creature nearestCreature = null;
        for (int i = 0; i < creatures.Count(); ++i)
        {
            //Debug.Log("get nearest");
            var creature = creatures.At(i);
            if (creature != null)
            {
                float newPos = ((Vector2)creature.transform.position - position).sqrMagnitude;
                if (newPos < minPos)
                {
                    minPos = newPos;
                    nearestCreature = creature;
                }
            }
        }
        return nearestCreature;
    }

    private void FillWithMonstersInArea(Vector2 center, float radius, Utils.LazyCollection<Monster> creatures, List<Creature> outCreatures)
    {
        for (int i = 0; i < creatures.Count(); ++i)
        {
            //Debug.Log("get monster in fill");
            var creature = creatures.At(i);
            if (((Vector2)creature.transform.position - center).sqrMagnitude <= radius * radius)
            {
                outCreatures.Add(creature);
            }
        }
    }

    private void MakeSomeActionWithCreatures(Utils.LazyCollection<Monster> creatures, Action<Monster> action)
    {
        for (int i = 0; i < creatures.Count(); ++i)
        {
            var creature = creatures.At(i);
            // here can be some race condition
            if (creature != null)
                action(creature);
        }
    }
}
