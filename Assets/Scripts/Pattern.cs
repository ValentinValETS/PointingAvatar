using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;

public class Pattern<T>
{
    List<T> objects;

    public Pattern (List<T> objects)
    {
        this.objects = objects;
    }

    public T FIFO()
    {
        if (objects.Count == 0)
            throw new ExperimentalTrialCompletedException();

        T target = objects[0];
        objects.RemoveAt(0);
        return target;
    }

    public int Count()
    {
        return objects.Count;
    }
}
