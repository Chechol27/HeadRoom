# State Machine Framework

HeadRoom requires a versatile character motion system that can be up- and down-scaled efficiently and seamlessly enough 
to accomodate to the core principle of ever-changing, always-innovative gameplay and level design. With such ambitious 
approach in mind, a hard-coded movement system won't cut it this motivates the creation of a state machine-like behavior
that defines motion paradigms for (theoretically) infinite environment possibilities.

This state machine framework intends to lay the foundation of the movement system without ignoring the possibilities of
being used in other contexts such as AI or Quests.

## Overview

The state machine framework as a macro-pipeline that could be described as such:

```mermaid

flowchart TB
    subgraph Build
        direction TB
        b_statesFromData("Discover States")
        b_transitionsFromData("Build Transitions")
        b_statesFromData --> b_transitionsFromData
    end
    
    subgraph Execute
        direction TB
        e_ExecuteCurrentState("Execute Current State")
        e_evaluateTransitions{"Arbitrate Transitions"}
        e_switch("Switch States")
        e_ExecuteCurrentState --> e_evaluateTransitions
        e_evaluateTransitions -->|true| e_switch
        e_evaluateTransitions -->|false| e_ExecuteCurrentState
        
    end
    g_stateMachineData("State Machine Data (State)")
    
    Build --> Execute
    g_stateMachineData -.Managed by.-> Execute
```

this is an interface driven system, this means everything in this figure is just a suggestion, but abiding by a standard
similar to this will allow for future tool compatibility en reusability.