using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private InputManager _inputManager;
    private Vector2 _moveDirection = Vector2.zero;

    [Header("Camera")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Vector2 mouseSensitivity;
    [SerializeField] private float tiltAngle = 10f;
    [SerializeField] private float tiltSmooth = 5f;
    
    private Transform _cameraTransform;
    private float _currentTilt = 0f;

    [Header("Movement")]
    [SerializeField] private Vector3 terminalVelocity;
    [SerializeField] private float groundTerminalVelocity;
    [SerializeField] private float groundMoveForce;
    [SerializeField] private float airMoveForce;

    private CapsuleCollider _collider;
    
    [Header("Jumping")]
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpBufferTime;
    
    private float _coyoteTimeCounter;
    private float _jumpBufferCounter;
    
    [Header("Ground/Slope")]
    [SerializeField] private float groundDetectionRange;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float slopeDetectionRange;
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private PhysicMaterial zeroFriction;
    
    private bool _isGrounded;
    private bool _isOnSlope;
    private Vector3 _slopeNormal;

    private Rigidbody _rb;

    private Vector3 _moveForce;

    private Vector2 _lookDirection;
    private Vector2 _mouseCameraRotation = Vector2.zero;


    

    public static PlayerMovementController Instance;
    private void Awake()
    {
        Instance = this;
    }
    public Vector2 GetCameraRotation() => _mouseCameraRotation;

    void Start()
    {
        InitializeComponents();
        LockCursor();

        _collider = GetComponent<CapsuleCollider>();
    }

    void FixedUpdate()
    {
        Move(_moveDirection);
        EnforceTerminalVelocity();
    }

    private void Update()
    {
        DetectGround();
        DetectSlope();
        
        ZeroFrictionCheck();
        
        ReadInput();
        JumpCheck();
        Look();
        TiltCamera(_moveDirection);
    }

    private void InitializeComponents()
    {
        _inputManager = FindObjectOfType<InputManager>();
        _rb = GetComponent<Rigidbody>();
        _cameraTransform = playerCamera.GetComponent<Transform>();
    }

    private void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ReadInput()
    {
        _moveDirection = _inputManager.playerInputActions.Player.Move.ReadValue<Vector2>();
    }

    private void DetectGround()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDetectionRange, groundLayer);
        Debug.DrawRay(transform.position, Vector3.down * groundDetectionRange, Color.green, Time.fixedDeltaTime);
    }

    private void DetectSlope()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, slopeDetectionRange, groundLayer))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            
            _isOnSlope = slopeAngle > 0f && slopeAngle < maxSlopeAngle;
            _slopeNormal = _isOnSlope ? hit.normal : Vector3.up;
            
            //Debug.Log($"On Slope: {_isOnSlope} Slope Angle: {slopeAngle}");
        }
        else
        {
            _isOnSlope = false;
            _slopeNormal = Vector3.up;
        }
        Debug.DrawRay(transform.position, Vector3.down * groundDetectionRange, Color.red, Time.fixedDeltaTime);
    }

    private void ZeroFrictionCheck()
    {
        if (_isGrounded || _isOnSlope)
        {
            _collider.material = null;
        }
        else
        {
            _collider.material = zeroFriction;
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

    public void Move(Vector2 direction)
    {
        if (direction == Vector2.zero) return;
        
        Vector3 normalizedMoveDirection = new Vector3(direction.x, 0f, direction.y).normalized;
        float forceStrength = _isGrounded ? groundMoveForce : airMoveForce;
        
        _moveForce = normalizedMoveDirection * forceStrength * Time.fixedDeltaTime;

        if (_isOnSlope)
        {
            _moveForce = Vector3.ProjectOnPlane(_moveForce, _slopeNormal);
        }

        _rb.AddRelativeForce(_moveForce, ForceMode.VelocityChange);
    }

    private void JumpCheck()
    {
        UpdateCoyoteTime();
        UpdateJumpBuffer();

        if (_coyoteTimeCounter > 0f && _jumpBufferCounter > 0f)
        {
            Jump();
            _jumpBufferCounter = 0f;
        }

        if (!_inputManager.playerInputActions.Player.Jump.inProgress && _rb.velocity.y > 0f)
        {
            _coyoteTimeCounter = 0f;
        }
    }

    private void UpdateCoyoteTime()
    {
        if (_isGrounded || _isOnSlope)
        {
            _coyoteTimeCounter = coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }
        _coyoteTimeCounter = Mathf.Max(_coyoteTimeCounter, 0f);
    }

    private void UpdateJumpBuffer()
    {
        if (_inputManager.playerInputActions.Player.Jump.triggered)
        {
            _jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
        _jumpBufferCounter = Mathf.Max(_jumpBufferCounter, 0f);
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
            float targetTilt = direction.x == 0 ? 0f : (direction.x < 0 ? tiltAngle : -tiltAngle);
            _currentTilt = Mathf.Lerp(_currentTilt, targetTilt, tiltSmooth * Time.deltaTime);
        }
        else
        {
            _currentTilt = Mathf.Lerp(_currentTilt, 0f, tiltSmooth * Time.deltaTime);
        }
        _cameraTransform.rotation = Quaternion.Euler(_mouseCameraRotation.x, _mouseCameraRotation.y, _currentTilt);


        OrientationManager.Instance.UpdateOrientation(_cameraTransform.rotation);
    }
}
