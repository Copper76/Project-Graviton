using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeTime : MonoBehaviour
{
    [SerializeField] private float _timeMultiplier = 1.0f;

    private const float Epsilon = 1e-6f;

    private Gravity _gravity;

    public float GetTimeMultiplier()
    {
        return _timeMultiplier;
    }

    public void SetTimeMultiplier(float timeMultiplier)
    {
        if (Mathf.Abs(timeMultiplier - _timeMultiplier) < Epsilon) return;

        if (_timeMultiplier >= Epsilon && _gravity != null)
        {
            _gravity.ResetProperties(_timeMultiplier);
        }

        _timeMultiplier = timeMultiplier;

        if (_gravity != null)
        {
            _gravity.UpdateProperties(_timeMultiplier, _timeMultiplier < Epsilon);
        }

    }

    public void SetGravityReference(Gravity gravity)
    {
        _gravity = gravity;
    }

    public float DeltaTime()
    {
        return Time.deltaTime * _timeMultiplier;
    }

    public float FixedDeltaTime()
    {
        return Time.fixedDeltaTime * _timeMultiplier;
    }
}
