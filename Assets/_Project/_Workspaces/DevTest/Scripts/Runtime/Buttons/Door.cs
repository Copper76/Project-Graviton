using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Vector3 offSet;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private AnimationCurve offsetCurve;

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
        _isOpen = !_isOpen;
        _currentStartPosition = transform.position;
        _targetPosition = _isOpen ? _openPosition : _closedPosition;
        _animationTime = 0f;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _targetPosition) < Epsilon)
            return;

        float deltaTime = _useRelativeTime ? _relativeTime.DeltaTime() : Time.deltaTime;
        _animationTime += deltaTime * smoothSpeed;
        float curveValue = offsetCurve.Evaluate(_animationTime);
        transform.position = Vector3.Lerp(_currentStartPosition, _targetPosition, curveValue);
    }
}