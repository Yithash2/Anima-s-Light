using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class InputsManager : MonoBehaviour
{
    public static InputsManager Instance { get; private set; }
    public PlayersActions Actions { get; private set; }

    public Action OnActionActive;
    public Action OnActionDeactive;

    public InputAction Jump { get; private set; }
    public InputAction Dive { get; private set; }
    public InputAction XInput { get; private set; }

    public InputAction Echap { get; private set; }

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Actions = new PlayersActions();

        Jump = Actions.Player.Jump;
        Dive = Actions.Player.Dive;
        XInput = Actions.Player.Move;
        Echap = Actions.Player.Echap;
    }

    private void OnEnable()
    {
        
        ActivateActions();
    }

    private void OnDisable()
    {
        DeactivateActions();
    }

    private void OnValidate()
    {
        GetComponent<PlayerInput>().uiInputModule = FindObjectOfType<InputSystemUIInputModule>();
    }

    public void ActivateActions()
    {
        Jump.Enable();
        Dive.Enable();
        XInput.Enable();
        Echap.Enable();
        OnActionActive?.Invoke();
    }

    public void DeactivateActions()
    {
        Jump.Disable();
        Dive.Disable();
        XInput.Disable();
        Echap.Disable();
        OnActionDeactive?.Invoke();
    }
}
