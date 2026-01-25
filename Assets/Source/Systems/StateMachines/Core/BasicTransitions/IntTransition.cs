using StateMachines.Core;
using StateMachines.Data;
using UnityEngine;

namespace StateMachines.Transitions
{
    public class IntTransition : MonoBehaviour, ILogicalStateTransition
    {
        [SerializeField] private string registryProperty;
        [SerializeField] private NumericComparison comparison;
        [SerializeField] private int threshold;

        [SerializeField] private Component to;

        public ILogicalState To
        {
            get => (ILogicalState)to;
            set => to = (Component)value;
        }

        public bool Evaluate(StateMachineRegistry registry)
        {
            int value = registry.Get<int>(registryProperty);
            switch (comparison)
            {
                case NumericComparison.Equal:
                    return value == threshold;
                case NumericComparison.NotEqual:
                    return value != threshold;
                case NumericComparison.MoreThan:
                    return value > threshold;
                case NumericComparison.LessThan:
                    return value < threshold;
            }

            return false;
        }
    }
}
