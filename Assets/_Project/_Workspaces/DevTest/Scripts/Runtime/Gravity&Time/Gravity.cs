using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(RelativeTime))]
public class Gravity : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private Vector3 gravityDir = Vector3.down;
    [SerializeField] private float gravityStrength = 20f;
    [SerializeField] private float maxSpeed = 10.0f;
    [SerializeField] private float highlightIntensity = 10.0f;

    private float _normalMass;

    private Rigidbody _rb;
    private Material _material;
    private SphereCollider _extendCollider;
    private RelativeTime _relativeTime;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _normalMass = _rb.mass;
        _material = GetComponent<MeshRenderer>().material;
        _extendCollider = GetComponent<SphereCollider>();
        _relativeTime = GetComponent<RelativeTime>();
    }

    private void Start()
    {
        _relativeTime.SetGravityReference(this);
    }

    void FixedUpdate()
    {
        if (!_rb.isKinematic)
        {
            Vector3 force = gravityDir * gravityStrength * _relativeTime.FixedDeltaTime() * _normalMass; // use normal mass here as we don't need it affected by time
            //Debug.Log("The gravitational force is: " + force);
            //_rb.AddForce(force, ForceMode.Impulse);
            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity + gravityDir * gravityStrength * _relativeTime.FixedDeltaTime() * _normalMass, maxSpeed);
        }
    }

    public void SetGravityDir(Vector3 dir)
    {
        if (gravityDir == dir) return;

        gravityDir = dir;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/GeneralEnvironment/ShapeBeginMove", GetComponent<Transform>().position);
        if (!_rb.isKinematic)
        {
            _rb.velocity = Vector3.zero; //reset velocity?
            _rb.angularVelocity = Vector3.zero;
        }
    }

    public Vector3 GetGravityDir()
    {
        return gravityDir;
    }

    public void ResetProperties(float currentTime)
    {
        _rb.velocity /= currentTime;
        _rb.angularVelocity /= currentTime;
        _rb.mass = _normalMass * currentTime;
    }

    public void UpdateProperties(float newTime, bool isStatic)
    {
        if (isStatic)
        {
            _rb.isKinematic = true;
        }
        else
        {
            _rb.isKinematic = false;

            //set eveything to relativistic
            _rb.velocity *= newTime;
            _rb.angularVelocity *= newTime;
            _rb.mass = _normalMass / newTime;
        }
    }

    public void LookAtObject()
    {
        _material.SetFloat("_HighlightIntensity", highlightIntensity);
        _extendCollider.enabled = true;
    }
    public void LookAwayFromObject()
    {
        _material.SetFloat("_HighlightIntensity", 1.0f);
        _extendCollider.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/GeneralEnvironment/ShapeCollision", transform.position);
    }
}