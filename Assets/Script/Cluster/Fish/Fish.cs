using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fish : MonoBehaviour
{
    /// <summary>
    /// ����� �ִ� �ӵ�
    /// </summary>
    public float maxSpeed = 2.0f;
    /// <summary>
    /// ����� ȸ�� �ӵ�
    /// </summary>
    public float maxTurnSpeed = 0.5f;
    /// <summary>
    /// ����� ���� �ӵ�
    /// </summary>
    private float speed;
    /// <summary>
    /// �̿��� �������� �Ÿ�(�ּҰŸ�?)
    /// </summary>
    private float neighborDistance = 3.0f;
    /// <summary>
    /// ȸ�������� Ȯ��
    /// </summary>
    private bool isTurning = false;

    public float maxUpDownDistance = 0.01f; // ���Ʒ� ������ �ִ� �Ÿ�
    public float upDownSpeed = 0.01f; // ���Ʒ� ������ �ӵ�

    Cluster cluster;
    Transform clusterTransfrom;
    Vector3 localOffset;

    private void Awake()
    {
        cluster = FindObjectOfType<Cluster>();
        clusterTransfrom = cluster.transform.GetComponent<Transform>();
    }

    void Start()
    {
        speed = Random.Range(0.5f, maxSpeed);
        localOffset = transform.localPosition;
    }

    void Update()
    {
        GetIsTurning(); // ����Ⱑ ȸ���ؾ� �ϴ��� Ȯ�� ��
        CheckTurn();    // ȸ�� ���� ����
        FishMove();
    }

    /// <summary>
    /// ȸ������ �����ϴ� �Լ�
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
    /// ����� ȸ�� �Լ�
    /// </summary>
    void SetRotation()
    {
        GameObject[] fishes;    // ��ü �� ��� ����⸦ ���� �迭
        fishes = cluster.fishes;

        Vector3 center = Vector3.zero;
        Vector3 avoid = Vector3.zero;
        float speed = 0.1f;

        Vector3 targetPosition = cluster.moveDir;

        float distance;
        int groupSize = 0;

        // ��ü �� ��� ����� Ȯ��
        for (int i = 0; i < fishes.Length; i++)
        {
            if (fishes[i] != gameObject)
            {
                distance = Vector3.Distance(fishes[i].transform.position, transform.position);
                distance = Vector3.Distance(fishes[i].transform.position, transform.position);

                if (distance <= neighborDistance)
                {
                    // �Ÿ� �̳��� �ٸ� ����Ⱑ �ִ� ���
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
            // �ֺ��� �̿��� ����Ⱑ �ִٸ�
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
    /// ȸ���ϰ� �ִ��� Ȯ���ϰ� �ൿ�ϴ� �Լ�
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
        transform.localPosition = localOffset;
        // �θ� ������Ʈ�� �������� ���󰡵�, �θ��� ��ȯ�� �ݿ����� �ʴ� ��ġ ���
        //  Vector3 targetPosition = clusterTransfrom.position + localOffset;
        //  transform.position = targetPosition;

        //  transform.localPosition += Vector3.forward * Time.deltaTime * speed;
        //  transform.LookAt(transform.position + transform.forward);
        //  
        //  float leftRightMovement = Time.deltaTime * speed; // �¿�� �̵��ϴ� ��
        //  float updown = Random.Range(0.1f, 0.5f);
        //  float upDownMovement = Mathf.PingPong(Time.time * upDownSpeed, maxUpDownDistance) - (maxUpDownDistance * updown); // ���Ʒ��� �̵��ϴ� ��
        //  
        //  // �¿�� �̵�
        //  transform.localPosition += Vector3.forward * leftRightMovement;
        //  transform.localPosition += Vector3.up * upDownMovement;
    }

    /// <summary>
    /// ȸ�� �ӵ� ��ȯ���ִ� �Լ�
    /// </summary>
    /// <returns></returns>
    float TurnSpeed()
    {
        return Random.Range(0.2f, maxTurnSpeed);
    }
}
