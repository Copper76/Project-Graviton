using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedButton : MonoBehaviour, IInteractable
{
    [SerializeField] private Material onHoverMaterial;
    [SerializeField] private UnityEvent triggeredEvents;

    private MeshRenderer _meshRenderer;
    private Material _originalMaterial;

    [SerializeField] private float resetTime = 1.0f;
    private float _resetTimer;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _originalMaterial = _meshRenderer.material;
    }

    private void Update()
    {
        if (_resetTimer > 0)
        {
            _resetTimer -= Time.deltaTime;
        }
        else
        {
            _resetTimer = 0;
        }
    }

    public void OnHover()
    {
        _meshRenderer.material = onHoverMaterial;

    }

    public void Interact()
    {
        if (_resetTimer  <= 0)
        {
            triggeredEvents.Invoke();
            //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/ButtonsAndPressurePlates/ButtonPushed", GetComponent<Transform>().position);
            _resetTimer = resetTime;

            StartCoroutine(CounterInteract());
        }
    }

    private IEnumerator CounterInteract()
    {
        yield return new WaitForSeconds(resetTime);

        triggeredEvents.Invoke();
        //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/ButtonsAndPressurePlates/ButtonPushed", GetComponent<Transform>().position);
    }

    public void OnHoverExit()
    {
        _meshRenderer.material = _originalMaterial;
    }
}
