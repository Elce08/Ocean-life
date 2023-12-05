using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssociationFish : MonoBehaviour
{
    Vector3 basicPos;

    private enum State
    {
        Association,
        Escape,
    }

    private State fishState = State.Association;
    private State FishState
    {
        get => fishState;
        set
        {
            fishState = value;
            switch (fishState)
            {
                case State.Association:
                    break;
                case State.Escape:
                    break;
            }
        }
    }

    System.Action act;

    private void Awake()
    {
        basicPos = transform.localPosition;
        speed = Random.Range(0.1f, 0.3f);
        moveSpeed = speed;
    }

    private void Update()
    {
        Update_Association();
    }

    // Association---------

    private float speed;
    private float moveSpeed;

    private void Update_Association()
    {
        transform.position += moveSpeed * Time.deltaTime * transform.forward;
        if (transform.localPosition.z > basicPos.z + 1.0f) moveSpeed = -speed;
        else if (transform.localPosition.z < basicPos.z - 1.0f) moveSpeed = speed;
    }
}
