using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssociationFish : MonoBehaviour
{
    public Vector3 basicPos;

    public float sightRange = 2.0f;

    public enum State
    {
        Association,
        Escape,
        GetBack,
    }

    public State fishState = State.Association;

    public State FishState
    {
        get => fishState;
        set
        {
            fishState = value;
            switch (fishState)
            {
                case State.Association:
                    act = Update_Association;
                    break;
                case State.Escape:
                    act = Update_Escape;
                    break;
                case State.GetBack:
                    act = Update_GetBack;
                    break;
            }
        }
    }

    System.Action act;

    private void Awake()
    {
        enemys = new();
        basicPos = transform.localPosition;
        speed = Random.Range(0.1f, 0.3f);
        moveSpeed = speed;
        act = Update_Association;
    }

    private void Update()
    {
        act();
        EnemyCheck();
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
            foreach (Collider enemy in enemyTargets)
            {
                float fromtoRotation;
                fromtoRotation = Quaternion.FromToRotation(transform.right, enemy.transform.position - transform.position).eulerAngles.z;
                if (fromtoRotation < 120.0f || fromtoRotation > 240.0f) enemys.Add(enemy);
            }
        }
        ChangeAct();
    }

    private void ChangeAct()
    {
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
            FishState = State.Escape;
        }
        else if(enemys.Count == 0)
        {
            if (FishState == State.Escape)
            {
                FishState = State.GetBack;
            }
        }
    }

    Vector3 dir = Vector3.zero;

    private void Update_Escape()
    {
        if (target != null)
        {
            dir = (transform.position - target.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 0.2f);
            transform.position += Time.deltaTime * (3.0f) * dir.normalized;
        }
        else
        {
            FishState = State.Association;
        }
    }

    // GetBack----------

    private void Update_GetBack()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, basicPos, Time.deltaTime);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, transform.parent.rotation, 10.0f * Time.deltaTime);
        if (transform.localPosition == basicPos && transform.localRotation == transform.parent.rotation) FishState = State.Association;
    }
}
