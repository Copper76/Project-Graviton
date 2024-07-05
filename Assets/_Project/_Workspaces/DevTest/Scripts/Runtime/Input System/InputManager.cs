using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    
    [HideInInspector] public PlayerInputActions playerInputActions;
    private GravityGun _gravityGun;
    private TimeDilationField _timeDilationField;
    private PlayerInteractionController _interactionController;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        _gravityGun = FindObjectOfType<GravityGun>();
        _timeDilationField = FindObjectOfType<TimeDilationField>();
        _interactionController = FindAnyObjectByType<PlayerInteractionController>();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Look.Enable();
        playerInputActions.Player.Interact.Enable();
        playerInputActions.Player.Move.Enable();
        playerInputActions.Player.Jump.Enable();
        playerInputActions.Player.Fire.Enable();
        playerInputActions.Player.AltFire.Enable();
        playerInputActions.Player.DilateTime.Enable();
        playerInputActions.Player.ToggleTimeField.Enable();

        if (_interactionController != null)
        {
            playerInputActions.Player.Interact.performed += _interactionController.Interact;
        }

        if (_gravityGun != null)
        {
            playerInputActions.Player.Fire.performed += _gravityGun.Fire;
            playerInputActions.Player.AltFire.performed += _gravityGun.AltFire;
        }

        if (_timeDilationField != null)
        {
            playerInputActions.Player.DilateTime.performed += _timeDilationField.ResizeTimeDilationField;
            playerInputActions.Player.ToggleTimeField.performed += _timeDilationField.ToggleTimeDilationField;
        }
    }

    private void OnDisable()
    {
        if (_interactionController != null)
        {
            playerInputActions.Player.Interact.performed -= _interactionController.Interact;
        }

        if (_gravityGun != null)
        {
            playerInputActions.Player.Fire.performed -= _gravityGun.Fire;
            playerInputActions.Player.AltFire.performed -= _gravityGun.AltFire;
        }

        if (_timeDilationField != null)
        {
            playerInputActions.Player.DilateTime.performed -= _timeDilationField.ResizeTimeDilationField;
            playerInputActions.Player.ToggleTimeField.performed -= _timeDilationField.ToggleTimeDilationField;
        }

        playerInputActions.Player.Look.Disable();
        playerInputActions.Player.Interact.Disable();
        playerInputActions.Player.Move.Disable();
        playerInputActions.Player.Jump.Disable();
        playerInputActions.Player.Fire.Disable();
        playerInputActions.Player.AltFire.Disable();
        playerInputActions.Player.DilateTime.Disable();
        playerInputActions.Player.ToggleTimeField.Disable();
    }

}
