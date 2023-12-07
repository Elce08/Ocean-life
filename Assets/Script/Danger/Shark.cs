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
    public float sideSpeed = 0.0f;
    public float amplitude = 0.5f;

    /// <summary>
    /// 배고픔 수치
    /// </summary>
    public float hungry = 0.0f;
    float timeSpan;
    float checkTime;

    SphereCollider bite;
    Transform targetObject;

    private void Awake()
    {
        bite = GetComponent<SphereCollider>();
        timeSpan = 0.0f;
        checkTime = 1.0f;
        act = Update_Hungry;
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
            Debug.Log(hungry);
            Debug.Log(moveSpeed);
        }
    }

    private void Update_Hungry()
    {
        moveSpeed = 3.0f;
        SharkMove();
        Collider[] colliders = Physics.OverlapSphere(transform.position, 15.0f);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Fish1") || col.CompareTag("Fish2") || col.CompareTag("Fish3") || col.CompareTag("Fish4"))
            {
                targetObject = col.gameObject.transform;
                SharkState = State.Chase;
                Debug.Log($"콜라이더 찾음{SharkState}");
            }
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
        checkHungry();
    }

    private void SharkMove()
    {
        // float hInput = Mathf.Sin(Time.time * sideSpeed) * amplitude;

        // Vector3 moveDirection = new Vector3(hInput, 0.0f, 1.0f).normalized;
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void ChaseMove()
    {
        if (targetObject != null)
        {
            Vector3 targetDirection = (targetObject.position - transform.position).normalized;

            transform.Translate(targetDirection * moveSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider == bite)
        {
            if (collision.gameObject.tag == "Fish1" || collision.gameObject.tag == "Fish2"
               || collision.gameObject.tag == "Fish3" || collision.gameObject.tag == "Fish4")
            {
                moveSpeed = 0.1f;
                targetObject = null;
                collision.gameObject.SetActive(false);
                if (collision.gameObject.tag == "Fish1")
                {
                    hungry += 30.0f;
                }
                else if (collision.gameObject.tag == "Fish2")
                {
                    hungry += 40.0f;
                }
                else if (collision.gameObject.tag == "Fish3")
                {
                    hungry += 50.0f;
                }
                else if (collision.gameObject.tag == "Fish4")
                {
                    hungry += 50.0f;
                }
                checkHungry();
            }
        }
    }

    private void checkHungry()
    {
        if (hungry <= 0.0f)
        {
            SharkState = State.Hungry;
        }
        else
        {
            SharkState = State.Full;
        }
    }
}

