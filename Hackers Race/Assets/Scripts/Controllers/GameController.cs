using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public  class GameController : MonoBehaviour
{
    public  event Action<int> OnMatchComplete;
    public  event Action<int> OnRoundComplete;
    public  event Action<int> OnGameComplete;

    [SerializeField] private List<PlayerController> playerControllers;

    private bool gameEnded;
    private int lastWinner;

    private StateController stateController;
    public int WINNER => lastWinner;

    private void Start()
    {
        stateController =FindObjectOfType<StateController>();
    }
    public void SetUp()
    {
        foreach(PlayerController controller in playerControllers)
        {
            controller.ChangeMode(true);            
            controller.gameIsRunning = true;
            controller.RefreshAllCombos();
            controller.currentPoints= 0;
            controller.progressFiller.fillAmount= 0;            
        }
        gameEnded = false;
    }

    public  void StartGame()
    {

    }

    public void GameOver(int winnerIndex)
    {
        if(!gameEnded)
        {
            gameEnded = true;
            lastWinner= winnerIndex;
            foreach (PlayerController controller in playerControllers)
            {
                controller.ChangeMode(true);
                controller.gameIsRunning= false;
            }            
            stateController.NextState();
        }
    }   
}

