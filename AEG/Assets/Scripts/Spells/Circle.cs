using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour, ICastable
{
    public void CastSpell(Vector2 start, Vector2 end, Vector2 center)
    {
        this.transform.position = center;
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
