using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Gravity : MonoBehaviour
{
    [SerializeField] private Vector3 gravityDir = Vector3.down;
    [SerializeField] private float gravityStrength = 20f;

    private float _timeMultiplier = 1.0f;
    private float _normalMass;

    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _normalMass = _rb.mass;
    }

    void FixedUpdate()
    {
        if (!_rb.isKinematic)
        {
            Vector3 force = gravityDir * gravityStrength * Time.deltaTime * _timeMultiplier * _normalMass; // use normal mass here as we don't need it affected by time
            Debug.Log("The gravitational force is: " + force);
            _rb.AddForce(force, ForceMode.Impulse);
        }
    }

    public void ChangeGravityDir(Vector3 dir)
    {
        gravityDir = dir;
    }

    //Call this function in fixed updates and events as the values will only be useful in physics calculation
    public void ChangeTimeSpeed(float timeSpeed)
    {
        if (timeSpeed == _timeMultiplier) return; //guard clause to avoid unnecessary calculation

        //reset the values to normal
        if (_timeMultiplier != 0)
        {
            _rb.velocity /= _timeMultiplier;
            _rb.angularVelocity /= _timeMultiplier;
            _rb.mass = _normalMass * _timeMultiplier;
        }

        _timeMultiplier = timeSpeed;

        if (_timeMultiplier == 0)
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
}
