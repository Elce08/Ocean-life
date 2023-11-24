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

    enum CurrentDir
    {
        Up,Down, Left, Right, Front,
    }

    private CurrentDir currentDir = CurrentDir.Front;

    System.Action move;

    IEnumerator SetMoveDir()
    {
        float stateTime = 0.0f;
        while (true)
        {
            stateTime = UnityEngine.Random.Range(0.5f, 1.0f);
            switch (UnityEngine.Random.Range(0, 10))
            {
                case 0:
                    move = Move_Up;
                    currentDir = CurrentDir.Up;
                    break;
                case 1:
                    move = Move_Down;
                    currentDir = CurrentDir.Down;
                    break;
                case 2:
                case 3:
                    move = Move_Left;
                    currentDir = CurrentDir.Left;
                    break;
                case 4:
                case 5:
                    move = Move_Right;
                    currentDir = CurrentDir.Right;
                    break;
                default:
                    move = Move_Front;
                    currentDir = CurrentDir.Front;
                    break;
            }
            yield return new WaitForSeconds(stateTime);
        }
    }

    private void StraightLook()
    {
        if (transform.rotation.eulerAngles.x > 0.05f && transform.rotation.eulerAngles.x < 180.0f) controller.transform.Rotate(-transform.right * Random.Range(0.1f, 0.5f), UnityEngine.Space.World);
        else if (transform.rotation.eulerAngles.x > 180.0f && transform.rotation.eulerAngles.x < 359.95f) controller.transform.Rotate(transform.right * Random.Range(0.1f, 0.5f), UnityEngine.Space.World);
        else
        {
            switch (currentDir)
            {
                case CurrentDir.Left:
                    move = Move_Left;
                    break;
                case CurrentDir.Right:
                    move = Move_Right;
                    break;
                default:
                    move = Move_Front;
                    break;
            }
        }
    }

    private void Move_Up()
    {
        Debug.Log(transform.rotation.eulerAngles.x);
        if (transform.rotation.eulerAngles.x < 270.5f && transform.rotation.eulerAngles.x >0.1f) move = StraightLook;
        else controller.transform.Rotate(-transform.right * Random.Range(0.1f, 0.5f), UnityEngine.Space.World);
    }

    private void Move_Down()
    {
        if (transform.rotation.eulerAngles.x > 89.5f && transform.rotation.eulerAngles.x < 180.0f) move = StraightLook;
        else controller.transform.Rotate(transform.right * Random.Range(0.1f, 0.5f), UnityEngine.Space.World);
    }

    private void Move_Left()
    {
        if (transform.rotation.eulerAngles.x > 359.5f || transform.rotation.eulerAngles.x < 0.05f) controller.transform.Rotate(transform.up * Random.Range(0.0f, 1.0f), UnityEngine.Space.World);
        else move = StraightLook;
    }

    private void Move_Right()
    {
        if (transform.rotation.eulerAngles.x > 359.5f || transform.rotation.eulerAngles.x < 0.05f) controller.transform.Rotate(-transform.up * Random.Range(0.0f, 1.0f), UnityEngine.Space.World);
        else move = StraightLook;
    }

    private void Move_Front()
    {
        if (transform.rotation.eulerAngles.x < 359.5f && transform.rotation.eulerAngles.x > 0.05f) move = StraightLook;
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
                    controller.transform.Rotate(transform.up,Space.World);
                    break;
                case 1:
                    controller.transform.Rotate(-transform.up, Space.World);
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
