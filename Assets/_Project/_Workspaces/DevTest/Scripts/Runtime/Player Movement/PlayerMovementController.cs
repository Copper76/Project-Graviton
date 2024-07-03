using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputManager))]
public class PlayerMovementController : MonoBehaviour
{
    private InputManager _inputManager;
    private Vector2 _moveDirection = Vector2.zero;

    [Header("Camera")]
    [SerializeField] private Vector2 mouseSensitivity;

    private Transform _cameraTransform;

    [Header("Movement")]
    [SerializeField] private Vector2 arialTerminalVelocity;
    [SerializeField] private float groundTerminalVelocity;
    [SerializeField] private float groundMoveForce;
    [SerializeField] private float airMoveForce;
    [Range(0f, 10f)] [SerializeField] private float dampingStrength;
    [Range(0f, 1f)] [SerializeField] private float airDampingRatio;
    
    [Header("Jumping")]
    [SerializeField] private float initialJumpSpeed;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpBufferTime;
    [Range(0f, 1f)] [SerializeField] private float slopeJumpNormalBias;
    
    private float _coyoteTimeCounter;
    private float _jumpBufferCounter;
    
    [Header("Ground/Slope")]
    [SerializeField] private float groundDetectionRange;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float slopeDetectionRange;
    [SerializeField] private float maxSlopeAngle;

    [Header("Camera Shake")]
    [SerializeField] private float cameraShakeMultiplier = 1.0f;
    [SerializeField] private float cameraSmoothSpeed = 1.0f;
    [SerializeField] private AnimationCurve cameraShakeCurve;
    [SerializeField] private Vector3[] _cameraShakeTargets = new Vector3[] { new Vector3(-2.0f, -1.0f, 0.0f), Vector3.up, new Vector3(2.0f, -1.0f, 0.0f), Vector3.up };
    private Vector3 _cameraOffset;
    private Vector3 _cameraTargetPosition = Vector3.zero;
    private int _nextCameraShakeIndex = 0;
    private float _cameraShakeTime = 0.0f;

    private bool _isGrounded;
    private bool _isOnSlope;
    private bool _isSteepSlope;
    private Vector3 _slopeNormal;

    private Rigidbody _rb;

    private Vector3 _moveForce;

    private Vector2 _lookDirection;
    private Vector2 _mouseCameraRotation = Vector2.zero;

    private const float Epsilon = 1e-6f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _cameraOffset = Camera.main.transform.localPosition;
        InitializeComponents();
        LockCursor();
    }

    void FixedUpdate()
    {
        Move();
        Damping();
        EnforceTerminalVelocity();
    }

    private void Update()
    {
        DetectGround();
        DetectSlope();
        
        ReadInput();
        JumpCheck();
        Look();
        CameraShake();
    }

    private void InitializeComponents()
    {
        _inputManager = FindObjectOfType<InputManager>();
        _cameraTransform = Camera.main?.transform;
    }

    private void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void ReadInput()
    {
        _moveDirection = _inputManager.playerInputActions.Player.Move.ReadValue<Vector2>();
    }

    private void Damping()
    {
        if (_moveDirection != Vector2.zero) return;

        Vector3 horizontalPlaneVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        Vector3 damping = -horizontalPlaneVelocity * dampingStrength * Time.fixedDeltaTime;
        if (!_isGrounded) damping *= airDampingRatio;
        _rb.AddForce(damping, ForceMode.VelocityChange);
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
        //if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

            _isSteepSlope = slopeAngle > maxSlopeAngle;
            _isOnSlope = slopeAngle > 0f && slopeAngle < maxSlopeAngle;
            _slopeNormal = _isOnSlope ? hit.normal : Vector3.up;

            Debug.Log($"On Slope: {_isOnSlope} Slope Angle: {slopeAngle}");
        }
        else
        {
            _isOnSlope = false;
            _slopeNormal = Vector3.up;
        }
        Debug.DrawRay(transform.position, Vector3.down * groundDetectionRange, Color.red, Time.fixedDeltaTime);
    }

    private void EnforceTerminalVelocity()
    {
        if (_isGrounded || _isOnSlope)
        {
            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, groundTerminalVelocity);
        }
        else
        {
            Vector2 horizontalVelocity = new Vector2(_rb.velocity.x, _rb.velocity.z);
            horizontalVelocity = Vector2.ClampMagnitude(horizontalVelocity, arialTerminalVelocity.x);
            float clampedY = Mathf.Clamp(_rb.velocity.y, -arialTerminalVelocity.y, arialTerminalVelocity.y);
            _rb.velocity = new Vector3(horizontalVelocity.x, clampedY, horizontalVelocity.y);
        }
    }

    public void Move()
    {
        if (_moveDirection == Vector2.zero) return;

        if (_isSteepSlope) return;
        
        Vector3 normalizedMoveDirection = new Vector3(_moveDirection.x, 0f, _moveDirection.y).normalized;
        float forceStrength = _isGrounded ? groundMoveForce : airMoveForce; //TODO set air relative to ground
        
        _moveForce = transform.TransformDirection(normalizedMoveDirection * forceStrength * Time.fixedDeltaTime); // Made this global

        SlopeCompensation();

        _rb.AddForce(_moveForce, ForceMode.VelocityChange);
    }

    private void SlopeCompensation()
    {
        if (_isOnSlope)
        {
            _moveForce = Vector3.ProjectOnPlane(_moveForce, _slopeNormal);

            //Vector3 slopeParallelGravity = Vector3.Project(Physics.gravity, _slopeNormal);

            //_rb.AddForce(-slopeParallelGravity, ForceMode.Acceleration); // removed negative sign
        }
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
        if (_isSteepSlope) return;
        Vector3 slopeJumpDirection = Vector3.Lerp(Vector3.up, _slopeNormal, slopeJumpNormalBias);
        Vector3 jumpDirection = _isOnSlope ? slopeJumpDirection : Vector3.up;
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        _rb.AddForce(jumpDirection * initialJumpSpeed, ForceMode.VelocityChange);
    }

    private void Look()
    {
        _lookDirection = _inputManager.playerInputActions.Player.Look.ReadValue<Vector2>();

        _mouseCameraRotation.x -= _lookDirection.y * mouseSensitivity.x * Time.deltaTime;
        _mouseCameraRotation.y += _lookDirection.x * mouseSensitivity.y * Time.deltaTime;

        _mouseCameraRotation.x = Mathf.Clamp(_mouseCameraRotation.x, -90f, 90f);

        transform.rotation = Quaternion.Euler(0.0f, _mouseCameraRotation.y, 0);
        _cameraTransform.rotation = Quaternion.Euler(_mouseCameraRotation.x, _mouseCameraRotation.y, 0.0f);
    }

    private void CameraShake()
    {
        if (_moveDirection == Vector2.zero)
        {
            _cameraTargetPosition = _cameraOffset;
            _nextCameraShakeIndex = 0;
            _cameraShakeTime = 0.0f;
        }

        if (Vector3.Distance(Camera.main.transform.localPosition, _cameraTargetPosition) < Epsilon)
        {
            Camera.main.transform.localPosition = _cameraTargetPosition;
            _nextCameraShakeIndex = (_nextCameraShakeIndex + 1) % _cameraShakeTargets.Length;
            _cameraTargetPosition = _cameraShakeTargets[_nextCameraShakeIndex] * cameraShakeMultiplier + _cameraOffset;
            _cameraShakeTime = 0.0f;
        }
        else
        {
            _cameraShakeTime += Time.deltaTime * cameraSmoothSpeed;
            float curveValue = cameraShakeCurve.Evaluate(_cameraShakeTime);
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, _cameraTargetPosition, curveValue);
        }
    }
}
