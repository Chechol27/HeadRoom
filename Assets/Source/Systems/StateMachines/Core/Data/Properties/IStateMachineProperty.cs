public interface IStateMachineProperty
{
    public string Name { get; set; }
}

public abstract class StateMachineProperty<TValue> : IStateMachineProperty
{
    public TValue Value { get; set; }
    public object GenericValue => Value;
    public string Name { get; set; }

    public static implicit operator TValue(StateMachineProperty<TValue> val)
    {
        return val.Value;
    }
}
