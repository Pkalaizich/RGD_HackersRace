using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayState : State
{
    [SerializeField] private GameController gameController;
    [SerializeField] private AudioClip gameMusic;
    public override void OnEnterState()
    {
        AudioManager.instance.PlayBGMusic(gameMusic, true, 0, 0.7f);
        PlayersManager.Instance.ChangeCurrentActionMaps(0);
        gameController.SetUp();
    }

    public override void OnExitState()
    {

    }

    public override void OnUpdateState()
    {

    }
}
