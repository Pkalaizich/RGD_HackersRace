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
        bool bothOnKeyboard = false;
        for(int i = 0;i<2;i++)
        {
            bothOnKeyboard = PlayersManager.Instance.INFO[i].playerBehaviour.PLAYER_INPUT.currentControlScheme=="Keyboard"?true:false;
            if(!bothOnKeyboard)
            {
                break;
            }
        }
        if(bothOnKeyboard)
        {
            PlayersManager.Instance.ChangeCurrentActionMaps(2);
        }
        else
        {
            PlayersManager.Instance.ChangeCurrentActionMaps(0);
        }        
        gameController.SetUp();
    }

    public override void OnExitState()
    {

    }

    public override void OnUpdateState()
    {

    }
}
