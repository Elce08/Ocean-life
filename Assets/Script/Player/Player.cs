using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Vector2 move;
    Vector2 look;

    public GameObject handleing;
    public GameObject inventorys;
    public ItemManager inven;
    public ItemManager storage;
    public GameObject work;
    GameObject equip;
    Handling handle;

    public bool jump;
    public bool sprint;
    public bool sink;
    public bool interaction;

    

    public enum Space
    {
        Ground,
        Water,
    }

    public Space floor;

    public Space Floor
    {
        get => floor;
        set
        {
            if(floor != value)
            {
                floor = value;
                switch (value)
                {
                    case Space.Ground:
                        gravity = -15.0f;
                        spaceCheck = GroundAct;
                        break;
                    case Space.Water:
                        gravity = 0.0f;
                        spaceCheck = WaterAct;
                        break;
                }
            }
        }
    }

    System.Action spaceCheck;

    CharacterController controller;
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
    readonly float _terminalVelocity = 53.0f;

    float _jumpTimeoutDelta;
    float _fallTimeoutDelta;

    public float threshold = 0.01f;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        handle = child.transform.GetComponent<Handling>();
        inventorys = GameObject.FindGameObjectWithTag("Inven");
        inven = inventorys.GetComponentInChildren<ItemManager>();
        equip = inventorys.transform.GetChild(1).gameObject;
        if(mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        spaceCheck = GroundAct;
    }

    private void Start()
    {
        equip.SetActive(false);
        inven.gameObject.SetActive(false);
        inventorys.SetActive(false);

        controller = GetComponentInChildren<CharacterController>();

        _jumpTimeoutDelta = jumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }

    private void Update()
    {
        spaceCheck();
    }

    private void GroundAct()
    {
        GroundMove();
        GroundedCheck();
        GroundCameraRotation();
        GroundJumpAndGravity();
    }

    private void WaterAct()
    {
        WaterMove();
        WaterCameraRotation();
        WaterUpAndDown();
    }

    // Ground==========

    private void GroundMove()
    {
        float targetSpeed = sprint ? sprintSpeed : moveSpeed;

        if (move == Vector2.zero) targetSpeed = 0.0f;
        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

        float speedOffset = 0.1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * speedChangeRate);
        }
        else _speed = targetSpeed;

        Vector3 inputDirection = new(move.x, 0.0f, move.y);

        if (move != Vector2.zero) inputDirection = transform.right * move.x + transform.forward * move.y;

        controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }

    private void GroundJumpAndGravity()
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
        }
        if (_verticalVelocity < _terminalVelocity) _verticalVelocity += gravity * Time.deltaTime;
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        grounded = Physics.CheckSphere(spherePosition, groundedRadius, GroundLayer, QueryTriggerInteraction.Ignore);
    }

    // Water==========

    private void GroundCameraRotation()
    {
        _cinemachineTargetPitchX += look.y * rotationSpeed;
        _cinemachineTargetPitchY += look.x * rotationSpeed;
        _rotationVelocity = look.x * rotationSpeed;


        _cinemachineTargetPitchX = ClampAngle(_cinemachineTargetPitchX, bottomClamp, topClamp);
        CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitchX, 0.0f, 0.0f);
        transform.Rotate(Vector3.up * _rotationVelocity, UnityEngine.Space.World);
    }

    private void WaterMove()
    {
        float targetSpeed = moveSpeed;

        if (move == Vector2.zero) targetSpeed = 0.0f;
        float currentHorizontalSpeed = new Vector3(controller.velocity.x, controller.velocity.y, controller.velocity.z).magnitude;
        if (sink || jump) currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

        float speedOffset = 0.1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * speedChangeRate);
        }
        else _speed = targetSpeed;

        Vector3 inputDirection = new(move.x, 0.0f, move.y);

        if(move != Vector2.zero) inputDirection = transform.right * move.x + transform.forward * move.y;

        controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }

    private void WaterUpAndDown()
    {
        if (jump) _verticalVelocity = moveSpeed;
        else if (sink) _verticalVelocity = -moveSpeed;
        else _verticalVelocity = 0.0f;
    }

    private void WaterCameraRotation()
    {
        _cinemachineTargetPitchX += look.y * rotationSpeed;
        _cinemachineTargetPitchY += look.x * rotationSpeed;
        _rotationVelocity = look.x * rotationSpeed;

        _cinemachineTargetPitchX = ClampAngle(_cinemachineTargetPitchX, bottomClamp, topClamp);

        transform.rotation = Quaternion.Euler(_cinemachineTargetPitchX, _cinemachineTargetPitchY, 0.0f);
        transform.Rotate(Vector3.up * _rotationVelocity, UnityEngine.Space.World);
    }

    // WaterTriggerCheck==========

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Floor = Space.Water;
            // Slowly Change
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Floor = Space.Ground;
            // Slowly recover
            transform.rotation = Quaternion.Euler(0.0f, _cinemachineTargetPitchY, 0.0f);
        }
    }

    // inven ==========

    private bool inventory = false;
    public bool storageWindow = false;
    public bool workWindow = false;

    private void Inventory()
    {
        if (!inventory)
        {
            Cursor.lockState = CursorLockMode.Locked;
            if (work != null)work.SetActive(false);
            if(storage != null)storage.gameObject.SetActive(false);
            equip.SetActive(false);
            inven.another = null;
            inven.gameObject.SetActive(false);
            inventorys.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            inventorys.SetActive(true);
            inven.gameObject.SetActive(true);
            equip.SetActive(true) ;
        }
    }

    // input==========

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (cursorInputForLook) look = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed) jump = true;
        else if(context.canceled) jump = false;
        else jump = false;
    }

    public void OnSink(InputAction.CallbackContext context)
    {
        sink = context.performed;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        sprint = (context.ReadValue<float>() > 0.1f);
    }

    public void Interaction(InputAction.CallbackContext context)
    {
        interaction = context.performed;
        if(interaction)
        {
            if (handle.rayHit != null)
            {
                handleing = handle.rayHit;
                if(handleing.CompareTag("ObjectWork"))
                {
                    Cursor.lockState = CursorLockMode.None;
                    work = handle.rayHit;
                    inventorys.SetActive(true);
                    inven.gameObject.SetActive(true);
                    work.SetActive(true);
                    workWindow = true;
                    inventory = true;
                }
                else if(handleing.CompareTag("ObjectStorage"))
                {
                    Cursor.lockState = CursorLockMode.None;
                    storage = handle.rayHit.GetComponent<UIManager>().targetInven;
                    inventorys.SetActive(true);
                    inven.gameObject.SetActive(true);
                    storage.gameObject.SetActive(true);
                    inven.another = storage;
                    storage.another = inven;
                    storageWindow = true;
                    inventory = true;
                }
            }
        }
    }

    public void Tab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!inventory) inventory = true;
            else inventory = false;
            Inventory();
        }
    }

    public void Escape(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inventory = false;
            Inventory();
            storageWindow = false;
            workWindow = false;
        }
    }

    // MouseLock==========

    private void OnApplicationFocus(bool hasfocus)
    {
        SetCursorState(cursorLocked);
    }

    void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

    /// <summary>
    /// Clamp angle by lfMin and lfMax
    /// </summary>
    /// <param name="lfAngle">Clamp angle target</param>
    /// <param name="lfMin">Min angle</param>
    /// <param name="lfMax">Max angle</param>
    /// <returns>If target angle over or under Min or Max angle, return Min or Max angle</returns>
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
