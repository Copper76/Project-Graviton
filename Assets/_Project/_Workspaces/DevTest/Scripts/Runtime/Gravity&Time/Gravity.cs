using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(SphereCollider))]
public class Gravity : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private Vector3 gravityDir = Vector3.down;
<<<<<<< HEAD
    [SerializeField] private float gravityStrength = 9.81f;
=======
    [SerializeField] private float gravityStrength = 20f;
    [SerializeField] private float maxSpeed = 10.0f;
    [SerializeField] private float highlightIntensity = 10.0f;
>>>>>>> master

    [SerializeField] private float timeMultiplier = 1.0f;

<<<<<<< HEAD
    [SerializeField] private float normalMass;

    private Rigidbody rb;
=======
    private float _epsilon = 1e-6f;
>>>>>>> master

    private Rigidbody _rb;
    private Material _material;
    private SphereCollider _extendCollider;

    void Awake()
    {
<<<<<<< HEAD
        rb = GetComponent<Rigidbody>();
        normalMass = rb.mass;
=======
        _rb = GetComponent<Rigidbody>();
        _normalMass = _rb.mass;
        _material = GetComponent<MeshRenderer>().material;
        _extendCollider = GetComponent<SphereCollider>();
>>>>>>> master
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeMultiplier == 0)
        {
<<<<<<< HEAD
            rb.isKinematic = true;
        }
        else
        {
            //set everything to normal
            rb.isKinematic = false;
            rb.velocity /= timeMultiplier;
            rb.angularVelocity /= timeMultiplier;
            rb.mass = normalMass;

            rb.AddForce(gravityDir * gravityStrength * Time.deltaTime * timeMultiplier * rb.mass, ForceMode.Impulse); // need to take terminal speed into account

            //set eveything to relativistic
            rb.velocity *= timeMultiplier;
            rb.angularVelocity *= timeMultiplier;
            rb.mass = normalMass / timeMultiplier;
=======
            Vector3 force = gravityDir * gravityStrength * Time.fixedDeltaTime * _timeMultiplier * _normalMass; // use normal mass here as we don't need it affected by time
            //Debug.Log("The gravitational force is: " + force);
            //_rb.AddForce(force, ForceMode.Impulse);
            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity + gravityDir * gravityStrength * Time.fixedDeltaTime * _timeMultiplier * _normalMass, maxSpeed);

            //_rb.velocity = Vector3.ClampMagnitude(_rb.velocity, 30.0f); Maybe?
>>>>>>> master
        }
    }

    public void SetGravityDir(Vector3 dir)
    {
        gravityDir = dir;
    }

<<<<<<< HEAD
    public void ChangeTimeSpeed(float timeSpeed)
    {
        timeMultiplier = timeSpeed;
=======
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
>>>>>>> master
    }

    public void LookAtObject()
    {
        _material.SetFloat("_HighlightIntensity", highlightIntensity);
        _extendCollider.enabled = true;
        //Activate Arrows here
    }
    public void LookAwayFromObject()
    {
        _material.SetFloat("_HighlightIntensity", 1.0f);
        _extendCollider.enabled = false;
        //Deactivate Arrows here
    }

}
