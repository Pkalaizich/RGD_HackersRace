using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayersManager : MonoBehaviour
{
    [SerializeField] private List<PlayerInfo> playersInfo;

    [System.Serializable]
    public class PlayerInfo
    {
        public List<Image> buttonsImages;
        public bool joined = false;
    }

    public List<Image> ReturnImages()
    {
        PlayerInfo info = playersInfo.First(x => x.joined == false);
        info.joined = true;
        return info.buttonsImages;
    }
}
