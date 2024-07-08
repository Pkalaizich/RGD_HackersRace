using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private List<PlayerCombos> playerCombos = new List<PlayerCombos>();
    [SerializeField] private List<string> availableTags= new List<string>(); //TODO refactor this
    [SerializeField] private int playerIndex;
    [SerializeField] private PlayerController OpponentPlayer;
    [SerializeField] private CanvasGroup hackPanel;
    [SerializeField] private CanvasGroup attackPanel;

    [Header("UI elements")]
    [SerializeField] private Image progressFiller;
    [SerializeField] private List<CanvasGroup> hackedWindows;
    [SerializeField] private Image windowsCooldownFiller;
    [SerializeField] private Image interruptCooldownFiller;
    [SerializeField] private Image decreaseCooldownFiller;
    [SerializeField] private Image statusIcon;
    [SerializeField] private Sprite hackingSprite;
    [SerializeField] private Sprite attackingSprite;
    [SerializeField] private Image L1Icon;
    [SerializeField] private Image R1Icon;
    [SerializeField] private Sprite JoyL1sprite;
    [SerializeField] private Sprite JoyR1Sprite;
    [SerializeField] private Sprite KeyL1sprite;
    [SerializeField] private Sprite KeyR1Sprite;

    [SerializeField] List<TextMeshProUGUI> hacksNames;
    [SerializeField] List<TextMeshProUGUI> attackNames;

    #region private variables
    private Combo activeCombo = null;
    private Combo previousCombo = null;
    private int currentComboButtonIndex;
    private int previousComboButtonIndex;

    private bool isInHackingMode = true;
    private float pointsNeededToWin;
    private float currentPoints;

    private float lightHackPoints;
    private float mediumHackPoints;
    private float heavyHackPoints;
    private float decreaseHackPower;

    private bool gameIsRunning = false;

    private int currentWindow = 0;

    private bool windowsOpen = false;

    private GameController gameController;

    private float attack1Cooldown;
    private float attack2Cooldown;
    private float attack3Cooldown;

    private TestControl playerTestActions;
    #endregion

    //TODO refactor from here
    [SerializeField] private KeyCode NorthButton;
    [SerializeField] private KeyCode SouthButton;
    [SerializeField] private KeyCode WestButton;
    [SerializeField] private KeyCode EastButton;
    [SerializeField] private KeyCode HackWindowButton;
    [SerializeField] private KeyCode AttackWindowButton;
    //TO HERE


    [System.Serializable]
    public class PlayerCombos
    {
        public string ComboType;
        public List<Combo> AvailableCombos;
    }

    [System.Serializable]
    public class Combo
    {        
        public string ComboName;
        public string SettingID;
        public int ComboLength;
        public int ComboIndex;
        public List<ButtonIndicator> ButtonsCombo = new List<ButtonIndicator>();
        public StaticValues.AssignedButtons firstButton;
        public UnityEvent <int> OnComboCompleted;
        public GameObject parentTransform;

        public bool OnCooldown;
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

    private void Start()
    {
        playerTestActions = new TestControl();
        playerTestActions.Gameplay.Enable();

        for (int i =0; i<playerCombos.Count; i++)
        {
            for(int j=0; j < playerCombos[i].AvailableCombos.Count; j++)
            {                
                string id = playerCombos[i].AvailableCombos[j].SettingID;
                playerCombos[i].AvailableCombos[j].ComboLength = GameplaySettings.Instance.GetLength(id);
            }
        }

        pointsNeededToWin = GameplaySettings.Instance.REQUIRED_POINTS;

        lightHackPoints = GameplaySettings.Instance.GetHackPointsValues("Light Hack");
        mediumHackPoints = GameplaySettings.Instance.GetHackPointsValues("Medium Hack");
        heavyHackPoints = GameplaySettings.Instance.GetHackPointsValues("Heavy Hack");
        decreaseHackPower = GameplaySettings.Instance.GetHackPointsValues("Decrease Opponent");

        attack1Cooldown = GameplaySettings.Instance.GetCooldown("Attack 1");
        attack2Cooldown = GameplaySettings.Instance.GetCooldown("Attack 2");
        attack3Cooldown = GameplaySettings.Instance.GetCooldown("Attack 3");

        gameController = FindObjectOfType<GameController>();
    }

    public void ButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.action.name == playerTestActions.Gameplay.North.name)
            {
                CheckButton(StaticValues.AssignedButtons.North);
            }
            if (context.action.name == playerTestActions.Gameplay.South.name)
            {
                CheckButton(StaticValues.AssignedButtons.South);
            }
            if (context.action.name == playerTestActions.Gameplay.West.name)
            {
                CheckButton(StaticValues.AssignedButtons.West);
            }
            if (context.action.name == playerTestActions.Gameplay.East.name)
            {
                CheckButton(StaticValues.AssignedButtons.East);
            }
            if (context.action.name == playerTestActions.Gameplay.ShoulderLeft.name && !isInHackingMode)
            {
                ChangeMode(true);
            }
            if (context.action.name == playerTestActions.Gameplay.ShoulderRight.name && isInHackingMode)
            {
                ChangeMode(false);
            }
        }
    }

    public void ButtonP2Pressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.action.name == playerTestActions.Gameplay2Keyboard.NorthP2.name)
            {
                CheckButton(StaticValues.AssignedButtons.North);
            }
            if (context.action.name == playerTestActions.Gameplay2Keyboard.SouthP2.name)
            {
                CheckButton(StaticValues.AssignedButtons.South);
            }
            if (context.action.name == playerTestActions.Gameplay2Keyboard.WestP2.name)
            {
                CheckButton(StaticValues.AssignedButtons.West);
            }
            if (context.action.name == playerTestActions.Gameplay2Keyboard.EastP2.name)
            {
                CheckButton(StaticValues.AssignedButtons.East);
            }
            if (context.action.name == playerTestActions.Gameplay2Keyboard.ShoulderLeftP2.name && !isInHackingMode)
            {
                ChangeMode(true);
            }
            if (context.action.name == playerTestActions.Gameplay2Keyboard.ShoulderRightP2.name && isInHackingMode)
            {
                ChangeMode(false);
            }
        }
    }

    public void SetInitialValues()
    {
        # region close windows if open from last game
        windowsOpen = false;
        currentWindow = 0;
        currentComboButtonIndex = 0;
        for (int i = 0; i<hackedWindows.Count; i++)
        {
            hackedWindows[i].DOFade(0, 0).Play();
        }
        #endregion


        if (PlayersManager.Instance.INFO[playerIndex].playerBehaviour.PLAYER_INPUT.currentControlScheme == "Keyboard")
        {
            R1Icon.sprite = KeyR1Sprite;
            L1Icon.sprite = KeyL1sprite;
        }
        else
        {
            R1Icon.sprite = JoyR1Sprite;
            L1Icon.sprite = JoyL1sprite;
        }

        activeCombo = null;
        previousCombo= null;
        
        currentPoints = 0;
        ChangeMode(true);
        progressFiller.fillAmount= 0;
        gameIsRunning = true;
        RefreshAllCombos();        
    }

    public void GameEnded()
    {
        ChangeMode(true);
        gameIsRunning= false;
    }

    public void CheckButton(StaticValues.AssignedButtons buttonPressed)
    {
        if(gameIsRunning)
        {
            int audioIndex = Random.Range(5, 7);
            AudioManager.instance.PlaySound("KeySound" + audioIndex);
            if (activeCombo == null)
            {
                int comboTypeIndex = isInHackingMode ? 0 : 1;
                foreach (Combo combo in playerCombos[comboTypeIndex].AvailableCombos)
                {
                    if (buttonPressed == combo.ButtonsCombo[0].ASSIGNED && combo.ButtonsCombo[0].IsEnabled)
                    {
                        activeCombo = combo;
                        combo.ButtonsCombo[0].SetButtonStatus(true);
                        currentComboButtonIndex++;
                        break;
                    }
                }
                if (activeCombo == null)
                {
                    Debug.Log("BAD INPUT");
                }
            }
            else
            {
                ButtonIndicator nextButton = activeCombo.ButtonsCombo.First(x => !x.IsActive);
                if (nextButton != null)
                {
                    if (nextButton.ASSIGNED == buttonPressed)
                    {
                        activeCombo.ButtonsCombo[currentComboButtonIndex].SetButtonStatus(true);
                        currentComboButtonIndex++;
                        if (activeCombo.ComboCompleted())
                        {
                            AudioManager.instance.PlaySound("ComboCompleted");
                            currentComboButtonIndex = 0;
                            RefreshCombo(activeCombo);
                            activeCombo.OnComboCompleted?.Invoke(activeCombo.ComboIndex);
                            if (!windowsOpen && activeCombo != previousCombo)
                            {
                                activeCombo = null;
                                previousCombo = null;
                            }
                            else if (activeCombo == previousCombo)
                            {
                                previousCombo = null;
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
    }

    public void ChangeMode(bool hacking)
    {
        isInHackingMode = hacking;
        if(hacking)
        {
            hackPanel.DOFade(1,0);
            attackPanel.DOFade(0, 0);
            statusIcon.sprite = hackingSprite;
        }
        else
        {
            hackPanel.DOFade(0, 0);
            attackPanel.DOFade(1, 0);
            statusIcon.sprite = attackingSprite;
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
            gameController.OnPlayerWon?.Invoke(playerIndex);            
        }
        progressFiller.DOFillAmount(progress,0.2f).Play();
    }


    #region CombosResults

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
        StartCoroutine(ErrorAudios());
        Sequence windowsSequence = DOTween.Sequence();
        windowsSequence.Append(hackedWindows[2].DOFade(1, 0.1f))
            .Append(hackedWindows[1].DOFade(1, 0.1f))
            .Append(hackedWindows[0].DOFade(1, 0.1f)).Play();
    }

    private IEnumerator ErrorAudios()
    {
        for(int i =0;i<3;i++)
        {
            yield return new WaitForSeconds(0.1f);
            AudioManager.instance.PlaySound("Error");
        }        
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
            for(int i =0; i < playerCombos[1].AvailableCombos[0].ButtonsCombo.Count; i++)
            {
                playerCombos[1].AvailableCombos[0].ButtonsCombo[i].SetButonEnabled(false, attack1Cooldown);
            }            
            windowsCooldownFiller.DOFillAmount(1, attack1Cooldown).SetEase(Ease.Linear).Play().OnComplete(() =>
            {
                windowsCooldownFiller.fillAmount= 0;                
            });
        }
        if (attackUsed == 1)
        {
            OpponentPlayer.DeleteCurrentCombo();
            for (int i = 0; i < playerCombos[1].AvailableCombos[1].ButtonsCombo.Count; i++)
            {
                playerCombos[1].AvailableCombos[1].ButtonsCombo[i].SetButonEnabled(false, attack2Cooldown);
            }
            interruptCooldownFiller.DOFillAmount(1, attack2Cooldown).SetEase(Ease.Linear).Play().OnComplete(() =>
            {
                interruptCooldownFiller.fillAmount = 0;
            });
        }
        if (attackUsed == 2)
        {
            OpponentPlayer.ModifyHackProgress(decreaseHackPower);
            for (int i = 0; i < playerCombos[1].AvailableCombos[2].ButtonsCombo.Count; i++)
            {
                playerCombos[1].AvailableCombos[2].ButtonsCombo[i].SetButonEnabled(false, attack3Cooldown);
            }
            decreaseCooldownFiller.DOFillAmount(1, attack3Cooldown).SetEase(Ease.Linear).Play().OnComplete(() =>
            {
                decreaseCooldownFiller.fillAmount = 0;
            });
        }

        Color original = attackNames[attackUsed].color;
        Sequence colorSequence = DOTween.Sequence();
        colorSequence.Append(attackNames[attackUsed].DOColor(Color.white, 0.2f))
            .Append(attackNames[attackUsed].DOColor(Color.black, 0.2f))
            .Append(attackNames[attackUsed].DOColor(original, 0.2f)).SetLoops(2).Play();
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

        Color original = hacksNames[hackUsed].color;
        Sequence colorSequence = DOTween.Sequence();
        colorSequence.Append(hacksNames[hackUsed].DOColor(Color.white, 0.2f))
            .Append(hacksNames[hackUsed].DOColor(Color.black,0.2f))
            .Append(hacksNames[hackUsed].DOColor(original, 0.2f)).SetLoops(2).Play();

    }
    #endregion

    public int GetPlayerIndex()
    {
        return playerIndex;
    }
}
