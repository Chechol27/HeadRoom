using System;
using UnityEngine;

namespace StateMachines.Core.Update
{
    public class PostGameTimeStateMachineUpdater : StateMachineUpdater
    {
        private void LateUpdate()
        {
            updateDelegate?.Invoke();
        }
    }
}
