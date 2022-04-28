using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log("trying to get element on place: " + index);

        while (index < values.Count && values[index].Equals(null))
        {
            RemoveAt(index);
        }

        Debug.Log(values.Count);

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