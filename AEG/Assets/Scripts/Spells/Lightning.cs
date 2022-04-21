using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : SpellKnowMonsters
{
    float damageRadius = 1f;

    public override void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        print("casting lightning...");
        //GameObject lightning = Instantiate(Resources.Load("Sprites/lightning_1.png"), end, Quaternion.identity) as GameObject;
        // do damage on end pos?
        //Zombie[] zombies = FindObjectsOfType<Zombie>();
        //for (int i = 0; i < zombies.Length; ++i)
        //{
        //    var zombie = zombies[i];
        //    Vector2 zombiePos = zombie.gameObject.transform.position;
        //    var distance = (zombiePos - end).magnitude;
        //    /*
        //    print("Zombie and lightning pos:");
        //    print(zombiePos);
        //    print(end);
        //    print(distance);
        //    */
        //    if (distance < damageRadius)
        //    {
        //        zombie.TakeDamage(1);
        //    }
        //}

        foreach(var monster in monsterController.GetMostersInArea(end, damageRadius))
        {
            monster.TakeDamage(1);
        }
    }

    void Start()
    {
        Destroy(gameObject, 1);
    }

    /*
    // Update is called once per frame
    void Update()
    {
        
    }*/
}
