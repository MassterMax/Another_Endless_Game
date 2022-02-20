using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed;
    [SerializeField] float destroyDelay;

    void Start()
    {
        Object.Destroy(gameObject, destroyDelay);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * bulletSpeed * Time.deltaTime, Space.Self);
    }

    private void Destroy()
    {
        
    }
}
