using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public class Gravity : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private Vector3 gravityDir = Vector3.down;
    [SerializeField] private float gravityStrength = 20f;
    [SerializeField] private float maxSpeed = 10.0f;
    [SerializeField] private float highlightIntensity = 10.0f;

    private float _timeMultiplier = 1.0f;
    private float _normalMass;

    private float _epsilon = 1e-6f;

    private Rigidbody _rb;
    private Material _material;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _normalMass = _rb.mass;
        _material = GetComponent<MeshRenderer>().material;
    }

    void FixedUpdate()
    {
        if (!_rb.isKinematic)
        {
            Vector3 force = gravityDir * gravityStrength * Time.fixedDeltaTime * _timeMultiplier * _normalMass; // use normal mass here as we don't need it affected by time
            //Debug.Log("The gravitational force is: " + force);
            //_rb.AddForce(force, ForceMode.Impulse);
            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity + gravityDir * gravityStrength * Time.fixedDeltaTime * _timeMultiplier * _normalMass, maxSpeed);

            //_rb.velocity = Vector3.ClampMagnitude(_rb.velocity, 30.0f); Maybe?
        }
    }

    public void SetGravityDir(Vector3 dir)
    {
        gravityDir = dir;
    }

    //Call this function in fixed updates and events as the values will only be useful in physics calculation
    public void SetTimeSpeed(float timeSpeed)
    {
        if (Math.Abs(timeSpeed - _timeMultiplier) < _epsilon) return;

        //reset the values to normal
        if (_timeMultiplier >= _epsilon)
        {
            _rb.velocity /= _timeMultiplier;
            _rb.angularVelocity /= _timeMultiplier;
            _rb.mass = _normalMass * _timeMultiplier;
        }

        _timeMultiplier = timeSpeed;

        if (_timeMultiplier < _epsilon)
        {
            _rb.isKinematic = true;
        }
        else
        {
            _rb.isKinematic = false;

            //set eveything to relativistic
            _rb.velocity *= _timeMultiplier;
            _rb.angularVelocity *= _timeMultiplier;
            _rb.mass = _normalMass / _timeMultiplier;
        }
    }

    public void LookAtObject()
    {
        _material.SetFloat("_HighlightIntensity", highlightIntensity);
        //Activate Arrows here
    }
    public void LookAwayFromObject()
    {
        _material.SetFloat("_HighlightIntensity", 1.0f);
        //Deactivate Arrows here
    }

}
