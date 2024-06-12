using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuState : State
{
    [SerializeField] private Button playButton;
    private void Start()
    {
        playButton.onClick.AddListener(()=>
        {
            stateController.NextState();
            playButton.interactable = false;
        });
    }
    public override void OnEnterState()
    {
        playButton.interactable = true;
    }

    public override void OnExitState()
    {

    }

    public override void OnUpdateState()
    {

    }
}
