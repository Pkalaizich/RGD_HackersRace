using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControllerSelectionState : State
{
    [SerializeField] private List<GameObject> waitingMessages;
    [SerializeField] private List<GameObject> readyMessages;

    [SerializeField] private List<GameObject> controllerIndicator;
    [SerializeField] private List<GameObject> keyboardIndicator;

    [SerializeField] private GameObject countDownElements;
    [SerializeField] private TextMeshProUGUI countdownTMP;

    private Coroutine countDownCoroutine;
    public override void OnEnterState()
    {
        PlayersManager.Instance.ChangeCurrentActionMaps(1);

        countDownElements.SetActive(false);
        for(int i =0; i<2; i++)
        {
            SetPlayerStatus(i, PlayersManager.Instance.INFO[i].joined);
        }
        PlayersManager.OnPlayerJoined.AddListener(CheckPlayerStatus);
        PlayersManager.OnPlayerLeft.AddListener(CheckPlayerStatus);

        CheckIfAllReady();
    }

    public override void OnExitState()
    {
        PlayersManager.OnPlayerJoined.RemoveListener(CheckPlayerStatus);
        PlayersManager.OnPlayerLeft.RemoveListener(CheckPlayerStatus);

        StopCoroutine(countDownCoroutine);
        countDownCoroutine= null;
    }

    public override void OnUpdateState()
    {
        
    }

    public void CheckIfAllReady()
    {
        if (PlayersManager.Instance.AllPlayersJoined())
        {
            countDownCoroutine = StartCoroutine(Countdown());
        }
    }

    public void CheckPlayerStatus(int playerToCheck)
    {
        if (PlayersManager.Instance.INFO[playerToCheck].joined)
        {
            SetPlayerStatus(playerToCheck, true);
            CheckIfAllReady();
            if (PlayersManager.Instance.INFO[playerToCheck].playerBehaviour.PLAYER_INPUT.currentControlScheme =="Keyboard")
            {
                controllerIndicator[playerToCheck].SetActive(false);
                keyboardIndicator[playerToCheck].SetActive(true);
            }
            else
            {
                controllerIndicator[playerToCheck].SetActive(true);
                keyboardIndicator[playerToCheck].SetActive(false);
            }
        }
        else
        {
            SetPlayerStatus(playerToCheck, false);
            if (countDownCoroutine != null)
            {
                StopCoroutine(countDownCoroutine);
                countDownCoroutine = null;
                countDownElements.SetActive(false);
            }
        }
    }
    

    public void SetPlayerStatus(int player, bool status)
    {
        if (status)
        {
            waitingMessages[player].SetActive(false);
            readyMessages[player].SetActive(true);
        }
        else
        {
            waitingMessages[player].SetActive(true);
            readyMessages[player].SetActive(false);
        }
    }
    
    private IEnumerator Countdown()
    {
        countDownElements.SetActive(true);
        for(int i=1;i<4;i++)
        {
            countdownTMP.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        countdownTMP.text = "GO!";       
        yield return new WaitForSeconds(1);
        stateController.NextState();
    }
    
}
