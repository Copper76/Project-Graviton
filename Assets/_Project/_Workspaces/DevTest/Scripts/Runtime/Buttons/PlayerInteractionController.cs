using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private float interactionRange = 5f;
    [SerializeField] private float interactionRadius = 0.5f;

    private InputManager _inputManager;
    private Camera _mainCamera;
    private IInteractable _currentInteractable;

    void Start()
    {
        _inputManager = FindObjectOfType<InputManager>();
        _mainCamera = Camera.main;
    }

    void Update()
    {
        CheckInteractable();
    }

    private void CheckInteractable()
    {
        RaycastHit hit;
        Vector3 origin = _mainCamera.transform.position;
        Vector3 direction = _mainCamera.transform.forward;

        bool hitSomething = Physics.SphereCast(origin, interactionRadius, direction, out hit, interactionRange);
        
        IInteractable interactable = hitSomething ? hit.transform.gameObject.GetComponent<IInteractable>() : null;
        
        if (interactable != _currentInteractable)
        {
            _currentInteractable?.OnHoverExit();
            _currentInteractable = interactable;
            _currentInteractable?.OnHover();
        }
        
        if (_currentInteractable != null && _inputManager.playerInputActions.Player.Interact.triggered)
        {
            _currentInteractable.Interact();
        }
    }
}