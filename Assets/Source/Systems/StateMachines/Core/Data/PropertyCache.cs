using System;

public class PropertyCache
{
    public Func<object> getDelegate;
    public Action<object> setDelegate;

    public TValue Get<TValue>()
    {
        return (TValue)getDelegate();
    }

    public void Set<TValue>(TValue value)
    {
        setDelegate(value);
    }
}
