namespace StateMachines.Core.Update
{
    public class GameTimeStateMachineUpdater : StateMachineUpdater
    {
        private void Update()
        {
            updateDelegate?.Invoke();
        }
    }
}
