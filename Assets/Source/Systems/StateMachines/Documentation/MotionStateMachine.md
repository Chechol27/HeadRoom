# Motion state machine implementation guide

As an implementation of the base state machine system, the movement system is configured as a hierarchical, prefab driven
modular system, in the following article, you will find a quick guide on how to implement a motion state machine using this system


## 1. State machine head

The first part of the motion state machine is the "head", the entrypoint that will manage state switches and execute the
current state whichever it is.

To  create a new motion state machine, you need to add the component **MotionStateMachine** to the object that will contain
every state in its hierarchy, it is not necessary for this object to be the root of the prefab or even the character model.

![img_2.png](img_2.png)

as you can see, the motion state machine component requires a **Player Input** component set to **Invoke C Shap Events** in
order to bind each state's input response properly. It also requires a **Motion State Machine Registry** component in order
to read and write data that will be shared between states.

## 2. Creating a new State

State creation aims to be as modular as possible, because of this, there's 2 sides of this process:
1. Code
2. Prefab

### Code:

A state needs to implement the **ILogicalState** interface in order to be discovered and manipulated by the state machine

you can also optionally use the interfaces **IInitializeState** and **IFinalizeState** to allow for initialization logic
(i.e: Input event binding, expensive data fetch caching, etc).

Another option a logical state may use is the **ITransitionalState**, this makes the state require a **TransitionEvaluator**
in order to execute transitions evaluations directly inside the state

Also, the motion is applied to the character by the state machine after the current state has been executed, this means
the states are not responsible for actually moving the character but instead updating a variable that represents the velocity,

these variables are **HorizontalMotion** and **VerticalMotion**

```csharp
public class ExampleMotionState : ILogicalState, IInitializeState, IFinalizeState
{
    //IInitializeState Implementation
    public void Initialize(StateMachineRegistry globalData, Component stateMachine)
    {
        //Execute initialization logic
        stateMachine.GetComponent<PlayerInput>().onActionTriggered += exampleMotionListener;
    }
    
    //ILogicalState Implementation
    public void Execute(StateMachineRegistry globalData, Component stateMachine)
    {
        //Execute motion logic, and update HorizontalMotion and/or VerticalMotion
        globalData.Set("HorizontalMotion", Vector3.forward); //State machine will move character's rigidbody to the world's front
    }
    
    //IFinalizeState Implementation
    public void Finalize(StateMachineRegistry globalData, Component stateMachine)
    {
        //Execute finalization logic
        stateMachine.GetComponent<PlayerInput>().onActionTriggered -= exampleMotionListener;
    }
}
```

additionaly, if the state requires a local data registry, it is strongly encouraged (albeit not enforced by code patterns)
to implement it using scriptable objects, so tweaks are decoupled from specific assets

if you need to add data to the MotionStateMachineRegistry, you can do so by modifying the class **MotionStateMachineRegistry**
it is important that the new data respects the following conventions:
1. Use the attribute **[StateMachineProperty]**.
2. Use auto properties {get; set;}

```csharp

public class MotionStateMachineRegistry : StateMachineRegistry
{
    //...
    [field:SerializeField][field:StateMachineProperty] public float NewFloatProperty {get; set;}
}
```

once you've modified the data class, make sure to generate it's runtime counterpart by pressing the "Generate Runtime Class"
in any instance of the registry present. this will generate the definitions that allow the system to access the class fields
without coupling the data and the logic classes.

![img_6.png](img_6.png)

### Prefab

Once the state has been coded, it can be included in the motion state machine by adding it as a **first level** child object
of the Motion State Machine Head object.

![img_3.png](img_3.png)

in this exmaple, the state "Horizontal Motion" is a special state that executes a collection of states sequentially as if
they were a single state, this allows to combine logic that modifies motion without having to code new redundant logic.

Inside the state's hierarchy, there's an optional object: the Transitions:

Transition evaluation occurs in a hierarchical manner as well, the "Root" **Transitions** object contains a **TransitionEvaluator**
component, that discovers and evaluates children transitions on-demand (ideally requersted by **ITransitionalState**'s)

![img_4.png](img_4.png)

![img_5.png](img_5.png)

so in this case, the state **HorizontalMotion** will switch to **WallRide** when the boolean value **Sliding** is evaluated
to be True








