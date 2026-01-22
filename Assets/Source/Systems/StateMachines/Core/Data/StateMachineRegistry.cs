using System.Collections.Generic;
using UnityEngine;
using System;

namespace StateMachines.Data
{
    public abstract class StateMachineRegistry : MonoBehaviour
    {
        protected readonly Dictionary<string, Tuple<Func<object>, Action<object>>> properties = new();

        public PropertyCache CacheProperty(string propertyName)
        {
            var cacheTuple = properties[propertyName];
            return new PropertyCache { getDelegate = cacheTuple.Item1, setDelegate = cacheTuple.Item2 };
        }

        public TValue Get<TValue>(string propertyName)
        {
            if (properties.TryGetValue(propertyName, out var val))
            {
                return (TValue)val.Item1();
            }

            throw new ArgumentException($"Key \"{propertyName}\" not found in registry {this}");
        }

        public void Set<TValue>(string propertyName, TValue value)
        {
            if (properties.ContainsKey(propertyName))
            {
                properties[propertyName].Item2(value);
                return;
            }

            throw new ArgumentException($"Key \"{propertyName}\" not found in registry {this}");
        }
    }
}
