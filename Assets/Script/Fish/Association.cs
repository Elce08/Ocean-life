using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static UnityEngine.InputSystem.InputSettings;

public class Association : MonoBehaviour
{
    public GameObject fish;

    public float moveSpeed = 5.0f;
    public float sphereRadius = 5.0f;
    GameObject[] fishs;
    public int headNum = 3000;

    public float noise = 0.1f;

    public float xMultiple = 1.0f;
    public float yMultiple = 1.0f;
    public float zMultiple = 1.0f;

    public float sightRange = 10.0f;

    private enum State
    {
        Basic,
        Escape,
    }

    private State associationState = State.Basic;

    private State AssociationState
    {
        get => associationState;
        set 
        {
            associationState = value;
            switch (associationState)
            {
                case State.Basic:
                    act = Update_Associate;
                    break;
                case State.Escape:
                    act = Update_Escape;
                    break;
            }
        }
    }

    System.Action act;

    private void Awake()
    {
        enemys = new();
        act = Update_Associate;
        move = Move_Front;
    }

    private void Start()
    {
        fishs = new GameObject[headNum];
        StartCoroutine(SetMoveDir());
        Spawn();
    }

    private void Update()
    {
        EnemyCheck();
        act();
    }

    private void Spawn()
    {
        SetSphere(0, (int)(headNum * 0.4), 1.0f, 0.0f);
        SetSphere((int)(headNum * 0.4),(int)(headNum * 0.7),0.75f, noise);
        SetSphere((int)(headNum * 0.7),(int)(headNum * 0.9),0.5f, noise);
        SetSphere((int)(headNum * 0.9),(headNum),0.25f, noise);
    }

    private void SetSphere(int from, int to, float radiusMultiple, float noise)
    {
        for (int i = from; i < to; i++)
        {
            float theta = Random.Range(0f, Mathf.PI * 2);
            float phi = Random.Range(0f, Mathf.PI);

            float x = (sphereRadius * radiusMultiple * xMultiple * Mathf.Sin(phi) * Mathf.Cos(theta)) + Random.Range(-noise,noise);
            float y = (sphereRadius * radiusMultiple * yMultiple * Mathf.Cos(phi)) + Random.Range(-noise, noise);
            float z = (sphereRadius * radiusMultiple * zMultiple * Mathf.Sin(phi) * Mathf.Sin(theta)) +Random.Range(-noise, noise);

            Vector3 spawnPosition = new(x, y, z);
            fishs[i] = Instantiate(fish, spawnPosition, Quaternion.identity, transform);
            fishs[i].transform.parent = transform;
        }
    }

    // Associate------------

    private void Update_Associate()
    {
        transform.position += moveSpeed * Time.deltaTime * transform.forward;
        move();
    }

    System.Action move;

    IEnumerator SetMoveDir()
    {
        float stateTime = 0.0f;
        while (true)
        {
            stateTime = UnityEngine.Random.Range(3.0f, 5.0f);
            move = UnityEngine.Random.Range(0, 8) switch
            {
                0 or 1 => Move_Left,
                2 or 3 => Move_Right,
                _ => Move_Front,
            };
            yield return new WaitForSeconds(stateTime);
        }
    }

    private void Move_Left()
    {
        transform.Rotate(transform.up * Random.Range(0.0f, 0.2f), UnityEngine.Space.World);
    }

    private void Move_Right()
    {
        transform.Rotate(-transform.up * Random.Range(0.0f, 0.2f), UnityEngine.Space.World);
    }

    private void Move_Front()
    {
    }

    // Escape----------

    Transform target;

    Collider[] enemyTargets;
    List<Collider> enemys;

    private void EnemyCheck()
    {
        enemyTargets = null;
        enemyTargets = Physics.OverlapSphere(transform.position, sightRange, LayerMask.GetMask("Enemy"));
        enemys.Clear();
        if (enemyTargets != null)
        {
            foreach (Collider target in enemyTargets)
            {
                float fromtoRotation;
                if (associationState != State.Escape)
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
        if (enemys.Count > 0)
        {
            foreach (Collider enemy in enemys)
            {
                if (target == null) target = enemy.transform;
                else
                {
                    if (Vector3.Distance(transform.position, target.position) > Vector3.Distance(transform.position, enemy.transform.position)) target = enemy.transform;
                }
            }
            associationState = State.Escape;
            Update_Escape();
        }
        else
        {
            if (associationState == State.Escape)
            {
                associationState = State.Basic;
            }
        }
    }

    Vector3 dir = Vector3.zero;

    private void Update_Escape()
    {
        if (target != null)
        {
            dir = (transform.position - target.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(dir.x,dir.y,0.0f)), Time.deltaTime * 0.2f);
            transform.position += Time.deltaTime * (moveSpeed + 2.0f) * dir.normalized;
        }
        else
        {
            associationState = State.Basic;
        }
    }
}
