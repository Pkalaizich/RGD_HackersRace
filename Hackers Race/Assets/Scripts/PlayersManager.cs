using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PlayersManager : MonoBehaviour
{
    public static UnityEvent <int> OnPlayerLeft = new UnityEvent<int>();
    public static UnityEvent<int> OnPlayerJoined = new UnityEvent<int>();

    #region Singleton
    static private PlayersManager instance;
    static public PlayersManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }
    #endregion


    private List<PlayerInfo> playersInfo = new List<PlayerInfo>();
    public List<PlayerInfo> INFO => playersInfo; 

    private void Start()
    {
        for(int i=0;i<2; i++)
        {
            playersInfo.Add(new PlayerInfo(null, false));
        }
    }

    [System.Serializable]
    public class PlayerInfo
    {
        public PlayerBehaviour playerBehaviour;
        public bool joined = false;

        public PlayerInfo(PlayerBehaviour newPlayerBehaviour, bool playerJoined)
        {
            playerBehaviour= newPlayerBehaviour;
            joined=playerJoined;
        }
    }

    public void AddPlayer(PlayerBehaviour newbehaviour, int playerIndex)
    {
        if (playersInfo[0].playerBehaviour!=null&&
            playersInfo[0].playerBehaviour.PLAYER_INPUT.currentControlScheme=="Keyboard" && playerIndex==1)
        {
            if(playersInfo[1].playerBehaviour != null &&
                playersInfo[1].playerBehaviour.PLAYER_INPUT.currentControlScheme == "Keyboard")
            {
                newbehaviour.DestroyPlayer();
            }
            else
            {
                playersInfo[playerIndex].playerBehaviour = newbehaviour;
                playersInfo[playerIndex].joined = true;

                OnPlayerJoined?.Invoke(playerIndex);
            }
        }
        else
        {
            playersInfo[playerIndex].playerBehaviour = newbehaviour;
            playersInfo[playerIndex].joined = true;

            OnPlayerJoined?.Invoke(playerIndex);
        }        
    }

    public void RemovePlayer(int playerIndex)
    {
        if(playerIndex==0
            && playersInfo[0].playerBehaviour!=null
            && playersInfo[0].playerBehaviour.PLAYER_INPUT.currentControlScheme == "Keyboard"
            && playersInfo[1].playerBehaviour != null
            && playersInfo[1].playerBehaviour.PLAYER_INPUT.currentControlScheme == "Keyboard")
        {
            playersInfo[1].playerBehaviour = null;
            playersInfo[1].joined = false;
        }
        playersInfo[playerIndex].playerBehaviour = null;
        playersInfo[playerIndex].joined = false;

        OnPlayerLeft?.Invoke(playerIndex);
    }

    public void ChangeCurrentActionMaps(int mapToActivate)
    {
        for(int i =0; i<playersInfo.Count; i++)
        {
            if(playersInfo[i].playerBehaviour!=null)
            {
                playersInfo[i].playerBehaviour.EnableActionMap(mapToActivate);
            }
        }
    }
    
    public bool AllPlayersJoined()
    {
        for(int i = 0; i<playersInfo.Count; i++) 
        {
            if (playersInfo[i].joined==false)
            {
                return false;
            }
        }
        return true;
    }
    
}
