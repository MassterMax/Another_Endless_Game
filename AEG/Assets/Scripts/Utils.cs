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

            while (index < values.Count && values[index].Equals(null))
            {
                RemoveAt(index);
            }

            // Debug.Log(values.Count);

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