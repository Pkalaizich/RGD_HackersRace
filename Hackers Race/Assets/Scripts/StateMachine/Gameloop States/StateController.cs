using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class StateController : MonoBehaviour
{
    [Header("State Machine Settings")]
    [SerializeField] private List<StateInfo> states;
    private StateInfo currentState;
    private int stateIndex = 0;
    public int CURRENTSTATE => stateIndex;

    private readonly UnityEvent stateChanged = new UnityEvent();
    public UnityEvent OnStateChanged => stateChanged;

    private void Awake()
    {
        foreach (var state in states)
            state._Screen.SetActive(false);
        for (int i = states.Count - 1; i >= 0; i--)
        {
            if (states[i]._StateIsEnabled == false)
            {
                states.RemoveAt(i);
            }
        }
        SetStateByIndex(0);
    }

    void Update()
    {
        currentState._State.OnUpdateState();
    }

    public void SetStateByIndex(int index)
    {
        currentState?._State.OnExitState();
        stateIndex = index;
        SetScreen(stateIndex);
        currentState = states[stateIndex];
        currentState._State.OnEnterState();
        stateChanged?.Invoke();
    }

    public void SetStateByID(string id)
    {
        int index = states.FindIndex(x => x._StateID == id);
        SetStateByIndex(index);
    }

    public void NextState()
    {
        currentState._State.OnExitState();
        stateIndex++;
        if (stateIndex == states.Count)
            stateIndex = 0;
        SetScreen(stateIndex);
        currentState = states[stateIndex];
        currentState._State.OnEnterState();
        stateChanged?.Invoke();
    }

    public void PreviousState()
    {
        currentState._State.OnExitState();
        stateIndex--;
        if (stateIndex == -1)
            stateIndex = states.Count - 1;
        SetScreen(stateIndex);
        currentState = states[stateIndex];
        currentState._State.OnEnterState();
        stateChanged?.Invoke();
    }

    public void SetScreen(int index)
    {
        foreach (var state in states)
            state._Screen.SetActive(false);
        states[index]._Screen.SetActive(true);
    }

    public StateInfo GetCurrentState() { return currentState; }
    public List<StateInfo> GetStates() { return states; }
    public List<GameObject> GetScreens()
    {
        List<GameObject> screens = new List<GameObject>();
        foreach (var state in states)
            screens.Add(state._Screen);
        return screens;
    }
}

[System.Serializable]
public class StateInfo
{
    public string _StateID = string.Empty;
    public State _State;
    public GameObject _Screen;
    public bool _StateIsEnabled = true;
}
