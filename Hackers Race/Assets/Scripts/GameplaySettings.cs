using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameplaySettings : MonoBehaviour
{
    //[Header("Hack Settings")]
    [SerializeField] private float pointsRequiredToWin = 100;
    public float REQUIRED_POINTS => pointsRequiredToWin;


    [SerializeField] private List<CombosLength> combosLengths= new List<CombosLength>();
    [SerializeField] private List<HackValues> hackValues = new List<HackValues>();
    [SerializeField] private List<Cooldowns> cooldownTimes = new List<Cooldowns>();   

    private static GameplaySettings instance;
    public static GameplaySettings Instance =>instance;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }            
        else
        {
            Destroy(this.gameObject);
        }
    }

    [System.Serializable]
    public class CombosLength
    {
        public string ComboID;
        public int comboLength;
    }

    [System.Serializable]
    public class HackValues
    {
        public string ComboID;
        public float hackPointsModification;
    }

    [System.Serializable]
    public class Cooldowns
    {
        public string ComboID;
        public float cooldownTime;
    }

    public int GetLength(string ID)
    {
        int length = 0;
        CombosLength combo = combosLengths.Find(x => x.ComboID == ID);
        if (combo != null)
            length = combo.comboLength;
        else
            Debug.LogError("NO HAY COMBO CON ESE ID");
        return length;
    }

    public float GetHackPointsValues(string ID)
    {
        float value = 0;
        HackValues hackValue = hackValues.Find(x => x.ComboID == ID);
        if (hackValue != null)
            value = hackValue.hackPointsModification;
        else
            Debug.LogError("NO HAY COMBO CON ESE ID");
        return value;
    }

    public float GetCooldown(string ID)
    {
        float value = 0;
        Cooldowns cooldown = cooldownTimes.Find(x => x.ComboID == ID);
        if (cooldown != null)
            value = cooldown.cooldownTime;
        else
            Debug.LogError("NO HAY COMBO CON ESE ID");
        return value;
    }
}
