// TECH DEBT NOTICE: This is a stump of the proposed generic registry system, neither used nor intended to be used in the current development cycle

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
