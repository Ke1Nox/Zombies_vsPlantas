using System;
using System.Collections.Generic;

public class MyStack<T>
{
    private List<T> elements = new List<T>();

    public void Push(T item)
    {
        elements.Add(item);
    }

    public T Pop()
    {
        if (elements.Count == 0)
            throw new InvalidOperationException("La pila está vacía.");

        T item = elements[elements.Count - 1];
        elements.RemoveAt(elements.Count - 1);
        return item;
    }

    public T Peek()
    {
        if (elements.Count == 0)
            throw new InvalidOperationException("La pila está vacía.");

        return elements[elements.Count - 1];
    }

    public int Count => elements.Count;

    public bool IsEmpty => elements.Count == 0;

    public void Clear()
    {
        elements.Clear();
    }
}

