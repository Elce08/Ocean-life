using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBase : MonoBehaviour
{
    PlayerInputAtions inputActions;

    protected virtual void Awake()
    {
        inputActions = new PlayerInputAtions();
        if (inputActions != null) Debug.Log("Have Inputs");
    }

    protected virtual void OnEnable()
    {
        inputActions.Test.Enable();
        inputActions.Test.Test1.performed += Test1;
        inputActions.Test.Test2.performed += Test2;
        inputActions.Test.Test3.performed += Test3;
        inputActions.Test.Test4.performed += Test4;
        inputActions.Test.Test5.performed += Test5;
    }

    protected virtual void OnDisable()
    {
        inputActions.Test.Test1.performed -= Test1;
        inputActions.Test.Test2.performed -= Test2;
        inputActions.Test.Test3.performed -= Test3;
        inputActions.Test.Test4.performed -= Test4;
        inputActions.Test.Test5.performed -= Test5;
        inputActions.Test.Disable();
    }

    protected virtual void Test1(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
    }

    protected virtual void Test2(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
    }

    protected virtual void Test3(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
    }

    protected virtual void Test4(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
    }

    protected virtual void Test5(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
    }
}
