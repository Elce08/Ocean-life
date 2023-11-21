using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish2 : MonoBehaviour
{
    CharacterController controller;

    enum State
    {
        Rest,
        Move,
        Void,
        Escape,
    }

    State fishState = State.Rest;
    State FishState
    {
        get => fishState;
        set
        {
            if(fishState != value)
            {
                fishState = value;
                switch (fishState)
                {
                    case State.Rest:
                        fishUpdate = Update_Rest;
                        break;
                    case State.Move:
                        fishUpdate = Update_Move;
                        break;
                    case State.Void:
                        fishUpdate = Update_Void;
                        break;
                    case State.Escape: 
                        fishUpdate = Update_Escape;
                        break;
                }
            }
        }
    }

    System.Action fishUpdate;

    public float speed = 3.0f;
    public float sprintSpeed = 5.0f;

    public float rotationSpeed = 10.0f;

    private void Awake()
    {
        enemys = new();
        controller = GetComponent<CharacterController>();
        fishUpdate = Update_Move;
    }

    private void Start()
    {
        StartCoroutine(SetMoveDir());
    }

    private void Update()
    {
        Update_Rest();
    }

    private void Update_Void()
    {

    }

    // Random Move----------

    private void Update_Move()
    {
        transform.position += speed * Time.deltaTime * transform.forward;
        move();
    }

    System.Action move;

    IEnumerator SetMoveDir()
    {
        float stateTime;
        while (true)
        {
            switch (UnityEngine.Random.Range(0, 8))
            {
                case 0:
                    move = Move_Up;
                    stateTime = UnityEngine.Random.Range(1.5f, 2.0f);
                    break;
                case 1:
                    move = Move_Down;
                    stateTime = UnityEngine.Random.Range(1.5f, 2.0f);
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                    move = Move_Left;
                    stateTime = UnityEngine.Random.Range(2.0f, 4.0f);
                    break;
                default:
                    move = Move_Right;
                    stateTime = UnityEngine.Random.Range(2.0f, 4.0f);
                    break;
            }
            yield return new WaitForSeconds(stateTime);
        }
    }

    private void StraightLook()
    {
        if (transform.rotation.eulerAngles.z > 0.5f && transform.rotation.eulerAngles.z < 180.0f) controller.transform.Rotate(-transform.up * Random.Range(0.8f, 1.5f), UnityEngine.Space.World);
        else if(transform.rotation.eulerAngles.z > 180.0f && transform.rotation.eulerAngles.z < 359.5f) controller.transform.Rotate(transform.up * Random.Range(0.8f, 1.5f), UnityEngine.Space.World);
        else
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    move = Move_Left;
                    break;
                case 1:
                    move = Move_Right;
                    break;
                default:
                    break;
            }
        }
    }

    private void Move_Up()
    {
        if (transform.rotation.eulerAngles.z < 90) controller.transform.Rotate(transform.forward * Random.Range(0.8f, 1.5f), UnityEngine.Space.World);
        else move = StraightLook;
    }

    private void Move_Down()
    {

        if (transform.rotation.eulerAngles.z > 270 || transform.rotation.eulerAngles.z <0.01) controller.transform.Rotate(-transform.forward * Random.Range(0.8f, 1.5f), UnityEngine.Space.World);
        else move = StraightLook;
    }

    private void Move_Left()
    {
        if (transform.rotation.eulerAngles.z > 359.5f || transform.rotation.eulerAngles.z < 0.05f) controller.transform.Rotate(transform.up * Random.Range(1.5f, 3.5f), UnityEngine.Space.World);
        else move = StraightLook;
    }

    private void Move_Right()
    {
        if (transform.rotation.eulerAngles.z > 359.5f || transform.rotation.eulerAngles.z < 0.05f) controller.transform.Rotate(-transform.up * Random.Range(1.5f, 3.5f), UnityEngine.Space.World);
        else move = StraightLook;
    }

    // Rest----------

    public float restMaxTime;

    int nextRestMove;

    float restTime;

    private void Update_Rest()
    {
        if(restTime < restMaxTime - 0.1f)
        {
            restTime += Time.deltaTime;
        }
        else if(restTime < restMaxTime)
        {
            switch (nextRestMove)
            {
                case 0:
                    controller.transform.LookAt(transform.forward);
                    break;
                case 1:
                    controller.transform.LookAt(-transform.forward);
                    break;
                default:
                    transform.position += Time.deltaTime * 20.0f * transform.forward;
                    break;
            }
            restTime += Time.deltaTime;
        }
        else
        {
            restMaxTime = Random.Range(3.0f, 4.0f);
            nextRestMove = (int)Random.Range(0, 4);
            restTime = 0.0f;
        }
    }

    // escape----------

    Transform target;

    public float sightRange = 10.0f;

    public float Hp = 100.0f;
    float hp = 100.0f;

    Vector3 dir = Vector3.zero;

    bool isSprint = false;

    private void Update_Escape()
    {
        if(target!= null)
        {
            dir = transform.position - target.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotationSpeed);
            // dir바라보게 회전
            if (hp < 0 || (!isSprint && (hp < Hp * 0.5f)))
            {
                transform.position += Time.deltaTime * speed * dir.normalized;
                isSprint = false;
            }
            else
            {
                transform.position += Time.deltaTime * sprintSpeed * dir.normalized;
                isSprint = true;
            }
        }
        else
        {
            fishUpdate = Update_Move;
            isSprint= false;
        }
    }

    Collider[] targets;
    List<Collider> enemys;

    private void EnemyCheck()
    {
        targets = Physics.OverlapSphere(transform.position, sightRange,LayerMask.GetMask("Enemy"));
        enemys.Clear();
        if(targets != null)
        {
            foreach (Collider target in targets)
            {
                float fromtoRotation;
                if (fishState != State.Escape)
                {
                    fromtoRotation = Quaternion.FromToRotation(transform.right, target.transform.position - transform.position).eulerAngles.z;
                    if (fromtoRotation < 120.0f || fromtoRotation > 240.0f) enemys.Add(target);
                }
                else
                {
                    enemys.Add(target);
                }
            }
        }
        if(enemys.Count > 0)
        {
            foreach(Collider enemy in enemys)
            {
                if (target == null) target = enemy.transform;
                else
                {
                    if (Vector3.Distance(transform.position, target.position) > Vector3.Distance(transform.position, enemy.transform.position)) target = enemy.transform;
                }
            }
            fishState= State.Escape;
            Update_Escape();
        }
        else
        {
            if(fishState == State.Escape)
            {
                fishState = State.Move;
            }
        }
    }
}
