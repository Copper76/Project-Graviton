using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class TimeDilationField : MonoBehaviour
{
    [Header("Time Dilation Field")]
    [SerializeField] private List<RelativeTime> _affectedObjects;
    [SerializeField] private float maxSize = 30.0f; //Max Radius
    [SerializeField] private float minSize = 3.0f; //Min Radius
    [SerializeField] private AnimationCurve timeDilationFieldStrength;

    [Header("Time Dilation Field Materials")]
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material inactiveMaterial;

    private FMOD.Studio.EventInstance TimeFieldSound;

    private MeshRenderer _meshRenderer;
    private bool _active = true;

    public void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        TimeFieldSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/TimeField/TimeField");
        TimeFieldSound.setParameterByName("FieldOff", 60.0f);
    }

    public void Update()
    {
        UpdateTimeDilation();
        TimeFieldSound.setParameterByName("FieldSize", gameObject.transform.localScale.x);
        
    }

    private void UpdateTimeDilation()
    {
        if (!_active) return;

        float timeFieldRadius = transform.localScale.x * 0.5f;
        if (timeFieldRadius <= 0) return;

        foreach (RelativeTime time in _affectedObjects)
        {
            float dist = Mathf.Clamp(Vector3.Distance(time.gameObject.transform.position, transform.position) / timeFieldRadius, 0.0f, 1.0f);
            time.SetTimeMultiplier(timeDilationFieldStrength.Evaluate(dist));
        }
    }

    public void ResizeTimeDilationField(InputAction.CallbackContext context)
    {
        transform.localScale = Vector3.one * Mathf.Clamp(transform.localScale.x + context.ReadValue<Vector2>().y * Time.deltaTime, minSize, maxSize);
    }

    public void ToggleTimeDilationField(InputAction.CallbackContext context)
    {
        Debug.Log("Toggling");
        _active = !_active;
        if (!_active)
        {
            TimeFieldSound.setParameterByName("FieldOff", 20f);
            TimeFieldSound.keyOff();

            _meshRenderer.material = inactiveMaterial;
            foreach (RelativeTime time in _affectedObjects)
            {
                time.SetTimeMultiplier(1.0f);
            }
           
        }
        else
        {
            TimeFieldSound.setParameterByName("FieldOff", 60.0f);
            TimeFieldSound.start();

            _meshRenderer.material = activeMaterial;
            
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        RelativeTime time = other.GetComponent<RelativeTime>();
        if (time != null)
        {
            _affectedObjects.Add(time);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        RelativeTime time = other.GetComponent<RelativeTime>();
        if (time != null)
        {
            time.SetTimeMultiplier(1.0f); //reset the time flow of object to normal
            _affectedObjects.Remove(time);
        }
    }
}