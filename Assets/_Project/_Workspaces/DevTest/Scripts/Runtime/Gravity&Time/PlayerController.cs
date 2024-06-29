using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private bool _isGrounded;
    private Vector2 _moveVector;
    private Rigidbody _rb;

    private float _groundCheckDist;
    private float _verticalRotationMinimum = -60f;
    private float _verticalRotationMaximum = 60f;
    private float _cameraRotationX = 0.0f;

    [Header("Speed setup")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float limitSpeed;

    [Header("Sens setup")]
    [SerializeField] private float horizontalSens;
    [SerializeField] private float verticalSens;

    [Header("Jump setup")]
    [SerializeField] private float jumpForce;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _isGrounded = true;
        _groundCheckDist = 1.1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        _isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, _groundCheckDist);
        Vector3 moveDir = new Vector3(_moveVector.x * moveSpeed, 0.0f, _moveVector.y * moveSpeed);
        moveDir = transform.TransformDirection(moveDir);
        _rb.AddForce(GetSlopeDirection(moveDir, transform.localScale.y * 0.5f) * _rb.mass);
        if (Math.Abs(_rb.velocity.x) > limitSpeed)
        {
            _rb.velocity = new Vector3(_rb.velocity.x < 0 ? -limitSpeed : limitSpeed, _rb.velocity.y, _rb.velocity.z);
        }
        if (Math.Abs(_rb.velocity.z) > limitSpeed)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y, _rb.velocity.z < 0 ? -limitSpeed : limitSpeed);
        }
    }

    private Vector3 GetSlopeDirection(Vector3 moveDir, float checkDist)
    {
        RaycastHit slopeHit;
        if (Physics.Raycast(transform.position, -transform.up, out slopeHit, checkDist + 1f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            if (angle != 0)
            {
                return Vector3.ProjectOnPlane(moveDir, slopeHit.normal);
            }
        }
        return moveDir;
    }

    public void RotateX(InputAction.CallbackContext context)
    {
        Vector3 rotation = transform.localEulerAngles;
        rotation.y += horizontalSens * context.ReadValue<float>() * Time.deltaTime;
        transform.localEulerAngles = rotation;
    }

    //normal camera
    public void RotateY(InputAction.CallbackContext context)
    {
        _cameraRotationX += verticalSens * context.ReadValue<float>() * Time.deltaTime;
        _cameraRotationX = Mathf.Clamp(_cameraRotationX, _verticalRotationMinimum, _verticalRotationMaximum);
        Vector3 cameraRotation = Camera.main.transform.localEulerAngles;
        cameraRotation.x = _cameraRotationX;
        Camera.main.transform.localEulerAngles = cameraRotation;
    }

    public void Move(InputAction.CallbackContext context)
    {
        _moveVector = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_isGrounded && context.performed)
        {
            _rb.AddForce(new Vector3(0, jumpForce, 0));
        }
    }
}
