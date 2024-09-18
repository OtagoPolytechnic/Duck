using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KeybindsDisplay : MonoBehaviour
{
    private RadioButtonGroup radioButtonGroup;

    //text for the movement, aiming, shooting, and special ability keybinds
    private Label movementKey;
    private Label aimingKey;
    private Label shootingKey;
    private Label skillKey;

    // Start is called before the first frame update
    void Start()
    {
        VisualElement document = GetComponent<UIDocument>().rootVisualElement;
        radioButtonGroup = document.Q<RadioButtonGroup>("RadioButtonGroup");
        movementKey = document.Q<Label>("MovementKeys");
        aimingKey = document.Q<Label>("AimingKeys");
        shootingKey = document.Q<Label>("ShootingKeys");
        skillKey = document.Q<Label>("SkillKeys");
        radioButtonGroup.RegisterCallback<ChangeEvent<int>>(ControlTypeChanged);

        if (GameSettings.controlType == controlType.Keyboard)
        {
            radioButtonGroup.value = 0;
        }
        else if (GameSettings.controlType == controlType.Controller)
        {
            radioButtonGroup.value = 1;
        }
        else if (GameSettings.controlType == controlType.Arcade)
        {
            radioButtonGroup.value = 2;
        }

    }

    public void ControlTypeChanged(ChangeEvent<int> evt)
    {
        if (evt.newValue == 0)
        {
            GameSettings.controlType = controlType.Keyboard;
            displayKeyboardControls();
        }
        else if (evt.newValue == 1)
        {
            GameSettings.controlType = controlType.Controller;
            displayControllerControls();
        }
        else if (evt.newValue == 2)
        {
            GameSettings.controlType = controlType.Arcade;
            displayArcadeControls();
        }
    }

    private void displayKeyboardControls()
    {
        //Display keyboard controls
        movementKey.text = "W\nA S D";
        aimingKey.text = "Mouse";
        shootingKey.text = "Left Click";
        skillKey.text = "Shift";
    }

    private void displayControllerControls()
    {
        //Display controller controls
        movementKey.text = "Left Stick";
        aimingKey.text = "Right Stick";
        shootingKey.text = "Right Trigger";
        skillKey.text = "South Button"; //This sounds weird. How should this be phrased
    }

    private void displayArcadeControls()
    {
        //Display arcade controls
        movementKey.text = "Joystick";
        aimingKey.text = "Move to aim";
        shootingKey.text = "Bottom left button";
        skillKey.text = "Top left button";
    }
}
