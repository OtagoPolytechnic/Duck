using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.InputSystem;

public static class SubmitCheck
{
    public static bool Submit(EventBase evt, InputActionAsset inputActions)
    {
        if (evt is KeyDownEvent keyEvent)
        {
            InputAction submitAction = inputActions.FindAction("UI/Submit");
            //Check if the key pressed matches the binding for UI/Submit
            if (submitAction.controls.Any(control => control.IsPressed()))
            {
                return true;
            }
            return false;
        }
        return true;
    }
}
