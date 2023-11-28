using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fish : MonoBehaviour
{
    /// <summary>
    /// 물고기 최대 속도
    /// </summary>
    public float maxSpeed = 2.0f;
    /// <summary>
    /// 물고기 회전 속도
    /// </summary>
    public float maxTurnSpeed = 0.5f;
    /// <summary>
    /// 물고기 현재 속도
    /// </summary>
    private float speed;
    /// <summary>
    /// 이웃한 물고기와의 거리(최소거리?)
    /// </summary>
    private float neighborDistance = 3.0f;
    /// <summary>
    /// 회전중인지 확인
    /// </summary>
    private bool isTurning = false;

    public float maxUpDownDistance = 0.5f; // 위아래 움직일 최대 거리
    public float upDownSpeed = 1.0f; // 위아래 움직일 속도

    Cluster cluster;
    Transform clusterTransfrom;

    private void Awake()
    {
        cluster = FindObjectOfType<Cluster>();
        clusterTransfrom = cluster.transform.GetComponent<Transform>();
    }

    void Start()
    {
        speed = Random.Range(0.5f, maxSpeed);
    }

    void Update()
    {
        GetIsTurning(); // 물고기가 회전해야 하는지 확인 후
        CheckTurn();    // 회전 로직 실행
        FishMove();
    }

    /// <summary>
    /// 회전할지 결정하는 함수
    /// </summary>
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

    /// <summary>
    /// 물고기 회전 함수
    /// </summary>
    void SetRotation()
    {
        GameObject[] fishes;    // 군체 내 모든 물고기를 담은 배열
        fishes = cluster.fishes;

        Vector3 center = Vector3.zero;
        Vector3 avoid = Vector3.zero;
        float speed = 0.1f;

        Vector3 targetPosition = cluster.targetPosition;

        float distance;
        int groupSize = 0;

        // 군체 내 모든 물고기 확인
        for (int i = 0; i < fishes.Length; i++)
        {
            if (fishes[i] != gameObject)
            {
                distance = Vector3.Distance(fishes[i].transform.position, transform.position);

                if (distance <= neighborDistance)
                {
                    // 거리 이내에 다른 물고기가 있는 경우
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
            // 주변에 이웃한 물고기가 있다면
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
    
    /// <summary>
    /// 회전하고 있는지 확인하고 행동하는 함수
    /// </summary>
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

    private void FishMove()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
        transform.LookAt(transform.position + transform.forward);

        float leftRightMovement = Time.deltaTime * speed; // 좌우로 이동하는 양
        float upDownMovement = Mathf.PingPong(Time.time * upDownSpeed, maxUpDownDistance) - (maxUpDownDistance * 0.5f); // 위아래로 이동하는 양

        // 좌우로 이동
        transform.Translate(Vector3.left * leftRightMovement);

        // 현재 위치에 위아래로 이동하는 양을 더합니다.
        transform.Translate(Vector3.up * upDownMovement);
    }

    /// <summary>
    /// 회전 속도 반환해주는 함수
    /// </summary>
    /// <returns></returns>
    float TurnSpeed()
    {
        return Random.Range(0.2f, maxTurnSpeed);
    }
}
