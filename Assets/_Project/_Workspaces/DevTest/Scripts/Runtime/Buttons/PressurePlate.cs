using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [Range(0f, 1f)] [SerializeField] private float plateDepression;
    [SerializeField] private float depressionSpeed;
    [SerializeField] private AnimationCurve depressionCurve;
    [SerializeField] private Transform plateTransform;

    [SerializeField] private bool triggersOnExit;
    [SerializeField] private UnityEvent triggeredEvents;

    private Vector3 _initialPosition;
    private Vector3 _targetPosition;
    private Vector3 _currentStartPosition;

    private float _depressionTime;
    private int _rigidbodyCount;

    private void Start()
    {
        _initialPosition = plateTransform.localPosition;
        _targetPosition = _initialPosition;
        _currentStartPosition = _initialPosition;
        _rigidbodyCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() == null) return;

        if (_rigidbodyCount == 0)
        {
            Interact();
        }

        _rigidbodyCount++;
        SetDepressionDepth(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>() == null) return;

        _rigidbodyCount--;
        if (_rigidbodyCount > 0) return;

        _rigidbodyCount = 0;
        SetDepressionDepth(false);

        if (triggersOnExit)
        {
            Interact();
        }
    }

    private void Interact()
    {
        triggeredEvents.Invoke();
    }

    private void SetDepressionDepth(bool isDepressed)
    {
        _currentStartPosition = plateTransform.localPosition;
        float depressionDepth = isDepressed ? _initialPosition.y + plateTransform.localScale.y * -plateDepression : _initialPosition.y;
        _targetPosition = new Vector3(_initialPosition.x, depressionDepth, _initialPosition.z);
        _depressionTime = 0f;
    }

    private void Update()
    {
        if (_depressionTime > 1f) return;

        _depressionTime += Time.deltaTime * depressionSpeed;
        float curveValue = depressionCurve.Evaluate(_depressionTime);
        plateTransform.localPosition = Vector3.Lerp(_currentStartPosition, _targetPosition, curveValue);
    }
}
