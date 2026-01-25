namespace StateMachines.Core.Update
{
    public class PhysicsTimeStateMachineUpdater : StateMachineUpdater
    {
        private void FixedUpdate()
        {
            updateDelegate?.Invoke();
        }
    }
}