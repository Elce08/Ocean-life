using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Association : MonoBehaviour
{
    public GameObject fish;

    public float moveSpeed = 13.0f;
    public float sphereRadius = 5.0f;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        GameObject[] fishs = new GameObject[3000];
        for (int i = 0; i < 1000; i++)
        {
            float theta = Random.Range(0f, Mathf.PI * 2);
            float phi = Random.Range(0f, Mathf.PI);      

            float x = sphereRadius * Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = sphereRadius * Mathf.Cos(phi);
            float z = sphereRadius * Mathf.Sin(phi) * Mathf.Sin(theta);

            Vector3 spawnPosition = new(x, y, z);
            fishs[i] = Instantiate(fish, spawnPosition, Quaternion.identity, transform);
            fishs[i].transform.parent = transform;
        }
        for (int i = 1000; i < 1800; i++)
        {
            float theta = Random.Range(0f, Mathf.PI * 2);
            float phi = Random.Range(0f, Mathf.PI);      

            float x = sphereRadius * 0.8f * Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = sphereRadius * 0.8f * Mathf.Cos(phi);
            float z = sphereRadius * 0.8f * Mathf.Sin(phi) * Mathf.Sin(theta);

            Vector3 spawnPosition = new(x, y, z);
            fishs[i] = Instantiate(fish, spawnPosition, Quaternion.identity, transform);
            fishs[i].transform.parent = transform;
        }
        for (int i = 1800; i < 2400; i++)
        {
            float theta = Random.Range(0f, Mathf.PI * 2);
            float phi = Random.Range(0f, Mathf.PI);      

            float x = sphereRadius * 0.6f * Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = sphereRadius * 0.6f * Mathf.Cos(phi);
            float z = sphereRadius * 0.6f * Mathf.Sin(phi) * Mathf.Sin(theta);

            Vector3 spawnPosition = new(x, y, z);
            fishs[i] = Instantiate(fish, spawnPosition, Quaternion.identity, transform);
            fishs[i].transform.parent = transform;
        }
        for (int i = 2400; i < 2800; i++)
        {
            float theta = Random.Range(0f, Mathf.PI * 2);
            float phi = Random.Range(0f, Mathf.PI);      

            float x = sphereRadius * 0.4f * Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = sphereRadius * 0.4f * Mathf.Cos(phi);
            float z = sphereRadius * 0.4f * Mathf.Sin(phi) * Mathf.Sin(theta);

            Vector3 spawnPosition = new(x, y, z);
            fishs[i] = Instantiate(fish, spawnPosition, Quaternion.identity, transform);
            fishs[i].transform.parent = transform;
        }
        for (int i = 2800; i <3000; i++)
        {
            float theta = Random.Range(0f, Mathf.PI * 2);
            float phi = Random.Range(0f, Mathf.PI);      

            float x = sphereRadius * 0.2f * Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = sphereRadius * 0.2f * Mathf.Cos(phi);
            float z = sphereRadius * 0.2f * Mathf.Sin(phi) * Mathf.Sin(theta);

            Vector3 spawnPosition = new(x, y, z);
            fishs[i] = Instantiate(fish, spawnPosition, Quaternion.identity, transform);
            fishs[i].transform.parent = transform;
        }
    }
}
