using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeDilationField : MonoBehaviour
{
    [Header("Gravity Field")]
    [SerializeField] private List<Gravity> affectedObjects;
    [SerializeField] private float staticDistance;
    [SerializeField] private float maxSize = 30.0f; //Max Radius

    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material inactiveMaterial;

    private MeshRenderer _meshRenderer;
    private bool _active = true;

    public void Start()
    {
        staticDistance = 0.5f;
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Update()
    {
        if (_active)
        {
            float gravityFieldRadius = transform.localScale.x * 0.5f;
            foreach (Gravity gravity in affectedObjects)
            {
                float dist = Mathf.Clamp(Vector3.Distance(gravity.gameObject.transform.position, transform.position) - staticDistance * gravityFieldRadius, 0.0f, gravityFieldRadius * (1.0f - staticDistance));
                gravity.ChangeTimeSpeed(dist / (gravityFieldRadius * (1.0f - staticDistance)));
            }
        }
    }

    public void ResizeTimeDilationField(InputAction.CallbackContext context)
    {
        transform.localScale = Vector3.one * Mathf.Clamp(transform.localScale.x + context.ReadValue<Vector2>().y * Time.deltaTime, 0.0f, maxSize);
    }

    public void ToggleTimeDilationField(InputAction.CallbackContext context)
    {
        //Switch the texture

        _active = !_active;
        if (!_active)
        {
            _meshRenderer.material = inactiveMaterial;
            foreach (Gravity gravity in affectedObjects)
            {
                gravity.ChangeTimeSpeed(1.0f);
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
            affectedObjects.Add(gravity);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Gravity gravity = other.GetComponent<Gravity>();
        if (gravity != null)
        {
            gravity.ChangeTimeSpeed(1.0f); //reset the time flow of object to normal
            affectedObjects.Remove(gravity);
        }
    }
}
