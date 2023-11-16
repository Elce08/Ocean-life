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
        controller = GetComponent<CharacterController>();
        fishUpdate = Update_Move;
    }

    private void Start()
    {
        StartCoroutine(SetMoveDir());
    }

    private void Update()
    {
        Update_Move();
    }

    private void Update_Rest()
    {

    }

    private void Update_Move()
    {
        transform.position += speed * Time.deltaTime * transform.right;
        move();
    }

    private void Update_Void()
    {

    }

    private void Update_Escape()
    {

    }

    // Random Move----------

    enum Ylook
    {
        Up,
        Down,
        Straight,
    }

    private Ylook Look()
    {
        if (transform.rotation.eulerAngles.z > 0.01f && transform.rotation.eulerAngles.z < 90.0f) return Ylook.Up;
        if (transform.rotation.eulerAngles.z > 90.0f && transform.rotation.eulerAngles.z < 180.0f) return Ylook.Up;
        else if (transform.rotation.eulerAngles.z > 180.0f && transform.rotation.eulerAngles.z <270.0f) return Ylook.Down;
        else if (transform.rotation.eulerAngles.z > 270.0f && transform.rotation.eulerAngles.z <359.99f) return Ylook.Down;
        else return Ylook.Straight;
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
        if (Look() == Ylook.Up) controller.transform.Rotate(-transform.forward * Random.Range(0.8f, 1.5f), UnityEngine.Space.World);
        else if(Look() == Ylook.Down) controller.transform.Rotate(transform.forward * Random.Range(0.8f, 1.5f), UnityEngine.Space.World);
        else
        {
            move = Random.Range(0, 2) switch
            {
                0 => Move_Right,
                _ => Move_Left,
            };
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
}
