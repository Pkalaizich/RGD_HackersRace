using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultState : State
{
    [SerializeField] private Button toMenuButton;
    private void Start()
    {
        toMenuButton.onClick.AddListener(() =>
        {
            stateController.NextState();
            toMenuButton.interactable = false;
        });
    }
    public override void OnEnterState()
    {
        toMenuButton.interactable = true;
    }

    public override void OnExitState()
    {

    }

    public override void OnUpdateState()
    {

    }
}
