using System;
using System.Collections.Generic;

public class ObjectPool<T> where T : class, new()
{
    private readonly Queue<T> objects;
    private readonly Action<T> resetAction;
    private readonly Action<T> initAction;
    private readonly Action<T> firstTimeInitAction;

    public ObjectPool(Action<T> resetAction = null,
        Action<T> initAction = null,
        Action<T> firstTimeInitAction = null)
    {
        objects = new Queue<T>();
        this.resetAction = resetAction;
        this.initAction = initAction;
        this.firstTimeInitAction = firstTimeInitAction;
    }

    public T New()
    {
        if (objects.Count > 0)
        {
            T t = objects.Dequeue();
            initAction?.Invoke(t);
            return t;
        }
        else
        {
            T t = new T();
            firstTimeInitAction?.Invoke(t);
            initAction?.Invoke(t);
            return t;
        }
    }

    public void Store(T obj)
    {
        resetAction?.Invoke(obj);
        objects.Enqueue(obj);
    }
}
