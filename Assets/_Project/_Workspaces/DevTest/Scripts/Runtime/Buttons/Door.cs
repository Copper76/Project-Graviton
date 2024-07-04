using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Door : MonoBehaviour
{
    [SerializeField] private Vector3 offSet;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private AnimationCurve offsetCurve;

    private FMOD.Studio.EventInstance DoorOpeningSound;

    private const float Epsilon = 1.0e-6f;

    private Vector3 _closedPosition;
    private Vector3 _openPosition;
    private Vector3 _currentStartPosition;
    private Vector3 _targetPosition;
    
    private bool _isOpen;
    private float _animationTime;
    
    private RelativeTime _relativeTime;
    private bool _useRelativeTime;

    

    private void Start()
    {
        DoorOpeningSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/GeneralEnvironment/DoorOpening");
        DoorOpeningSound.setParameterByName("DoorFullClosed", 20f);
        DoorOpeningSound.setParameterByName("TimeDilation", _relativeTime.GetTimeMultiplier());

        _closedPosition = transform.position;
        _openPosition = _closedPosition + offSet;

        _isOpen = false;
        _targetPosition = _closedPosition;
        _currentStartPosition = _closedPosition;

        _useRelativeTime = false;
        
        if (TryGetComponent(out RelativeTime relativeTime))
        {
            _relativeTime = relativeTime;
            _useRelativeTime = true;
        }
    }

    public void ToggleDoor()
    {
        DoorOpeningSound.start();
        
        _isOpen = !_isOpen;
        _currentStartPosition = transform.position;
        _targetPosition = _isOpen ? _openPosition : _closedPosition;
        _animationTime = 0f;
        

    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _targetPosition) < Epsilon)
        {
            DoorOpeningSound.setParameterByName("DoorFullClosed", 20f);
            DoorOpeningSound.release();
            return;
        }

        DoorOpeningSound.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject.transform));

        float deltaTime = _useRelativeTime ? _relativeTime.DeltaTime() : Time.deltaTime;
        _animationTime += deltaTime * smoothSpeed;
        float curveValue = offsetCurve.Evaluate(_animationTime);
        transform.position = Vector3.Lerp(_currentStartPosition, _targetPosition, curveValue);
    }
}