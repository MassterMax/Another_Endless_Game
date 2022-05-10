using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Launchable : DamagingThing
{
    const float g = 9.81f;

    protected bool inFlight = false;
    protected Vector2 direction;
    protected float currentX;
    protected float currentY;
    protected bool inTarget = false;

    public bool InFlight { get => inFlight; }
    public bool InTarget { get => inTarget; }

    private float GetLaunchAngle(float length, float velocity)
    {
        float sinValue = length * g / (velocity * velocity);

        if (sinValue > 1)
        {
            Debug.LogError("Launch failed: l * g should be lesser than V0^2! l: " + length + " V0: " + velocity);
            return 0;
        }

        float alpha = Mathf.Asin(length * g / (velocity * velocity));
        if (alpha > Mathf.PI / 2)
        {
            alpha = Mathf.PI - alpha;
        }
        return alpha / 2;  // l = V0^2 * sin2a / g  -->  a = asin(lg/V0^2) / 2
    }

    public virtual void LaunchObject(Vector2 destination, float velocity)
    {
        Vector2 startPos = gameObject.transform.position;
        direction = destination - startPos;
        float l = (startPos - destination).magnitude;

        print("lg/V^2: " + l * g / (velocity * velocity));

        float alpha = GetLaunchAngle(l, velocity);

        StartCoroutine(Launch(destination, velocity, l, alpha));
    }

    IEnumerator Launch(Vector2 destination, float velocity, float length, float alpha)
    {
        Debug.LogWarning("launched!");
        inFlight = true;

        float eps = 0.02f * velocity;  // todo maybe change
        float startTime = Time.time;
        float velocityX = velocity * Mathf.Cos(alpha);
        float velocityY = velocity * Mathf.Sin(alpha);

        float x1 = gameObject.transform.position.x;
        float y1 = gameObject.transform.position.y;
        float x2 = destination.x;
        float y2 = destination.y;

        Vector2 prev2Pos = gameObject.transform.position;
        bool skipStep = true;

        while (!inTarget && ((Vector2)gameObject.transform.position - destination).magnitude > eps)
        {
            //Debug.Log("step!");
            float time = Time.time - startTime;

            currentX = velocityX * time;
            currentY = velocityY * time - g * time * time / 2;

            float delta = velocityX * time / length;  // from 0 to 1

            // this is a linear transformation
            float extraX = (1 - delta) * x1 + delta * (x2 - length);
            float extraY = (1 - delta) * y1 + delta * y2;

            var newPos = new Vector2(currentX + extraX, currentY + extraY);
            gameObject.transform.position = newPos;

            if (skipStep)
            {
                skipStep = false;
            }
            else
            {
                float directionAngle = Utils.VectorToAngle(newPos - prev2Pos);
                gameObject.transform.eulerAngles = new Vector3(0, 0, directionAngle - 90);
                prev2Pos = newPos;
                skipStep = true;
            }

            // maybe make end of frame?
            yield return new WaitForFixedUpdate();
        }

        if (!inTarget) gameObject.transform.position = destination;
        inFlight = false;

        OnLanding();
    }

    protected virtual void OnLanding()
    {

    }

    public virtual bool LandInTarget(Transform target, float targetHeight = 0)
    {
        // skip this step if actually thing lies on the ground
        if (!inFlight)
        {
            return false;
        }

        this.transform.parent = target;
        inTarget = true;
        // todo remove after some duration
        return true;
    }
}
