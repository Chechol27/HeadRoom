
using StateMachines.Transitions;

namespace StateMachines.Core
{
    public interface ITransitionalState
    {
        TransitionEvaluator TransitionEvaluator { get; set; }
    }
}
