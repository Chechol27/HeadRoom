namespace StateMachines.Core.Update
{
    public class PhysicsTimeStateMachineUpdater : StateMachineUpdater
    {
        private void Update()
        {
            updateDelegate?.Invoke();
        }
    }
}