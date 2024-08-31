using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TutorialText : MonoBehaviour
{
    public TextMeshPro TextField;
    public string message;

    [SerializeField]
        private InputActionReference m_Action;

    [SerializeField]
        private PlayerInput _playerInput;
    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    void UpdateText(){
        int device = 0;

        if(_playerInput.currentControlScheme == "Gamepad"){
            device = 1;
        }else{
            device = 0;
        }

        var displayString = InputControlPath.ToHumanReadableString(m_Action.action.bindings[device].effectivePath);

        if(device == 0){
            displayString = displayString.Replace(" [Keyboard]", "") + " Button";
        }else{
            displayString = displayString.Replace(" [Gamepad]","");

            if(displayString.Contains("Button")){
            string[] str =  displayString.Split(' ');

            displayString = str[1] + " " + str[0];
            }
        }


        TextField.text = message.Replace("#VALUE#", displayString);
    }

}
