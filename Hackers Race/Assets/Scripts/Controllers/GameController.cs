using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;

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

    private UnityEvent<int> playerWon = new UnityEvent <int>();
    public UnityEvent<int> OnPlayerWon => playerWon;

    [SerializeField] private List<Image> finalMessage;
    [SerializeField] private Sprite wonSprite;
    [SerializeField] private Sprite loseSprite;

    private void Start()
    {
        stateController =FindObjectOfType<StateController>();
        playerWon.AddListener(GameOver);
    }
    public void SetUp()
    {
        for (int i = 0; i < finalMessage.Count; i++)
        {
            finalMessage[i].gameObject.SetActive(false);            
        }
        foreach (PlayerController controller in playerControllers)
        {
            controller.SetInitialValues();          
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
                controller.GameEnded();
            }
            StartCoroutine(DelayedStateChange());
            for(int i=0;i<finalMessage.Count;i++)
            {
                finalMessage[i].gameObject.SetActive(true);
                finalMessage[i].sprite = i==winnerIndex? wonSprite : loseSprite;
            }
        }
    }   

    private IEnumerator DelayedStateChange()
    {
        yield return new WaitForSeconds(1.5f);
        stateController.NextState();
    }
}

