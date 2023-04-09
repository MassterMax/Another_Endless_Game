using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreesSpawner : MonoBehaviour
{
    [SerializeField] GameObject treePrefab;
    private int numberOfTrees = 64;
    private int absMaxX = 12;
    private int absMaxY = 10;

    List<GameObject> trees = new List<GameObject>();
    void Start()
    {
        if ((2 * absMaxX + 1) * (2 * absMaxY + 1) - 25 < numberOfTrees * 2)
            throw new System.Exception("too many trees");

        var cnt = 0;
        while (cnt < numberOfTrees)
        {
            var x = Random.Range(-absMaxX, absMaxX + 1);
            var y = Random.Range(-absMaxY, absMaxY + 1);
            var pos = new Vector3(x, y, 0);
            if (Mathf.Abs(x) > 2 && Mathf.Abs(y) > 2 && !CheckIfExists(pos))
            {
                var tree = Instantiate(treePrefab, pos, Quaternion.identity);
                trees.Add(tree);
                cnt += 1;
            }
        }
    }

    bool CheckIfExists(Vector3 pos)
    {
        foreach (var tree in trees)
        {
            if (tree.transform.position == pos) return true;
        }
        return false;
    }

}
