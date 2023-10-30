using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Vector2 move;
    Vector2 look;
    public bool jump;
    public bool sprint;
    public bool sink;
    public bool inWater = false;

    CharacterController controller;
    PlayerInput inputActions;
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

    float _cinemachineTargetPitch;

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
        controller = GetComponent<CharacterController>();
        inputActions = GetComponent<PlayerInput>();

        _jumpTimeoutDelta = jumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }

    private void Update()
    {
        GroundedCheck();
        Move();
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
        if (!inWater) currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;
        else currentHorizontalSpeed = new Vector3(controller.velocity.x, controller.velocity.y, controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = move.magnitude;

        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * speedChangeRate);
        }
        else _speed = targetSpeed;

        Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y);

        if(move != Vector2.zero)
        {
            if (!inWater) inputDirection = transform.right * move.x + transform.forward * move.y;
            else inputDirection = transform.right * move.x + transform.forward * move.y;
        }

        controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        grounded = Physics.CheckSphere(spherePosition,groundedRadius,GroundLayer,QueryTriggerInteraction.Ignore);
    }

    void CameraRotation()
    {
        _cinemachineTargetPitch += look.y * rotationSpeed;
        _rotationVelocity = look.x * rotationSpeed;

        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);
        CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
        transform.Rotate(Vector3.up * _rotationVelocity);
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
}