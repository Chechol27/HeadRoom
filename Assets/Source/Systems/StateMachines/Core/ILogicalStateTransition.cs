
using StateMachines.Data;

namespace StateMachines.Core
{
    public interface ILogicalStateTransition
    {
        ILogicalState To { get; set; }

        bool Evaluate(StateMachineRegistry registry);
    }
}