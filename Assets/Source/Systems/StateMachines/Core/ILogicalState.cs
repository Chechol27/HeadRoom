using System;
using StateMachines.Data;
using StateMachines.Decorators;
using UnityEngine;

namespace StateMachines.Core
{
    public interface ILogicalState
    {
        public Action<StateMachineRegistry, Component> PreExecute { get; set; }
        public Action<StateMachineRegistry, Component> PostExecute { get; set; }

        public void RegisterDecorators(Component thisAsComponent)
        {
            foreach (StateDecorator decorator in thisAsComponent.GetComponents<StateDecorator>())
            {
                if (decorator.Order == StateDecoratorOrder.BeforeExecution)
                {
                    PreExecute += decorator.Execute;
                }

                if (decorator.Order == StateDecoratorOrder.AfterExecution)
                {
                    PostExecute += decorator.Execute;
                }
            }
        }

        public void Execute(StateMachineRegistry globalData, Component stateMachine);
    }
}
