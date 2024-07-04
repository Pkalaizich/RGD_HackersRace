using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    private TestControl playerTestActions;    

    [SerializeField] private List<Image> buttonsImages;
    [SerializeField] private int playerIndex;

    private void Awake()
    {        
        playerTestActions = new TestControl();
        playerTestActions.Gameplay.Enable();
    }

    public void SetDevice(InputDevice device)
    {
        
    }

    public void ButtonPressed(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(context.action.activeControl == playerTestActions.Gameplay.North.activeControl)
            {
                LightButton(AssignedButtons.North);
            }
            if (context.action.activeControl == playerTestActions.Gameplay.South.activeControl)
            {
                LightButton(AssignedButtons.South);
            }
            if (context.action.activeControl == playerTestActions.Gameplay.West.activeControl)
            {
                LightButton(AssignedButtons.West);
            }
            if (context.action.activeControl == playerTestActions.Gameplay.East.activeControl)
            {
                LightButton(AssignedButtons.East);
            }
        }
    }

    public void LightButton(AssignedButtons pressed)
    {
        switch(pressed)
        {
            case AssignedButtons.North:
                StartCoroutine(ChangeColor(buttonsImages[0]));
                break;
            case AssignedButtons.South:
                StartCoroutine(ChangeColor(buttonsImages[1]));
                break;            
            case AssignedButtons.West:
                StartCoroutine(ChangeColor(buttonsImages[2]));
                break;
            case AssignedButtons.East:
                StartCoroutine(ChangeColor(buttonsImages[3]));
                break;
        }
    }
    
    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    private IEnumerator ChangeColor(Image image)
    {
        image.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        image.color = Color.white;
    }

    public enum AssignedButtons
    {
        North,
        South,
        West,
        East
    }
}
