using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuState : State
{
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject inputManager;

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject creditsPanel;
    private void Start()
    {
        playButton.onClick.AddListener(()=>
        {
            stateController.NextState();
            playButton.interactable = false;
        });
    }
    public override void OnEnterState()
    {
        inputManager.SetActive(false);
        playButton.interactable = true;
        creditsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public override void OnExitState()
    {
        inputManager.SetActive(true);
    }

    public override void OnUpdateState()
    {
        if(Input.GetKeyUp(KeyCode.C))
        {
            creditsPanel.SetActive(true);
            menuPanel.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            creditsPanel.SetActive(false);
            menuPanel.SetActive(true);
        }
    }
}
