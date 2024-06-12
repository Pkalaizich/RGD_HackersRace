using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private List<PlayerCombos> playerCombos = new List<PlayerCombos>();
    [SerializeField] private List<string> availableTags= new List<string>(); //TODO refactor this
    private Combo activeCombo = null;
    private Combo previousCombo = null;
    private int currentComboButtonIndex;
    private int previousComboButtonIndex;

    private bool isInHackingMode =true;

    //TODO refactor from here
    [SerializeField] private KeyCode NorthButton;
    [SerializeField] private KeyCode SouthButton;
    [SerializeField] private KeyCode WestButton;
    [SerializeField] private KeyCode EastButton;

    [SerializeField] private KeyCode HackWindowButton;
    [SerializeField] private KeyCode AttackWindowButton;

    public bool gameIsRunning = false;
    public int playerIndex;

    public CanvasGroup hackPanel;
    public CanvasGroup attackPanel;

    public PlayerController OpponentPlayer;

    public float pointsNeededToWin;
    public float currentPoints;
    public float lightHackPoints;
    public float mediumHackPoints;
    public float heavyHackPoints;

    public float windowsRefreshTime;
    private float windowsTimer;
    public Image windowsCooldownFiller;
    public float interruptRefreshTime;
    private float interruptTimer;
    public Image interruptCooldownFiller;
    public float decreaseHackTime;
    private float decreaseTimer;
    public Image decreaseCooldownFiller;

    public float decreaseHackPower;
    [Header("UI elements")]
    public Image progressFiller;
    public List<CanvasGroup> hackedWindows;
    private int currentWindow = 0;
    private bool windowsOpen = false;
    //TO HERE


    [System.Serializable]
    public class PlayerCombos
    {
        public string ComboType;
        //public bool IsActiveCombo = false;        
        public List<Combo> AvailableCombos;
    }

    [System.Serializable]
    public class Combo
    {        
        public string ComboName;
        public int ComboLength;
        public int ComboIndex;
        public List<ButtonIndicator> ButtonsCombo = new List<ButtonIndicator>();
        public StaticValues.AssignedButtons firstButton;
        public UnityEvent <int> OnComboCompleted;
        public GameObject parentTransform;

        public bool ComboCompleted()
        {
            foreach(ButtonIndicator button in ButtonsCombo)
            {
                if(!button.IsActive)
                {
                    return false;
                }
            }
            return true;
        }
    }

    private void Update()
    {
        if(gameIsRunning)
        {
            if(Input.GetKeyDown(NorthButton))
            {
                CheckButton(StaticValues.AssignedButtons.North);
            }
            if (Input.GetKeyDown(SouthButton))
            {
                CheckButton(StaticValues.AssignedButtons.South);
            }
            if (Input.GetKeyDown(WestButton))
            {
                CheckButton(StaticValues.AssignedButtons.West);
            }
            if (Input.GetKeyDown(EastButton))
            {
                CheckButton(StaticValues.AssignedButtons.East);
            }
            if (Input.GetKeyDown(HackWindowButton)&& !isInHackingMode)
            {
                ChangeMode(true);
                return;
            }
            if (Input.GetKeyDown(AttackWindowButton) && isInHackingMode)
            {
                ChangeMode(false);
                return;
            }
        }
    }

    public void CheckButton(StaticValues.AssignedButtons buttonPressed)
    {
        if (activeCombo == null)
        {
            int comboTypeIndex = isInHackingMode ? 0 : 1;
            foreach(Combo combo in playerCombos[comboTypeIndex].AvailableCombos)
            {
                if(buttonPressed == combo.ButtonsCombo[0].ASSIGNED)
                {
                    activeCombo = combo;
                    combo.ButtonsCombo[0].SetButtonStatus(true);
                    currentComboButtonIndex++;
                    break;
                }
            }
            if(activeCombo == null)
            {
                Debug.Log("BAD INPUT");
            }
        }
        else
        {
            ButtonIndicator nextButton = activeCombo.ButtonsCombo.First(x => !x.IsActive);
            if(nextButton != null)
            {
                if(nextButton.ASSIGNED == buttonPressed)
                {
                    activeCombo.ButtonsCombo[currentComboButtonIndex].SetButtonStatus(true);
                    currentComboButtonIndex++;
                    if(activeCombo.ComboCompleted())
                    {
                        currentComboButtonIndex = 0;
                        RefreshCombo(activeCombo);
                        activeCombo.OnComboCompleted?.Invoke(activeCombo.ComboIndex);
                        if (!windowsOpen && activeCombo != previousCombo)
                        {
                            activeCombo = null;
                            previousCombo = null;
                        }
                        else if(activeCombo == previousCombo)
                        {
                            previousCombo= null;
                        }
                    }
                }
                else
                {
                    DeleteCurrentCombo();
                    Debug.LogWarning("BAD INPUT");
                }
            }
            else
            {
                Debug.LogError("Unexpected, something went wrong");
            }
        }
    }

    public void ChangeMode(bool hacking)
    {
        isInHackingMode = hacking;
        if(hacking)
        {
            hackPanel.DOFade(1,0);
            attackPanel.DOFade(0, 0);
        }
        else
        {
            hackPanel.DOFade(0, 0);
            attackPanel.DOFade(1, 0);
        }        
        DeleteCurrentCombo();
    }

    public void DeleteCurrentCombo()
    {
        if (activeCombo != null)
        {
            foreach (ButtonIndicator button in activeCombo.ButtonsCombo) 
            {
                
                button.SetButtonStatus(false);
            }            
        }
        if(!windowsOpen)
        {
            activeCombo = null;
            previousCombo= null;
        }        
        currentComboButtonIndex = 0;
    }

    public void RefreshAllCombos()
    {
        windowsOpen = false;
        foreach(PlayerCombos playerCombos in playerCombos)
        {
            foreach(Combo combo in playerCombos.AvailableCombos)
            {
                RefreshCombo(combo);
            }
        }
    }

    public void RefreshCombo(Combo toRefresh)
    {
        if(gameIsRunning)
        {
            foreach (ButtonIndicator indicator in toRefresh.ButtonsCombo)
            {
                ObjectPooler.Instance.Enqueue(indicator.POOL_TAG, indicator.gameObject);
                indicator.gameObject.SetActive(false);
            }
            toRefresh.ButtonsCombo.Clear();
            for (int i = 0; i < toRefresh.ComboLength; i++)
            {
                GameObject toDequeue;
                if (i == 0)
                {
                    bool useFirst = (int)toRefresh.firstButton == 10 ? false:true;
                    if(useFirst)
                        toDequeue = ObjectPooler.Instance.SpawnFromPool(transform.position, Quaternion.identity, availableTags[(int)toRefresh.firstButton]);
                    else
                        toDequeue = ObjectPooler.Instance.SpawnRandomObjectFromPool(transform.position, Quaternion.identity);
                }
                else
                {
                    //int toSpawn = Random.Range(0, 4);
                    toDequeue = ObjectPooler.Instance.SpawnRandomObjectFromPool(transform.position, Quaternion.identity);
                }
                toDequeue.transform.SetParent(toRefresh.parentTransform.transform, false);
                toRefresh.ButtonsCombo.Add(toDequeue.GetComponent<ButtonIndicator>());
            }
        }        
    }

    public void ModifyHackProgress(float modifyValue)
    {
        currentPoints += modifyValue;
        currentPoints = currentPoints <= 0 ? 0 : currentPoints;
        currentPoints = currentPoints >= pointsNeededToWin? pointsNeededToWin : currentPoints;

        float progress = currentPoints / pointsNeededToWin;

        if(currentPoints>=pointsNeededToWin)
        {
            FindObjectOfType<GameController>().GameOver(playerIndex); //TODO REFACTOR THIS
        }

        progressFiller.DOFillAmount(progress,0.2f).Play();
    }

    //TODO refactor, no es necesario escribir los ataques por separado

    public void OpenWindows()
    {
        previousCombo = activeCombo;
        previousComboButtonIndex = currentComboButtonIndex;
        currentComboButtonIndex = 0;
        currentWindow = 0;
        windowsOpen = true;
        foreach (Combo combo in playerCombos[2].AvailableCombos)
        {
            RefreshCombo(combo);
        }
        activeCombo = playerCombos[2].AvailableCombos[0];
        Sequence windowsSequence = DOTween.Sequence();
        windowsSequence.Append(hackedWindows[2].DOFade(1, 0.1f))
            .Append(hackedWindows[1].DOFade(1, 0.1f))
            .Append(hackedWindows[0].DOFade(1, 0.1f)).Play();
    }

    public void CloseWindow()
    {
        hackedWindows[2-currentWindow].DOFade(0,0).Play();
        currentWindow++;        
        if (currentWindow>2)
        {
            windowsOpen = false;
            activeCombo = previousCombo;
            currentComboButtonIndex = previousComboButtonIndex;
            return;
        }
        activeCombo = playerCombos[2].AvailableCombos[currentWindow];
    }

    public void AttackPlayer(int attackUsed)
    {
        if(attackUsed==0)
        {
            OpponentPlayer.OpenWindows();
        }
        if (attackUsed == 1)
        {
            OpponentPlayer.DeleteCurrentCombo();
        }
        if (attackUsed == 2)
        {
            OpponentPlayer.ModifyHackProgress(-decreaseHackPower);
        }
    }

    public void HackPerformed(int hackUsed)
    {
        if(hackUsed ==0)
        {
            ModifyHackProgress(lightHackPoints);
        }
        if (hackUsed == 1)
        {
            ModifyHackProgress(mediumHackPoints);
        }
        if (hackUsed == 2)
        {
            ModifyHackProgress(heavyHackPoints);
        }
    }
    //UNTIL HERE

    public enum AttackTypes
    {
        OpenWindows,
        InterruptCombo,
        ReduceHack
    }
}
