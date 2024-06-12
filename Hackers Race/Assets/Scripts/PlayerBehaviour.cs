using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    private PlayerInput playerInput;
    private TestScript testScript;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var testers = FindObjectsOfType<TestScript>();
        var index = playerInput.playerIndex;
        testScript = testers.FirstOrDefault(x => x.GetPlayerIndex() == index);

        
    }

    public void OnButtonPressed(InputAction.CallbackContext context)
    {
        testScript.ButtonPressed(context);
    }
}
