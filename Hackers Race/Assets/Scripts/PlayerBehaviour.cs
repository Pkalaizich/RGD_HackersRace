using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput PLAYER_INPUT=> playerInput;    
    private PlayerController playerController;    

    private TestControl testControl;

    private int currentMap = 0;
    

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var player = FindObjectsOfType<PlayerController>();
        var index = playerInput.playerIndex;        
        playerController = player.FirstOrDefault(x => x.GetPlayerIndex() == index);
        PlayersManager.Instance.AddPlayer(this, index);
        StartCoroutine(EnableMapWithDelay());
    }

    public void OnButtonPressed(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            playerController.ButtonPressed(context);
        }        
    }

    public void EnableActionMap(int mapToActivate)
    {
        playerInput.actions.actionMaps[currentMap].Disable();
        playerInput.actions.actionMaps[mapToActivate].Enable();
        currentMap= mapToActivate;
    }

    public void DisconnectPlayer()
    {
        PlayersManager.Instance.RemovePlayer(playerInput.playerIndex);
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        EnableActionMap(0);
    }

    private IEnumerator EnableMapWithDelay()
    {
        EnableActionMap(0);
        yield return new WaitForSeconds(0.1f);
        if (FindObjectOfType<StateController>().CURRENTSTATE == 1)
        {
            EnableActionMap(1);
        }
        else
        {
            EnableActionMap(0);
        }
    }
}
