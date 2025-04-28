using System;
using System.Collections.Generic;

public class MyQueue<T>
{
    private List<T> elements = new List<T>();

    public void Enqueue(T item)
    {
        elements.Add(item);
    }

    public T Dequeue()
    {
        if (elements.Count == 0)
            throw new InvalidOperationException("La cola está vacía.");

        T item = elements[0];
        elements.RemoveAt(0);
        return item;
    }

    public T Peek()
    {
        if (elements.Count == 0)
            throw new InvalidOperationException("La cola está vacía.");

        return elements[0];
    }

    public int Count()
    {
         return elements.Count; 
    }

    public bool IsEmpty()
    {
         return elements.Count == 0; 
    }

    public void Clear()
    {
        elements.Clear();
    }
}

