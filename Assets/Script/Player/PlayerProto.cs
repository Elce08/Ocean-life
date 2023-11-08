using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerProto : MonoBehaviour
{
    Vector2 move;
    Vector2 look;
    public bool jump;
    public bool sprint;
    public bool sink;
    public bool inWater = false;

    public enum Space
    {
        Ground,
        Water,
    }

    public Space space = Space.Ground;

    CharacterController controller;
    private PlayerInput inputActions;
    GameObject mainCamera;

    [Header("Player")]
    public float moveSpeed = 4.0f;
    public float sprintSpeed = 6.0f;
    public float rotationSpeed = 1.0f;
    public float speedChangeRate = 10.0f;

    [Space(10)]
    public float jumpHeight = 1.2f;
    public float gravity = -15.0f;

    [Space(10)]
    public float jumpTimeout = 0.1f;
    public float FallTimeout = 0.15f;

    [Header("Groud Check")]
    public bool grounded;
    public float groundedOffset = -0.14f;
    public float groundedRadius = 0.5f;
    public LayerMask GroundLayer;

    [Header("Cinemachine")]
    public GameObject CinemachineCameraTarget;
    public float topClamp = 90.0f;
    public float bottomClamp = -90.0f;

    [Header("Mouse Cursor Setting")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    float _cinemachineTargetPitchX;
    float _cinemachineTargetPitchY;

    float _speed;
    float _rotationVelocity;
    float _verticalVelocity;
    float _terminalVelocity = 53.0f;

    float _jumpTimeoutDelta;
    float _fallTimeoutDelta;

    public float threshold = 0.01f;

    private void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Start()
    {
        controller = GetComponentInChildren<CharacterController>();
        inputActions = GetComponent<PlayerInput>();

        _jumpTimeoutDelta = jumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }

    private void Update()
    {
        GroundedCheck();
        Move();
        JumpAndGravity();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }   


    void Move()
    {
        float targetSpeed = sprint ? sprintSpeed : moveSpeed;

        if (move == Vector2.zero) targetSpeed = 0.0f;
        float currentHorizontalSpeed;
        if (space == Space.Ground) currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;
        else 
        {
            if(sink || jump)currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;
            else currentHorizontalSpeed = new Vector3(controller.velocity.x, controller.velocity.y, controller.velocity.z).magnitude;
        }


        float speedOffset = 0.1f;
        float inputMagnitude = move.magnitude;

        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * speedChangeRate);
        }
        else _speed = targetSpeed;

        Vector3 inputDirection = new(move.x, 0.0f, move.y);

        if(move != Vector2.zero)
        {
            if (space == Space.Ground) inputDirection = transform.right * move.x + transform.forward * move.y;
            else inputDirection = transform.right * move.x + transform.forward * move.y;
        }

        controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }

    void JumpAndGravity()
    {
        if(space == Space.Ground)
        {
            if (grounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_verticalVelocity < 0.0f) _verticalVelocity = -2.0f;
                if (jump && _jumpTimeoutDelta <= 0.0f) _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                if (_jumpTimeoutDelta >= 0.0f) _jumpTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                _jumpTimeoutDelta = jumpTimeout;
                if (FallTimeout >= 0.0f) _fallTimeoutDelta -= Time.deltaTime;
                jump = false;
                sink = false;
            }
            if (_verticalVelocity < _terminalVelocity) _verticalVelocity += gravity * Time.deltaTime;
        }
        if(space == Space.Water)
        {
            if (jump) _verticalVelocity = moveSpeed;
            else if (sink) _verticalVelocity = -moveSpeed;
            else _verticalVelocity = 0.0f;
        }
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        grounded = Physics.CheckSphere(spherePosition,groundedRadius,GroundLayer,QueryTriggerInteraction.Ignore);
    }

    void CameraRotation()
    {
        _cinemachineTargetPitchX += look.y * rotationSpeed;
        _cinemachineTargetPitchY += look.x * rotationSpeed;
        _rotationVelocity = look.x * rotationSpeed;


        _cinemachineTargetPitchX = ClampAngle(_cinemachineTargetPitchX, bottomClamp, topClamp);
        if (space != Space.Water)
        {
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitchX, 0.0f, 0.0f);
            transform.Rotate(Vector3.up * _rotationVelocity, UnityEngine.Space.World);
        }
        else
        {
            transform.rotation = Quaternion.Euler(_cinemachineTargetPitchX, _cinemachineTargetPitchY, 0.0f);
            transform.Rotate(Vector3.up * _rotationVelocity, UnityEngine.Space.World);
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if(cursorInputForLook) look = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jump = context.performed;
    }

    public void OnSink(InputAction.CallbackContext context)
    {
        sink = context.performed;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        sprint = (context.ReadValue<float>() > 0.1f);
    }

    private void OnApplicationFocus(bool hasfocus)
    {
        SetCursorState(cursorLocked);
    }

    void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked :CursorLockMode.None;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            space = Space.Water;
            gravity = 0.0f;
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(0.0f,0.0f,0.0f);
            // CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(CinemachineCameraTarget.transform.localRotation.x - transform.rotation.x,0.0f,0.0f);

            Debug.Log(space);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            space = Space.Ground;
            gravity = -15.0f;
            transform.rotation = Quaternion.Euler(0.0f, _cinemachineTargetPitchY, 0.0f);

            Debug.Log(space);
        }
    }
}