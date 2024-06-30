using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Gravity : MonoBehaviour
{
    [SerializeField] private Vector3 gravityDir = Vector3.down;
    [SerializeField] private float gravityStrength = 20f;

    [SerializeField] private float timeMultiplier = 1.0f;

    [SerializeField] private float normalMass;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        normalMass = rb.mass;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeMultiplier == 0)
        {
            rb.isKinematic = true;
        }
        else
        {
            //set everything to normal
            rb.isKinematic = false;
            //rb.velocity /= timeMultiplier;
            //rb.angularVelocity /= timeMultiplier;
            //rb.mass = normalMass;

            rb.AddForce(gravityDir * gravityStrength * Time.deltaTime * timeMultiplier * rb.mass, ForceMode.Impulse); // need to take terminal speed into account

            //set eveything to relativistic
            rb.velocity *= timeMultiplier;
            rb.angularVelocity *= timeMultiplier;
            rb.mass = normalMass / timeMultiplier;
        }
    }

    public void ChangeGravityDir(Vector3 dir)
    {
        gravityDir = dir;
    }

    public void ChangeTimeSpeed(float timeSpeed)
    {
        timeMultiplier = timeSpeed;
    }
}
