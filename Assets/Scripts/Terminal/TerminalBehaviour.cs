using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TerminalBehaviour : MonoBehaviour
{

    public List<Label> stdout = new List<Label>();
    public TextField input;
    private VisualElement document;
    private VisualElement terminalWindow;
    
    void Awake()
    {
        document = GetComponent<UIDocument>().rootVisualElement;
    }
}
