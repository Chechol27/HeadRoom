using System.Collections.Generic;
using UnityEngine;
using System;

//TODO: more efficient parameter fetch through hash tables
public class StateMachineRegistry : ScriptableObject
{
    private readonly Dictionary<Type, Dictionary<string, IStateMachineProperty>> collection = new Dictionary<Type, Dictionary<string, IStateMachineProperty>>();
    public void SetValue<TValue>(StateMachineProperty<TValue> value)
    {
        Type t = typeof(TValue);
        collection[t] ??= new Dictionary<string, IStateMachineProperty>();
        if (collection[t].ContainsKey(value.Name))
        {
            collection[t][value.Name] = value;
        }
        else
        {
            collection[t].Add(value.Name, value);
        }
    }
    
    public StateMachineProperty<TValue> GetValue<TValue>(string paramName)
    {
        Type t = typeof(TValue);
        try
        {
            return (StateMachineProperty<TValue>)collection[t][paramName];
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public bool TryGetValue<TValue>(string paramName, out StateMachineProperty<TValue> value)
    {
        Type t = typeof(TValue);
        try
        {
            value = (StateMachineProperty<TValue>)collection[t][paramName];
            return true;
        }
        catch (Exception e)
        {
            value = null;
            return false;
        }
    }
}
