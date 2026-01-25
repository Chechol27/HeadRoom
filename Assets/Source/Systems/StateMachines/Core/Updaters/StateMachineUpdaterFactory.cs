using System;
using UnityEngine;

namespace StateMachines.Core.Update
{

    public enum StateMachineUpdateMode
    {
        Update,
        FixedUpdate,
        LateUpdate
    }

    public static class StateMachineUpdaterFactory
    {
        public static StateMachineUpdater CreateStateMachineUpdater(GameObject anchor,
            StateMachineUpdateMode updateMode, Action executeDelegate)
        {
            StateMachineUpdater ret;
            switch (updateMode)
            {
                case StateMachineUpdateMode.Update:
                    ret = anchor.AddComponent<GameTimeStateMachineUpdater>();
                    break;
                case StateMachineUpdateMode.FixedUpdate:
                    ret = anchor.AddComponent<PhysicsTimeStateMachineUpdater>();
                    break;
                case StateMachineUpdateMode.LateUpdate:
                    ret = anchor.AddComponent<PostGameTimeStateMachineUpdater>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(updateMode), updateMode, null);
            }

            ret.SetupDelegate(executeDelegate);
            return ret;
        }
    }
}
