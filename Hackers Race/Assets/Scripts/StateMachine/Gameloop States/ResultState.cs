using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultState : State
{
    [SerializeField] private Button toMenuButton;
    [SerializeField] private TextMeshProUGUI winner;
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
        winner.text = "WINNER IS PLAYER " + (FindObjectOfType<GameController>().WINNER+1).ToString();
    }

    public override void OnExitState()
    {

    }

    public override void OnUpdateState()
    {

    }
}
