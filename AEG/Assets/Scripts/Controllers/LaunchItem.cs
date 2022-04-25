using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchItem : MonoBehaviour
{
    float g = 9.81f;

    [SerializeField] GameObject gameObject1;
    [SerializeField] Vector2 destination1;
    [SerializeField] float vel;

    private Vector2 realStartPos;
    private bool set = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoNothing()
    {
        Debug.LogWarning("do nothing!");
        LaunchObject(gameObject1, destination1, vel);
    }

    public void LaunchObject(GameObject gameObject, Vector2 destination, float velocity)
    {
        Vector2 startPos = gameObject.transform.position;
        print(startPos);

        if (!set)
        {
            realStartPos = startPos;
            set = true;
        }
        else
        {
            startPos = realStartPos;
        }

        print(startPos);
        gameObject.transform.position = startPos;

        print(gameObject.transform.position);
        float l = (startPos - destination).magnitude;

        print("lg/V^2: " + l * g / (velocity * velocity));
        float sinValue = l * g / (velocity * velocity);
        if (sinValue > 1)
        {
            Debug.LogError("Launch failed: l * g should be lesser than V0^2! l: " + l + " V0: " + velocity);
            return;
        }

        float alpha = Mathf.Asin(l * g / (velocity * velocity));
        if (alpha > Mathf.PI / 2)
        {
            alpha = Mathf.PI - alpha;
        }
        //if (alpha < Mathf.PI / 2)
        //{
        //    alpha = Mathf.PI / 2 - alpha;
        //}

        alpha = alpha / 2;  // l = V0^2 * sin2a / g  -->  a = asin(lg/V0^2) / 2

        StartCoroutine(Launch(gameObject, destination, velocity, l, alpha));
    }

    IEnumerator Launch(GameObject gameObject, Vector2 destination, float velocity, float length, float alpha)
    {
        Debug.LogWarning("launched!");

        float eps = 0.02f * velocity;  // todo maybe change
        float startTime = Time.time;
        float velocityX = velocity * Mathf.Cos(alpha);
        float velocityY = velocity * Mathf.Sin(alpha);

        print(velocity);
        print(alpha);
        print(Mathf.Cos(alpha));
        print(Mathf.Sin(alpha));
        print("Vx: " + velocityX);
        print("Vy: " + velocityY);

        // yield break;

        float x1 = gameObject.transform.position.x;
        float y1 = gameObject.transform.position.y;
        float x2 = destination.x;
        float y2 = destination.y;

        Vector2 prev2Pos = gameObject.transform.position;
        bool skipStep = true;

        while (((Vector2)gameObject.transform.position - destination).magnitude > eps)
        {
            //Debug.Log("step!");
            float time = Time.time - startTime;
           
            float newX = velocityX * time;
            float newY = velocityY * time - g * time * time / 2;

            float delta = velocityX * time / length;  // from 0 to 1
            float extraX = (1 - delta) * x1 + delta * (x2 - length);
            float extraY = (1 - delta) * y1 + delta * y2;

            var newPos = new Vector2(newX + extraX, newY + extraY);
            gameObject.transform.position = newPos;

            if (skipStep)
            {
                skipStep = false;
            }
            else
            {
                print("direction vector: " + (newPos - prev2Pos).normalized);
                float directionAngle = PlayerController.VectorToAngle(newPos - prev2Pos);
                print("direction angle: " + directionAngle);
                gameObject.transform.eulerAngles = new Vector3(0, 0, directionAngle - 90);
                prev2Pos = newPos;
                skipStep = true;
            }

            yield return new WaitForFixedUpdate();
        }

        gameObject.transform.position = destination;
        Debug.LogWarning("in place!");
    }
}
