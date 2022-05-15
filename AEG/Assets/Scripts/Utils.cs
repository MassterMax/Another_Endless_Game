using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Utils
{
    public static float VectorToAngle(Vector2 vector)
    {
        var angle = Vector2.Angle(Vector2.right, vector);
        if (vector.y < 0)
        {
            angle = -angle;
            if (vector.x < 0) angle = 360 + angle;
        }
        return angle;
    }

    public static Vector2 AngleToVector(float angle)
    {
        float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(angle * Mathf.Deg2Rad);

        return new Vector2(cos, sin).normalized;
    }

    public static Vector2 RotateVector(Vector2 vector, float angle)
    {
        float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(angle * Mathf.Deg2Rad);

        float tx = vector.x;
        float ty = vector.y;
        vector.x = (cos * tx) - (sin * ty);
        vector.y = (sin * tx) + (cos * ty);
        return vector.normalized;
    }

    public class LazyCollection<T>
    {
        private List<T> values;

        public LazyCollection()
        {
            values = new List<T>();
        }

        public void Add(T value)
        {
            values.Add(value);
        }

        public T At(int index)
        {
            // var value = values[index];
            // Debug.Log("trying to get element on place: " + index);

            while (index < values.Count && (values[index].Equals(null) || values == null))
            {
                RemoveAt(index);
            }

            // Debug.Log(values.Count);
            if (index >= values.Count) return default;

            return values[index];
        }

        public void RemoveAt(int index)
        {
            values.RemoveAt(index);
        }

        public int Count()
        {
            return values.Count;
        }
    }
}

public interface IDelayable
{
    public IEnumerator ExecuteAfterDelay(float delay, System.Action action)
    {
        // Debug.Log("going to sleep for " + delay);
        yield return new WaitForSeconds(delay);
        // yield return new WaitForFixedUpdate();
        // Debug.Log("YES!! will make action!");
        action();
        // Debug.Log("action made");
        // yield return new WaitForFixedUpdate();
    }
}

public interface IBlinkable
{
    public IEnumerator Blinking(SpriteRenderer spriteRenderer, int times, float dilation)
    {
        for (var n = 0; n < times; n++)
        {
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(dilation);
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(dilation);
        }
        spriteRenderer.enabled = true;
    }
}

public interface IFadestroyable
{
    // resize = true is equal to just resizing, false equal to fading

    public IEnumerator FadingDestroy(SpriteRenderer spriteRenderer, List<SpriteRenderer> childrenSR = null, float dilation = 0f, float time = 1f, bool resize = false)
    {
        if (spriteRenderer == null) yield break;
        if (childrenSR == null) childrenSR = new List<SpriteRenderer>();

        yield return new WaitForSeconds(dilation);

        List<Color> colorSteps = new List<Color> { Color.black * spriteRenderer.color.a / time };

        foreach (var child in childrenSR)
        {
            colorSteps.Add(Color.black * child.color.a / time);
        }

        float start = Time.time;
        Vector3 startScale = spriteRenderer.transform.localScale;

        //var colorStep = new Color(0, 0, 0, spriteRenderer.color.a);
        //Debug.Log(time + " time of fading");
        //Debug.LogWarning(childColorStep.Count + " count of child");

        while (Time.time - start < time)
        {
            if (resize)
                spriteRenderer.transform.localScale -= startScale * Time.fixedDeltaTime;
            else
            {
                spriteRenderer.color -= colorSteps[0] * Time.fixedDeltaTime;
                for (int i = 0; i < childrenSR.Count; ++i) // childSpriteRenderers.Count
                {
                    childrenSR[i].color -= colorSteps[i + 1] * Time.fixedDeltaTime;
                }
            }

            //Debug.Log(spriteRenderer.name + " is sleeping at " + Time.time + " alpha is " + spriteRenderer.color.a);
            //Debug.Log("step is " + childColorStep[0] * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        GameObject.Destroy(spriteRenderer.gameObject);
    }
}