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

    List<Vector3> dirs;

    private void Awake()
    {
        AddDirs();
        fishUpdate = Update_Rest;
    }

    private void Update_Rest()
    {

    }

    private void Update_Move()
    {

    }

    private void Update_Void()
    {

    }

    private void Update_Escape()
    {

    }

    void AddDirs()
    {
        dirs = new()
        {
            new(0f, 0f, 0f),
            new(1f, 0f, 0f),
            new(-1f, 0f, 0f),
            new(0f, 1f, 0f),
            new(0f, -1f, 0f),
            new(0f, 0f, 1f),
            new(0f, 0f, -1f),
            new(1f, 1f, 0f),
            new(-1f, 1f, 0f),
            new(1f, -1f, 0f),
            new(-1f, -1f, 0f),
            new(-1f, 0f, 1f),
            new(1f, 0f, -1f),
            new(-1f, 0f, -1f),
            new(0f, 1f, 1f),
            new(0f, -1f, 1f),
            new(0f, 1f, -1f),
            new(0f, -1f, -1f),
            new(1f, 1f, 1f),
            new(-1f, 1f, 1f),
            new(1f, -1f, 1f),
            new(1f, 1f, -1f),
            new(-1f, -1f, 1f),
            new(-1f, 1f, -1f),
            new(1f, -1f, -1f),
            new(-1f, -1f, -1f)
        };
    }
}
