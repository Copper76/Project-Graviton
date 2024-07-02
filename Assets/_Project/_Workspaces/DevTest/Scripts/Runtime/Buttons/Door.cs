using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Vector3 offSet;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private AnimationCurve offsetCurve;

    private const float Epsilon = 1.0e-6f;

    private Vector3 _closedPosition;
    private Vector3 _openPosition;
    private Vector3 _targetPosition;
    private bool _isOpen;

    private void Start()
    {
        _closedPosition = transform.position;
        _openPosition = _closedPosition + offSet;
     
        _isOpen = false;
        _targetPosition = _closedPosition;
    }

    public void ToggleDoor()
    {
        _isOpen = !_isOpen;
        _targetPosition = _isOpen ? _openPosition : _closedPosition;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _targetPosition) < Epsilon)
            return;

        float curveValue = offsetCurve.Evaluate(Time.time);
        transform.position = Vector3.Lerp(transform.position, _targetPosition, smoothSpeed * curveValue * Time.deltaTime);
    }
}