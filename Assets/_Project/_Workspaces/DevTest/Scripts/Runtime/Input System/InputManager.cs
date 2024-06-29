using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    
    [HideInInspector] public PlayerInputActions playerInputActions;
    private TimeDilationField _timeDilationField;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        _timeDilationField = FindObjectOfType<TimeDilationField>();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Look.Enable();
        playerInputActions.Player.Move.Enable();
        playerInputActions.Player.Jump.Enable();
        playerInputActions.Player.Fire.Enable();
        playerInputActions.Player.DilateTime.Enable();
        playerInputActions.Player.DilateTime.performed += _timeDilationField.ResizeGravityField;
    }

    private void OnDisable()
    {
        playerInputActions.Player.Look.Disable();
        playerInputActions.Player.Move.Disable();
        playerInputActions.Player.Jump.Disable();
        playerInputActions.Player.Fire.Disable();
        playerInputActions.Player.DilateTime.Disable();
    }


}
