using UnityEngine;

public class FloatTransition: MonoBehaviour, ILogicalStateTransition
{
    [SerializeField] private string registryProperty;
    [SerializeField] private NumericComparison comparison;
    [SerializeField] private float threshold;

    [SerializeField] private Component to;
    public ILogicalState To { get => (ILogicalState)to; set => to = (Component)value; }
    public bool Evaluate(StateMachineRegistry registry)
    {
        float value = registry.Get<float>(registryProperty);
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
