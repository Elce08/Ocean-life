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

    public float rotationSpeed = 1.0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        fishUpdate = Update_Move;
        StartCoroutine(SetHorizontal());
        StartCoroutine(SetVertical());
    }

    private void Start()
    {
    }

    private void Update()
    {
        fishUpdate();
    }

    private void Update_Rest()
    {

    }

    private void Update_Move()
    {
        transform.position += speed * Time.deltaTime * transform.right;
        targetDir = Vector3.RotateTowards(transform.forward, new(0.0f, leftright, updown), rotationSpeed, 0.0f);
        controller.transform.Rotate(targetDir);
    }

    private void Update_Void()
    {

    }

    private void Update_Escape()
    {

    }

    float leftright;
    float updown;
    Vector3 targetDir;

    IEnumerator SetHorizontal()
    {
        while (true)
        {
            leftright = Random.Range(0, 20) switch
            {
                0 or 1 or 2 or 3 or 4 or 5 or 6 or 7 => Random.Range(-1.5f,1.5f),
                _ => transform.forward.y,
            };
            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }

    IEnumerator SetVertical()
    {
        while (true)
        {
            updown = Random.Range(0, 20) switch
            {
                0 => Random.Range(2f, 4f),
                1 => Random.Range(-2f, -4f),
                _ => -transform.rotation.eulerAngles.z,
            };
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }
}
