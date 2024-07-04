using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayState : State
{
    [SerializeField] private GameController gameController;
    public override void OnEnterState()
    {
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
