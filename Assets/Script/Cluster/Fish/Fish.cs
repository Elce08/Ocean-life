using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float maxSpeed = 2.0f;
    public float maxTurnSpeed = 0.5f;
    private float speed;
    private float neighborDistance = 3.0f;
    private bool isTurning = false;
    Cluster cluster;

    private void Awake()
    {
        cluster = GetComponentInParent<Cluster>();
    }

    void Start()
    {
        speed = Random.Range(0.5f, maxSpeed);
    }

    void Update()
    {
        GetIsTurning();
        CheckTurn();
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void GetIsTurning()
    {
        if (Vector3.Distance(transform.position, Vector3.zero) >= cluster.boundary)
        {
            isTurning = true;
        }

        else
        {
            isTurning = false;
        }
    }

    void SetRotation()
    {
        GameObject[] fishes;
        fishes = cluster.fishes;

        Vector3 center = Vector3.zero;
        Vector3 avoid = Vector3.zero;
        float speed = 0.1f;

        Vector3 targetPosition = cluster.targetPosition;

        float distance;
        int groupSize = 0;

        for (int i = 0; i < fishes.Length; i++)
        {
            if (fishes[i] != gameObject)
            {
                distance = Vector3.Distance(fishes[i].transform.position, transform.position);

                if (distance <= neighborDistance)
                {
                    center += fishes[i].transform.position;
                    groupSize++;

                    if (distance < 0.75f)
                    {
                        avoid += (transform.position - fishes[i].transform.position);
                    }

                    Fish anotherFish = fishes[i].GetComponent<Fish>();
                    speed += anotherFish.speed;
                }
            }
        }

        if (groupSize > 0)
        {
            center = center / groupSize + (targetPosition - transform.position);
            this.speed = speed / groupSize;

            Vector3 direction = (center + avoid) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction),
                    TurnSpeed() * Time.deltaTime);
            }
        }
    }
    
    private void CheckTurn()
    {
        if (isTurning)
        {
            Vector3 direction = Vector3.zero - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(direction),
            TurnSpeed() * Time.deltaTime);
            speed = Random.Range(0.5f, maxSpeed);
        }

        else
        {
            if (Random.Range(0, 5) < 1)
                SetRotation();
        }
    }

    float TurnSpeed()
    {
        return Random.Range(0.2f, maxTurnSpeed);
    }
}
