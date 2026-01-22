namespace StateMachines.Core
{
    public interface IStateMachine
    {
        /// <summary>
        /// Intended to evaluate the current state
        /// </summary>
        public void Evaluate();

        /// <summary>
        /// Intended to do pre- and post-logic for a state switch
        /// </summary>
        public void SwitchState(int nextStateId);

        public void SwitchState(ILogicalState targetState);
    }
}
