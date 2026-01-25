using StateMachines.Core;
using StateMachines.Data;
using UnityEngine;

namespace StateMachines.Transitions
{
    public class BoolTransition : MonoBehaviour, ILogicalStateTransition
    {
        [SerializeField] private string registryProperty;
        [SerializeField] private bool comparisonValue;
        [SerializeField] private Component to;

        public ILogicalState To
        {
            get => (ILogicalState)to;
            set => to = (Component)value;
        }

        public bool Evaluate(StateMachineRegistry registry)
        {
            bool value = registry.Get<bool>(registryProperty);
            return value == comparisonValue;
        }
    }
}
