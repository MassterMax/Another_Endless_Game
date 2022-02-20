using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] float attackDuration = 0.15f;
    [SerializeField] Transform bulletStartPos;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject gameObjectContainer;
    [SerializeField] float attackDilation;
    float lastAttack = 0;

    bool playerIsAlive = true;
    float animationDuration = 0f;
    float animationBias = 0f;

    void Start()
    {
        StartCoroutine(Attack());
    }

    public float getAnimationBias()
    {
        return animationBias;
    }


    public void OnAttack()
    {
        animationDuration = Mathf.Max(attackDuration, animationDuration);

        if (Time.time - lastAttack >= attackDilation)
        {
            lastAttack = Time.time;
            Instantiate(bulletPrefab, bulletStartPos.position, transform.rotation, gameObjectContainer.transform);
        }
    }

    IEnumerator Attack()
    {
        while(playerIsAlive)
        {
            if (animationDuration > 0)
            {
                Debug.Log("should move");
                animationBias = Mathf.Abs(Mathf.Sin(16 * Time.time) / 6);
                animationDuration -= Time.fixedDeltaTime;
            } else
            {
                animationBias = 0f;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void OnAttackFinished()
    {

    }
}