using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    MonsterController monsterController;

    // we should have:
    // spawn rate
    // spawn count in one place
    // scale this in time
    // buff monsters in time!
    int monsterCount = 1;
    const float startSpawnDilation = 5f;  // seconds
    float spawnDilation = startSpawnDilation;  // seconds
    float lastSpawnTime;
    float startTime;

    void Start()
    {
        monsterController = FindObjectOfType<MonsterController>();
        //monsterController.CreateMonster("Zombie", new Vector2(1, 1));
        lastSpawnTime = Time.time;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSpawnTime > spawnDilation)
        {
            for (int i = 0; i < monsterCount; ++i)
            {
                var pos = Random.insideUnitCircle.normalized * 10f + monsterController.GetPlayerPos();
                string monster;
                if (Random.value < 0.75)
                {
                    monster = "Zombie";
                }
                else
                {
                    monster = "Skeleton";
                }

                monsterController.CreateMonster(monster, pos);
            }

            lastSpawnTime = Time.time;
            Debug.Log(monsterCount + " " + spawnDilation);
        }

        ChangeMonsterCount();
        ChangeSpawnDilation();
    }

    void ChangeMonsterCount()
    {
        monsterCount = (int)((Time.time - startTime) / 90f) + 1;

    }

    void ChangeSpawnDilation()
    {
        spawnDilation = Mathf.Max(3f, startSpawnDilation - (Time.time - startTime) / 90f);
    }
}
