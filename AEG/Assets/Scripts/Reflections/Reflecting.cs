using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflecting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var spriteMask = gameObject.AddComponent<SpriteMask>();
        spriteMask.sprite = GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
