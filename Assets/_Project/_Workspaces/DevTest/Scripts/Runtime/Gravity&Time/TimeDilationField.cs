using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeDilationField : MonoBehaviour
{
    [Header("Gravity Field")]
    [SerializeField] private List<Gravity> _affectedObjects;
    [SerializeField] private float maxSize = 30.0f; //Max Radius
    [SerializeField] private float minSize = 3.0f; //Min Radius
    [SerializeField] private AnimationCurve timeDilationFieldStrength;

    [Header("Time Dilation Field Materials")]
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material inactiveMaterial;

    private MeshRenderer _meshRenderer;
    private bool _active = true;

    public void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Update()
    {
        UpdateTimeDilation();
    }

    private void UpdateTimeDilation()
    {
        if (!_active) return;

        float gravityFieldRadius = transform.localScale.x * 0.5f;
        if (gravityFieldRadius <= 0) return;

        foreach (Gravity gravity in _affectedObjects)
        {
            float dist = Mathf.Clamp(Vector3.Distance(gravity.gameObject.transform.position, transform.position) / gravityFieldRadius, 0.0f, 1.0f);
            gravity.SetTimeSpeed(timeDilationFieldStrength.Evaluate(dist));
        }
    }

    public void ResizeTimeDilationField(InputAction.CallbackContext context)
    {
        transform.localScale = Vector3.one * Mathf.Clamp(transform.localScale.x + context.ReadValue<Vector2>().y * Time.deltaTime, minSize, maxSize);
    }

    public void ToggleTimeDilationField(InputAction.CallbackContext context)
    {
        _active = !_active;
        if (!_active)
        {
            _meshRenderer.material = inactiveMaterial;
            foreach (Gravity gravity in _affectedObjects)
            {
                gravity.SetTimeSpeed(1.0f);
            }
        }
        else
        {
            _meshRenderer.material = activeMaterial;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Gravity gravity = other.GetComponent<Gravity>();
        if (gravity != null)
        {
            _affectedObjects.Add(gravity);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Gravity gravity = other.GetComponent<Gravity>();
        if (gravity != null)
        {
            gravity.SetTimeSpeed(1.0f); //reset the time flow of object to normal
            _affectedObjects.Remove(gravity);
        }
    }
}