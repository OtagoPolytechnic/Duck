using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class TerminalBehaviour : MonoBehaviour
{

    public List<Label> stdout = new List<Label>();
    public TextField input;
    private VisualElement document;
    private VisualElement terminalWindow;

    
    void Awake()
    {
        document = GetComponent<UIDocument>().rootVisualElement;
        terminalWindow = document.Q<VisualElement>("TerminalWindow");

    }

    void Update()
    {

    }

    public void ActivateWindow(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameSettings.gameState == GameState.Paused)
            {
                GameSettings.gameState = GameState.InGame;
            }
            else 
            {
                GameSettings.gameState = GameState.Paused;
                terminalWindow.visible = !terminalWindow.visible; //terminal should be hidden on game start`
            }
        }
    }
}
