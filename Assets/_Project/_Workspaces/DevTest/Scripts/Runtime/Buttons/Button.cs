using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField] private Material onHoverMaterial;
    [SerializeField] private UnityEvent triggeredEvents;
    
    private MeshRenderer _meshRenderer;
    private Material _originalMaterial;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _originalMaterial = _meshRenderer.material;
    }

    public void OnHover()
    {
        _meshRenderer.material = onHoverMaterial;
        
    }

    public void Interact()
    {
        triggeredEvents.Invoke();
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/GeneralEnvironment/ButtonPushed", transform.position);
    }

    public void OnHoverExit()
    {
        _meshRenderer.material = _originalMaterial;
    }
}
