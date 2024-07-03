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

    private void Start()
    {
        _closedPosition = transform.position;
        _openPosition = _closedPosition + offSet;

        _isOpen = false;
        _targetPosition = _closedPosition;
        _currentStartPosition = _closedPosition;
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

        _animationTime += Time.deltaTime * smoothSpeed;
        float curveValue = offsetCurve.Evaluate(_animationTime);
        transform.position = Vector3.Lerp(_currentStartPosition, _targetPosition, curveValue);
    }
}