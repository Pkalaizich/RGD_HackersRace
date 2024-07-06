using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonIndicator : MonoBehaviour
{
    [SerializeField] private StaticValues.AssignedButtons assignedButton;
    public StaticValues.AssignedButtons ASSIGNED => assignedButton;
    [SerializeField] private Image buttonIcon;
    [SerializeField] private Image ButtonBack;
    [SerializeField] private string poolTag;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;
    [SerializeField] private Color disabledColor;

    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite disabledSprite;
    public string POOL_TAG=>poolTag;

    [Header("Icons for different controllers")]
    [SerializeField] private Sprite xboxIcon;
    [SerializeField] private Sprite psIcon;
    [SerializeField] private Sprite switchIcon;
    [SerializeField] private Sprite keyboardIcon;
    [SerializeField] private Sprite numpadIcon;
    
    public bool IsActive { get; private set; }
    public bool IsEnabled { get; private set; }

    private void OnEnable()
    {
        //IsActive = false;
        SetButtonStatus(false);
        SetButonEnabled(true, 0);
    }

    private void OnDisable()
    {
        StopAllCoroutines();        
    }

    public void SetButtonStatus(bool status)
    {
        IsActive = status;
        //Color newColor = status? activeColor : inactiveColor;
        //ButtonBack.color= newColor;

        ButtonBack.sprite = status ? activeSprite : inactiveSprite;
    }

    public void SetButonEnabled(bool enabled, float cooldownTime)
    {
        IsEnabled= enabled;
        //Color newColor = enabled ? inactiveColor : disabledColor;
        //ButtonBack.color = newColor;

        ButtonBack.sprite = enabled ? inactiveSprite : disabledSprite;
        if (!enabled)
        {
            StartCoroutine(ReactivateButton(cooldownTime));
        }
    }

    private IEnumerator ReactivateButton(float timeToReactivate)
    {
        yield return new WaitForSeconds(timeToReactivate);
        SetButonEnabled(true, 0);
    }
}
