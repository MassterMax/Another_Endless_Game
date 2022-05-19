using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDrawer : MonoBehaviour
{
    Dictionary<Type, int> buffTypeToPos = new()
    {
        { typeof(DirtSlowBuff), 0},
        { typeof(PuddleSlowBuff), 1 },
        { typeof(FireMeadowBuff), 2 },
        { typeof(MeadowHealBuff), 3 },
        {typeof(ElectricityDamageBuff), 4},
    };

    [SerializeField] List<GameObject> icons;

    void Start()
    {
        DrawBuffs(new List<Buff>());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DrawBuffs(List<Buff> buffs)
    {
        int count = buffs.Count;
        //if (count == 0)
        //{
        foreach (var icon in icons)
            icon.SetActive(false);
        //    return;
        //}

        int startX = -16 * ((count - 1) / 2) - 8 * ((count + 1) % 2);
        int i = 0;
        foreach (var buff in buffs)
        {
            if (!buffTypeToPos.ContainsKey(buff.GetType()))
            {
                Debug.LogError("no such icon for buff: " + buff.GetType());
                continue;
            }

            int index = buffTypeToPos[buff.GetType()];
            var icon = icons[index];
            icon.transform.localPosition = new Vector3(startX + 16 * i, icon.transform.localPosition.y);
            icon.SetActive(true);
            i += 1;
        }

    }
}
