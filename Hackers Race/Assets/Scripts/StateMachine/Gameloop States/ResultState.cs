using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultState : State
{
    [SerializeField] private Button toMenuButton;
    [SerializeField] private TextMeshProUGUI winner;
    [SerializeField] private AudioClip menuMusic;
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
        AudioManager.instance.PlayBGMusic(menuMusic, true, 0, 0.7f);
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
