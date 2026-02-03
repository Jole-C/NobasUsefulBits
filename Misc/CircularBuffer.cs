using System;
using UnityEngine;

/// <summary>
/// Basic circular buffer implementation.
/// </summary>
/// <typeparam name="T"></typeparam>

public class CircularBuffer<T>
{
    T[] buffer;

    public int Read { get; private set; } = 0;
    public int Write { get; private set; } = 0;
    public int Size { get; private set; } = 0;
    public T[] Buffer => buffer;

    public CircularBuffer(int size)
    {
        Size = size;
        Array.Resize(ref buffer, Size);
    }

    public T ReadData()
    {
        T value = buffer[Read];
        Read++;

        WrapValues();

        return value;
    }

    public void WriteData(T data)
    {
        buffer[Write] = data;
        Write++;

        WrapValues();
    }

    public T GetPoint(int index)
    {
        return buffer[index];
    }

    public void WriteToPoint(int index, T data)
    {
        if (index < 0 || index >= buffer.Length - 1)
        {
            Debug.LogError("Invalid index.");
            return;
        }

        buffer[index] = data;
    }

    public void SetSize(int size)
    {
        Size = size;
        Array.Resize(ref buffer, Size);

        WrapValues();
    }

    public void ClearData()
    {
        for (int i = 0; i < buffer.Length - 1; i++)
        {
            buffer[i] = default(T);
        }
    }

    void WrapValues()
    {
        Read %= Size - 1;
        Write %= Size - 1;
    }
}