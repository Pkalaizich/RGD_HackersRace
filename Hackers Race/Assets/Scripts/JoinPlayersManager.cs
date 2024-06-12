using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoinPlayersManager : MonoBehaviour
{
    private PlayerInputManager inputManager;

    private void Awake()
    {
        inputManager = new PlayerInputManager();        
    }


}
