using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{

    private InputManager _inputManager;
    private Vector2 _moveDirection = Vector2.zero;
    
    [SerializeField] private Camera playerCamera;
    private Transform _cameraTransform;
    
    [SerializeField] private Vector2 mouseSensitivity;
    [SerializeField] private float tiltAngle = 10f;
    [SerializeField] private float tiltSmooth = 5f;
    private float _currentTilt = 0f;
    
    [SerializeField] private Vector3 terminalVelocity;

    [SerializeField] private float groundDetectionRange;
    [SerializeField] private LayerMask groundLayer;
    
    [SerializeField] private float groundTerminalVelocity;
    [SerializeField] private float groundMoveForce;
    [SerializeField] private float airMoveForce;
    
    [Header("Jumping")]
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpBufferTime;
    
    private float _coyoteTimeCounter;
    private float _jumpBufferCounter;
    
    private bool _isGrounded;
    
    

    private Rigidbody _rb;
    
    private Vector2 _lookDirection;
    Vector2 _mouseCameraRotation = Vector2.zero;
    


    void Start()
    {
        _inputManager = FindObjectOfType<InputManager>();
        _rb = GetComponent<Rigidbody>();
        _cameraTransform = playerCamera.GetComponent<Transform>();
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {

        float forceStrength = _isGrounded ? groundMoveForce : airMoveForce;
        Move(_moveDirection, forceStrength);
        
        EnforceTerminalVelocity();
    }

    private void Update()
    {
        _moveDirection = _inputManager.playerInputActions.Player.Move.ReadValue<Vector2>();

        DetectGroundPlane();
        JumpCheck();
        Look();
        TiltCamera(_moveDirection);
    }

    private void DetectGroundPlane()
    {
        RaycastHit hit;
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundDetectionRange, groundLayer);
        Debug.DrawRay(transform.position, Vector3.down * groundDetectionRange, Color.green, Time.fixedDeltaTime);
        
        Debug.Log(_isGrounded);
        
        if (_isGrounded)
        {
            
        }
    }

    private void EnforceTerminalVelocity()
    {
        if (_isGrounded)
        {
            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, groundTerminalVelocity);
        }
        else
        {
            Vector2 horizontalVelocity = new Vector2(_rb.velocity.x, _rb.velocity.z);
            horizontalVelocity = Vector2.ClampMagnitude(horizontalVelocity, terminalVelocity.x);

            float clampedY = Mathf.Clamp(_rb.velocity.y, -terminalVelocity.y, terminalVelocity.y);

            _rb.velocity = new Vector3(horizontalVelocity.x, clampedY, horizontalVelocity.y);
        }
    }

    
    public void Move(Vector2 direction, float forceStrength)
    {
        if (direction != Vector2.zero)
        {
            Vector3 normalizedMoveDirection = new Vector3(direction.x, 0f, direction.y).normalized;
            Vector3 moveForce = normalizedMoveDirection * forceStrength * Time.fixedDeltaTime;
            _rb.AddRelativeForce(moveForce, ForceMode.Acceleration);
        }
    }

    private void JumpCheck()
    {
        if (_isGrounded)
        {
            _coyoteTimeCounter = coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }
        _coyoteTimeCounter = Mathf.Max(_coyoteTimeCounter, 0f);

        if (_inputManager.playerInputActions.Player.Jump.triggered)
        {
            _jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
        _jumpBufferCounter = Mathf.Max(_jumpBufferCounter, 0f);


        if (_coyoteTimeCounter > 0f && _jumpBufferCounter > 0f)
        {
            Jump();

            _jumpBufferCounter = 0f;
        }

        if (!_inputManager.playerInputActions.Player.Jump.inProgress && _rb.velocity.y > 0f)
        {
            _coyoteTimeCounter = 0f;
        }

        Debug.Log($"coyote time: {_coyoteTimeCounter}, jump buffer: {_jumpBufferCounter}");
    }
    
    
    public void Jump()
    {
        _rb.velocity = Vector3.up * jumpSpeed;
    }
    
    private void Look()
    {
        _lookDirection = _inputManager.playerInputActions.Player.Look.ReadValue<Vector2>();

        _mouseCameraRotation.x -= _lookDirection.y * mouseSensitivity.x * Time.deltaTime;
        _mouseCameraRotation.y += _lookDirection.x * mouseSensitivity.y * Time.deltaTime;

        _mouseCameraRotation.x = Mathf.Clamp(_mouseCameraRotation.x, -90f, 90f);

        transform.rotation = Quaternion.Euler(0, _mouseCameraRotation.y, 0);
    }
    
    private void TiltCamera(Vector2 direction)
    {
        if (_isGrounded)
        {
            float targetTilt = 0f;
            if (direction.x != 0)
            {
                targetTilt = direction.x < 0 ? tiltAngle : -tiltAngle;
            }

            _currentTilt = Mathf.Lerp(_currentTilt, targetTilt, tiltSmooth * Time.deltaTime);
            _cameraTransform.rotation = Quaternion.Euler(_mouseCameraRotation.x, _mouseCameraRotation.y, _currentTilt);
        }
        else
        {
            _currentTilt = Mathf.Lerp(_currentTilt, 0f, tiltSmooth * Time.deltaTime);
            _cameraTransform.rotation = Quaternion.Euler(_mouseCameraRotation.x, _mouseCameraRotation.y, _currentTilt);
        }
    }
}
