using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Association : MonoBehaviour
{
    public GameObject fish;

    public float moveSpeed = 13.0f;
    public float sphereRadius = 5.0f;
    GameObject[] fishs;
    public int headNum = 3000;

    public float xMultiple = 1.0f;
    public float yMultiple = 1.0f;
    public float zMultiple = 1.0f;

    private void Start()
    {
        fishs = new GameObject[headNum];
        Spawn();
    }

    private void Spawn()
    {
        SetSphere(0,(int)(headNum * 0.4),1.0f);
        SetSphere((int)(headNum * 0.4),(int)(headNum * 0.7),0.75f);
        SetSphere((int)(headNum * 0.7),(int)(headNum * 0.9),0.5f);
        SetSphere((int)(headNum * 0.9),(headNum),0.25f);
    }

    private void SetSphere(int from, int to, float radiusMultiple)
    {
        for (int i = from; i < to; i++)
        {
            float theta = Random.Range(0f, Mathf.PI * 2);
            float phi = Random.Range(0f, Mathf.PI);

            float x = sphereRadius * radiusMultiple * xMultiple * Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = sphereRadius * radiusMultiple * yMultiple * Mathf.Cos(phi);
            float z = sphereRadius * radiusMultiple * zMultiple * Mathf.Sin(phi) * Mathf.Sin(theta);

            Vector3 spawnPosition = new(x, y, z);
            fishs[i] = Instantiate(fish, spawnPosition, Quaternion.identity, transform);
            fishs[i].transform.parent = transform;
        }
    }
}
