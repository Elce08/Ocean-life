using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Ray : TestBase
{
    Vector3 Pos;
    RaycastHit hit;
    Ray ray;
    float fromtoRotation;
    Collider[] cols;

    protected override void Awake()
    {
        base.Awake();
        Pos = transform.position;
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        ray = new Ray(Pos, -transform.up * 20.0f);
        if(Physics.Raycast(ray,out hit, 20.0f))
        {
            Vector3 normal = hit.normal;
            Vector3 forward = transform.forward;
            Vector3 right = Vector3.Cross(normal, forward).normalized;
            Vector3 up = Vector3.Cross(right, forward).normalized;

            // 충돌 지점의 오일러 각도 계산
            Quaternion rotation = Quaternion.LookRotation(forward, up);
            Vector3 eulerAngle = rotation.eulerAngles;
            Debug.Log(eulerAngle);
        }
    }
}
