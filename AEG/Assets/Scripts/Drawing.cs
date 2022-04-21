using System.Collections.Generic;
using UnityEngine;

public class Drawing : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float minDistance = 10f;
    List<Vector2> coords = new List<Vector2>();
    LineRenderer lr;

    public Vector2 GetFirstPoint()
    {
        return coords[0];
    }

    public Vector2 GetLastPoint()
    {
        return coords[coords.Count - 1];
    }

    public Vector2 GetMeanPoint()
    {
        float x = 0;
        float y = 0;
        foreach(var point in coords)
        {
            x += point.x;
            y += point.y;
        }

        return new Vector2(x / coords.Count, y / coords.Count);
    }

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector2 newPos = Input.mousePosition;
            newPos = cam.ScreenToWorldPoint(newPos);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                coords = new List<Vector2> { newPos };
                lr.positionCount = 1;
                lr.SetPosition(0, newPos);
            }
            else
            {
                var lastPos = coords[coords.Count - 1];
                if ((lastPos - newPos).sqrMagnitude > minDistance)
                {
                    coords.Add(newPos);
                    lr.positionCount += 1;
                    lr.SetPosition(lr.positionCount - 1, newPos);
                }
            }
        }
    }
}
