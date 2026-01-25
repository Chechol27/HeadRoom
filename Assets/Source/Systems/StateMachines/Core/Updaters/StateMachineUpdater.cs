using System;
using UnityEngine;

namespace StateMachines.Core.Update
{
    public abstract class StateMachineUpdater : MonoBehaviour
    {
        protected Action updateDelegate;
        internal virtual void SetupDelegate(Action targetUpdateDelegate)
        {
            updateDelegate = targetUpdateDelegate;
        }

        public Action UpdateDelegate => updateDelegate;
    }
}
