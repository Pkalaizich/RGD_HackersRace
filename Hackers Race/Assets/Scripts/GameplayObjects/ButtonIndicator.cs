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
    public string POOL_TAG=>poolTag;

    [Header("Icons for different controllers")]
    [SerializeField] private Sprite xboxIcon;
    [SerializeField] private Sprite psIcon;
    [SerializeField] private Sprite switchIcon;
    [SerializeField] private Sprite keyboardIcon;
    [SerializeField] private Sprite numpadIcon;

    private bool firstDisable = true;
    public bool IsActive { get; private set; }

    private void OnEnable()
    {
        //IsActive = false;
        SetButtonStatus(false);
    }

    private void OnDisable()
    {
        //IsActive = false;
        //if(firstDisable)
        //{
        //    firstDisable= false;
        //}
        //else
        //{
        //    SetButtonStatus(false);
        //    ObjectPooler.Instance.Enqueue(poolTag, this.gameObject);
        //}        
    }

    public void SetButtonStatus(bool status)
    {
        IsActive = status;
        Color newColor = status? Color.green : Color.white;
        ButtonBack.color= newColor;
    }
}
