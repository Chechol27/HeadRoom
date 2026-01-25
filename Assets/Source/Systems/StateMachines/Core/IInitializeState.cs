using System;
using StateMachines.Data;
using StateMachines.Decorators;
using UnityEngine;

/// <summary>
/// A logical state that requires to be initialized with the state machine's global data
/// </summary>

namespace StateMachines.Core
{
    public interface IInitializeState
    {
        public Action<StateMachineRegistry, Component> PreInitialize { get; set; }
        public Action<StateMachineRegistry, Component> PostInitialize { get; set; }

        public void RegisterDecorators(Component thisAsComponent)
        {
            foreach (StateDecorator decorator in thisAsComponent.GetComponents<StateDecorator>())
            {
                if (decorator.Order == StateDecoratorOrder.BeforeExecution)
                {
                    PreInitialize += decorator.Execute;
                }

                if (decorator.Order == StateDecoratorOrder.AfterExecution)
                {
                    PostInitialize += decorator.Execute;
                }
            }
        }

        public void Initialize(StateMachineRegistry globalData, Component stateMachine);
    }
}
