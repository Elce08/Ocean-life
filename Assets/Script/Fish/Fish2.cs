using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish2 : MonoBehaviour
{
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
                        StartCoroutine(SetDir());
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
        fishUpdate = Update_Move;
    }

    private void Start()
    {
        StartCoroutine(SetDir());
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
    }

    private void Update_Void()
    {

    }

    private void Update_Escape()
    {

    }

    private Vector3 Dir()
    {
        Vector3 result = Vector3.zero;

        float RandomRotate = Random.Range(20f, 110f);

        switch ((int)Random.Range(0, 8))
        {
            case 0:
                result = new(0,RandomRotate,0);
                break;
            case 1:
                result = new(0, -RandomRotate, 0);
                break;
            case 2:
                result = new(0, 0, RandomRotate);
                break;
            case 3:
                result = new(0, 0, -RandomRotate);
                break;
            case 4:
                result = new(0, RandomRotate, RandomRotate);
                break;
            case 5:
                result = new(0, -RandomRotate, RandomRotate);
                break;
            case 6:
                result = new(0, RandomRotate, -RandomRotate);
                break;
            case 7:
                result = new(0, -RandomRotate, -RandomRotate);
                break;
        }
        Debug.Log(result);

        return result;
    }

    IEnumerator SetDir()
    {
        Vector3 targetRotation = Dir();
        Debug.Log(transform.rotation * Quaternion.Euler(targetRotation));
        while(FishState != State.Move)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,transform.rotation * Quaternion.Euler(targetRotation), rotationSpeed  * Time.deltaTime);
            yield return new WaitForSeconds(rotationSpeed + Random.Range(0, 3));
        }
    }
}
