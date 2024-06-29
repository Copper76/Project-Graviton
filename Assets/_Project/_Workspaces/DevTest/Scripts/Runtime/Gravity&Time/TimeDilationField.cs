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

    public void Start()
    {
        staticDistance = 0.5f;
    }

    public void Update()
    {
        float gravityFieldRadius = transform.localScale.x * 0.5f;
        foreach (Gravity gravity in affectedObjects)
        {
            float dist = Mathf.Clamp(Vector3.Distance(gravity.gameObject.transform.position, transform.position) - staticDistance * gravityFieldRadius, 0.0f, gravityFieldRadius * (1.0f - staticDistance));
            gravity.ChangeTimeSpeed(dist / (gravityFieldRadius * (1.0f - staticDistance)));
        }
    }

    public void ResizeGravityField(InputAction.CallbackContext context)
    {
        transform.localScale = Vector3.one * Mathf.Clamp(transform.localScale.x + context.ReadValue<Vector2>().y * Time.deltaTime, 0.0f, maxSize);
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
