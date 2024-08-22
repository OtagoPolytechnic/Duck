using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreInputField : MonoBehaviour
{

    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text validationText;
    public string playerName;

    public void ValidateInput()
    {
        playerName = inputField.text;
        if (playerName.Length < 1)
        {
            validationText.text = "Please enter a name";
            validationText.color = Color.red;
        }
        else if (playerName.Length > 7)
        {
            validationText.text = "Name too long";
            validationText.color = Color.red;
        }
        else 
        {
            validationText.text = "Valid name!";
            validationText.color = Color.green;
        }
        //currently allows invalid names
        Debug.Log(playerName);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
