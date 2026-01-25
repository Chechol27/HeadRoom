using System;
using StateMachines.Data;
using StateMachines.Decorators;
using UnityEngine;

/// <summary>
/// A logical state that requires to do release operations with the state machine's global data once the state has exited
/// </summary>
namespace StateMachines.Core
{
    public interface IFinalizeState
    {
        public Action<StateMachineRegistry, Component> PreFinalize { get; set; }
        public Action<StateMachineRegistry, Component> PostFinalize { get; set; }

        public void RegisterDecorators(Component thisAsComponent)
        {
            foreach (StateDecorator decorator in thisAsComponent.GetComponents<StateDecorator>())
            {
                if (decorator.Order == StateDecoratorOrder.BeforeExecution)
                {
                    PreFinalize += decorator.Execute;
                }

                if (decorator.Order == StateDecoratorOrder.AfterExecution)
                {
                    PostFinalize += decorator.Execute;
                }
            }
        }

        public void Finalize(StateMachineRegistry globalData, Component stateMachine);
    }
}
