using StateMachines.Data;
using UnityEngine;

namespace StateMachines.Decorators
{
    public enum StateDecoratorOrder
    {
        BeforeInitialization,
        AfterInitialization,
        BeforeExecution,
        AfterExecution,
        BeforeFinalization,
        AfterFinalization
    }

    public abstract class StateDecorator : MonoBehaviour
    {
        [field: SerializeField] public StateDecoratorOrder Order { get; set; }

        public abstract void Execute(StateMachineRegistry registry, Component stateMachine);
    }
}
