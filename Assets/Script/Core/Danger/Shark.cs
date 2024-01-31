using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shark : MonoBehaviour
{
    public enum State
    {
        Hungry,
        Chase,
        Full
    }

    public State sharkState = State.Hungry;

    public State SharkState
    {
        get => sharkState;
        set
        {
            sharkState = value;
            switch (sharkState)
            {
                case State.Hungry:
                    act = Update_Hungry;
                    break;
                case State.Chase:
                    act = Update_Chase;
                    break;
                case State.Full:
                    act = Update_Full;
                    break;
            }
        }
    }

    System.Action act;

    /// <summary>
    /// 상어 이동제어
    /// </summary>
    public float moveSpeed = 0.0f;
    public float rotationSpeed = 90.0f;

    /// <summary>
    /// 배고픔 수치
    /// </summary>
    public float hungry = 0.0f;
    float timeSpan;
    float checkTime;

    Transform targetObject;
    GameObject target;
    AssociationFish eatFish;
    GameObject closestCollider = null; // 가장 가까운 콜라이더
    UI ui;
    Water water;
    BoxCollider waterCollider;
    float nowY;
    float nowZ;

    private void Awake()
    {
        timeSpan = 0.0f;
        checkTime = 1.0f;
        act = Update_Hungry;
        ui = FindObjectOfType<UI>();
        water = FindObjectOfType<Water>();
        waterCollider = water.GetComponent<BoxCollider>();
    }
    private void Update()
    {
        hungryValue();
        act();
    }

    void hungryValue()
    {
        timeSpan += Time.deltaTime;
        if (timeSpan > checkTime)
        {
            hungry -= 5.0f;
            if (hungry < 0.0f)
            {
                hungry = 0.0f;
            }
            timeSpan = 0.0f;
        }
    }

    private void Update_Hungry()
    {
        moveSpeed = 3.0f;
        SharkMove();
        Collider[] colliders = Physics.OverlapSphere(transform.position, 15.0f);

        float closestDistance = Mathf.Infinity; // 가장 가까운 거리

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                if (Water.inWater)
                {
                    closestCollider = null;
                    target = col.gameObject;
                    targetObject = target.transform;
                    SharkState = State.Chase;
                }
            }
            else if (col.CompareTag("Fish1") || col.CompareTag("Fish2") || col.CompareTag("Fish3") || col.CompareTag("Fish4"))
            {
                // 자기 자신과의 거리 계산
                float distance = Vector3.Distance(transform.position, col.transform.position);

                if (distance < closestDistance) // 가장 가까운 거리의 콜라이더
                {
                    closestDistance = distance;
                    closestCollider = col.gameObject;
                }
            }
        }
        // 가장 가까운 콜라이더가 있다면 그 콜라이더를 target으로 설정
        if (closestCollider != null)
        {
            target = closestCollider;
            targetObject = target.transform;
            SharkState = State.Chase;
        }
    }

    private void Update_Chase()
    {
        moveSpeed = 5.0f;
        ChaseMove();
    }

    private void Update_Full()
    {
        moveSpeed = 1.0f;
        SharkMove();
        checkHungry();
    }

    private void SharkMove()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        Rotation();
    }

    private void ChaseMove()
    {
        if (targetObject != null)
        {
            if (closestCollider != null)
            {
                // target이 물고기라면
                // targetObject와의 벡터 계산
                Vector3 targetDirection = targetObject.position - transform.position;

                // targetDirection의 각도 계산
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                Bounds bounds = waterCollider.bounds;

                Vector3 newPosition = transform.position + transform.forward * moveSpeed * Time.deltaTime;

                // 새 위치가 콜라이더의 경계 안에 있는지 확인하고 조정
                newPosition.x = Mathf.Clamp(newPosition.x, bounds.min.x, bounds.max.x);
                newPosition.z = Mathf.Clamp(newPosition.z, bounds.min.z, bounds.max.z);

                // forward 방향으로 이동
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);


                float distanceToTarget = Vector3.Distance(transform.position, targetObject.position);
                if (distanceToTarget < 1.0f) // 0.1보다 가까워지면(사실상 닿으면 Eat실행)
                {
                    Eat(targetObject.gameObject);
                }
            }
            else
            {
                if(Water.inWater)
                {
                    // target이 Player라면
                    // targetObject와의 벡터 계산
                    Vector3 targetDirection = targetObject.position - transform.position;

                    // targetDirection의 각도 계산
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                    Bounds bounds = waterCollider.bounds;

                    Vector3 newPosition = transform.position + transform.forward * moveSpeed * Time.deltaTime;

                    // 새 위치가 콜라이더의 경계 안에 있는지 확인하고 조정
                    newPosition.x = Mathf.Clamp(newPosition.x, bounds.min.x, bounds.max.x);
                    newPosition.z = Mathf.Clamp(newPosition.z, bounds.min.z, bounds.max.z);

                    // forward 방향으로 이동
                    transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);


                    float distanceToTarget = Vector3.Distance(transform.position, targetObject.position);
                    if (distanceToTarget < 1.0f) // 0.1보다 가까워지면(사실상 닿으면 Eat실행)
                    {
                        AttackPlayer(targetObject.gameObject);
                    }
                    nowY = this.transform.rotation.y;
                    nowZ = transform.rotation.z;
                }
                else
                {
                    checkHungry();
                }
            }
        }
    }

    /// <summary>
    /// 배고픔 수치 체크
    /// </summary>
    private void checkHungry()
    {
        transform.rotation = Quaternion.Euler(0.0f, nowY, nowZ);
        if (hungry <= 0.0f)
        {
            SharkState = State.Hungry;
        }
        else
        {
            SharkState = State.Full;
        }
    }

    private void Rotation()
    {
        Vector3 movementDirection = transform.forward; // 이동 방향 벡터를 얻음
        if (movementDirection != Vector3.zero)  // 보는 방향으로 회전
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1.0f * Time.deltaTime);
        }
    }

    /// <summary>
    /// 물고기를 먹는 함수
    /// </summary>
    /// <param name="target"></param>
    private void Eat(GameObject target)
    {
        if (target.tag == "Fish1" || target.tag == "Fish2"
   || target.tag == "Fish3" || target.tag == "Fish4")
        {
            eatFish = target.GetComponent<AssociationFish>();
            moveSpeed = 1.0f;
            targetObject = null;
            if(eatFish != null)
            {
                eatFish.Die();
            }

            if (target.tag == "Fish1")
            {
                hungry += 30.0f;
            }
            else if (target.tag == "Fish2")
            {
                hungry += 40.0f;
            }
            else if (target.tag == "Fish3")
            {
                hungry += 50.0f;
            }
            else if (target.tag == "Fish4")
            {
                hungry += 50.0f;
            }
            checkHungry();
        }
    }

    private float attackCooldown = 3.0f;
    private float lastAttackTime = -Mathf.Infinity;
    private void AttackPlayer(GameObject target)
    {
        if(target.tag == "Player" && Time.time >= lastAttackTime + attackCooldown)
        {
            ui.Hp -= 10;
            moveSpeed = 1.0f;
            StartCoroutine(ResetSpeedAfterDelay(3.0f));
            checkHungry();
            lastAttackTime = Time.time;
        }
    }

    private IEnumerator ResetSpeedAfterDelay(float delay)
    {
        // 지정된 시간만큼 기다립니다.
        yield return new WaitForSeconds(delay);

        // 속도를 초기 속도로 복구합니다.
        moveSpeed = 3.0f;
    }
}
