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
    /// ��� �̵�����
    /// </summary>
    public float moveSpeed = 0.0f;
    public float rotationSpeed = 90.0f;

    /// <summary>
    /// ����� ��ġ
    /// </summary>
    public float hungry = 0.0f;
    float timeSpan;
    float checkTime;

    Transform targetObject;
    GameObject target;
    AssociationFish eatFish;
    GameObject closestCollider = null; // ���� ����� �ݶ��̴�
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

        float closestDistance = Mathf.Infinity; // ���� ����� �Ÿ�

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
                // �ڱ� �ڽŰ��� �Ÿ� ���
                float distance = Vector3.Distance(transform.position, col.transform.position);

                if (distance < closestDistance) // ���� ����� �Ÿ��� �ݶ��̴�
                {
                    closestDistance = distance;
                    closestCollider = col.gameObject;
                }
            }
        }
        // ���� ����� �ݶ��̴��� �ִٸ� �� �ݶ��̴��� target���� ����
        if (closestCollider != null)
        {
            target = closestCollider;
            targetObject = target.transform;
            SharkState = State.Chase;
        }
    }

    /// <summary>
    /// ����� ���¿��� ���̸� �߰������� ��������
    /// </summary>
    private void Update_Chase()
    {
        moveSpeed = 5.0f;
        ChaseMove();
    }

    /// <summary>
    /// ������� ���� ���
    /// </summary>
    private void Update_Full()
    {
        moveSpeed = 1.0f;
        SharkMove();
        checkHungry();
    }

    /// <summary>
    /// ����� �̵� �Լ�(�����ϰ� ����)
    /// </summary>
    private void SharkMove()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        Rotation();
    }

    /// <summary>
    /// ����� ���¿��� ���̸� �߰������� �����ϴ� ����
    /// </summary>
    private void ChaseMove()
    {
        if (targetObject != null)
        {
            if (closestCollider != null)
            {
                // target�� �������
                // targetObject���� ���� ���
                Vector3 targetDirection = targetObject.position - transform.position;

                // targetDirection�� ���� ���
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // ���ӿ����� �̵�
                Bounds bounds = waterCollider.bounds;

                Vector3 newPosition = transform.position + transform.forward * moveSpeed * Time.deltaTime;

                // �� ��ġ�� �ݶ��̴��� ��� �ȿ� �ִ��� Ȯ���ϰ� ����
                newPosition.x = Mathf.Clamp(newPosition.x, bounds.min.x, bounds.max.x);
                newPosition.z = Mathf.Clamp(newPosition.z, bounds.min.z, bounds.max.z);

                // forward �������� �̵�
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);


                float distanceToTarget = Vector3.Distance(transform.position, targetObject.position);
                if (distanceToTarget < 1.0f) // 0.1���� ���������(��ǻ� ������ Eat����)
                {
                    Eat(targetObject.gameObject);
                }
            }
            else
            {
                if(Water.inWater)
                {
                    // target�� Player���
                    // targetObject���� ���� ���
                    Vector3 targetDirection = targetObject.position - transform.position;

                    // targetDirection�� ���� ���
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                    Bounds bounds = waterCollider.bounds;

                    Vector3 newPosition = transform.position + transform.forward * moveSpeed * Time.deltaTime;

                    // �� ��ġ�� �ݶ��̴��� ��� �ȿ� �ִ��� Ȯ���ϰ� ����
                    newPosition.x = Mathf.Clamp(newPosition.x, bounds.min.x, bounds.max.x);
                    newPosition.z = Mathf.Clamp(newPosition.z, bounds.min.z, bounds.max.z);

                    // forward �������� �̵�
                    transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);


                    float distanceToTarget = Vector3.Distance(transform.position, targetObject.position);
                    if (distanceToTarget < 1.0f) // 0.1���� ���������(��ǻ� ������ Eat����)
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
    /// ����� ��ġ üũ
    /// </summary>
    private void checkHungry()
    {
        transform.rotation = Quaternion.Euler(0.0f, nowY, nowZ);
        //State ����
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
        Vector3 movementDirection = transform.forward; // �̵� ���� ���͸� ����
        if (movementDirection != Vector3.zero)  // ���� �������� ȸ��
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1.0f * Time.deltaTime);
        }
    }

    /// <summary>
    /// ����⸦ �Դ� �Լ�
    /// </summary>
    /// <param name="target"></param>
    private void Eat(GameObject target)
    {
        if (target.tag == "Fish1" || target.tag == "Fish2"
   || target.tag == "Fish3" || target.tag == "Fish4")
        {
            // target�� �±װ� ������϶� ����
            eatFish = target.GetComponent<AssociationFish>();   // ����� Ȯ��
            moveSpeed = 1.0f;   // �ӵ� ����
            targetObject = null;    // ������� �ʱ�ȭ
            if(eatFish != null)
            {
                eatFish.Die();  // ����� ����
            }

            // ����� ��ġ ȸ��
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
            checkHungry();  // ����� Ȯ��
        }
    }

    private float attackCooldown = 3.0f;    // ����� �÷��̾� ���� ��Ÿ��
    private float lastAttackTime = -Mathf.Infinity;

    /// <summary>
    /// �÷��̾ �����ϴ� �Լ�
    /// </summary>
    /// <param name="target"></param>
    private void AttackPlayer(GameObject target)
    {
        if(target.tag == "Player" && Time.time >= lastAttackTime + attackCooldown)
        {
            ui.Hp -= 10;    // �÷��̾� hp ����
            moveSpeed = 1.0f;   // ��� �̵��ӵ� ����
            StartCoroutine(ResetSpeedAfterDelay(3.0f)); // ��Ÿ�ӵ��� ���
            checkHungry();  // ����� Ȯ��
            lastAttackTime = Time.time; // �ð� �ʱ�ȭ
        }
    }

    /// <summary>
    /// �����ϰ� ��� ��Ÿ�� ���
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator ResetSpeedAfterDelay(float delay)
    {
        // ������ �ð���ŭ ��ٸ��ϴ�.
        yield return new WaitForSeconds(delay);

        // �ӵ��� �ʱ� �ӵ��� �����մϴ�.
        moveSpeed = 3.0f;
    }
}
