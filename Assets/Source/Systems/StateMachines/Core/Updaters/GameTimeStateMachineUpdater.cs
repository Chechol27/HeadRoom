namespace StateMachines.Core.Update
{
    public class GameTimeStateMachineUpdater : StateMachineUpdater
    {
        private void FixedUpdate()
        {
            updateDelegate?.Invoke();
        }
    }
}
