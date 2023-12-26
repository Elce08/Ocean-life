using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AssociationFish : MonoBehaviour, IFishInter
{
    public Vector3 basicPos;

    public float sightRange = 2.0f;

    public bool inWater = true;
    public bool warning = false;

    public float fishSprintSpeed;
    public CapsuleCollider fishCollider;

    public enum State
    {
        Association,
        Escape,
        GetBack,
    }

    public State fishState = State.Association;

    private State FishState
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
        speed = Random.Range(0.1f, 0.3f);
        moveSpeed = speed;
        fishCollider = GetComponent<CapsuleCollider>();
        act = Update_Association;
        water = Update_NormalWater;
    }

    private void Update()
    {
        if (warning) EnemyCheck();
        act();
        water();
    }

    // Association---------

    private float speed;
    private float moveSpeed;

    private void Update_Association()
    {
        transform.localPosition = transform.localPosition + moveSpeed * Time.deltaTime * new Vector3(0,0,1.0f);
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
            transform.localPosition += Time.deltaTime * fishSprintSpeed * dir.normalized;
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

    // Gravity--------

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water")) WaterState = Water.Out;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water")) WaterState = Water.In;
    }

    Vector3 gravity = new(0.0f, 10.0f, 0.0f);

    public enum Water
    {
        Normal,
        In,
        Out,
    }

    public Water waterState = Water.Normal;
    private Water WaterState
    {
        get => waterState;
        set
        {
            if(value != waterState)
            {
                waterState = value;
                switch (waterState)
                {
                    case Water.Normal:
                        water = Update_NormalWater;
                        break;
                    case Water.In:
                        water = Update_InWater;
                        break;
                    case Water.Out:
                        water = Update_OutWater;
                        break;

                }
            }
        }
    }

    System.Action water;

    private void Update_NormalWater()
    {
    }

    private void Update_InWater()
    {
        if (transform.localPosition.y < basicPos.y - 0.01f) transform.localPosition += Time.deltaTime * gravity;
        else if(transform.localPosition.y > basicPos.y + 0.01f) transform.localPosition -= Time.deltaTime * gravity;
        else WaterState = Water.Normal;
    }

    private void Update_OutWater()
    {
        transform.position -= Time.deltaTime * gravity;
    }

    //----------

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
