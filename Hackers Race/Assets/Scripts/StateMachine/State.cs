using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class State : MonoBehaviour
{
    //protected readonly UnityEvent enterAnimationCompleted = new UnityEvent();
    //public  UnityEvent OnEnterAnimationCompleted => enterAnimationCompleted;
    public StateController stateController;
    protected virtual void Awake()
    {
        stateController= FindObjectOfType<StateController>();
    }
    public abstract void OnEnterState();
    public abstract void OnUpdateState();
    public abstract void OnExitState();
}

