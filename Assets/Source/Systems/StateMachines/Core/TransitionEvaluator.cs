using System;
using UnityEngine;

public class TransitionEvaluator : MonoBehaviour
{
    private ILogicalStateTransition[] transtions;

    private void Awake()
    {
        transtions = GetComponentsInChildren<ILogicalStateTransition>();
    }

    public bool Evaluate(StateMachineRegistry registry, out ILogicalStateTransition hitTransition)
    {
        hitTransition = null;
        foreach (var logicalStateTransition in transtions)
        {
            if (logicalStateTransition.Evaluate(registry))
            {
                hitTransition = logicalStateTransition;
                return true;
            }
        }
        return false;
    }
}
