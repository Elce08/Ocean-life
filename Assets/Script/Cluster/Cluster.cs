using System.Diagnostics;
using UnityEngine;

public class Cluster : MonoBehaviour
{
    [Header("물고기")]
    public GameObject prefab;
    [Header("물고기 수")]
    public int gen = 10;

    public float clusterMoveSpeed;
    public int boundary = 7;
    public GameObject[] fishes;
    public Vector3 targetPosition = Vector3.zero;

    private void Start()
    {   
        Generator();
    }

    private void Update()
    {
        ClusterMove();
    }

    void ClusterMove()
    {
        if (Random.Range(1, 10000) < 50)
        {
            targetPosition = new Vector3(
                Random.Range(-boundary, boundary),
                Random.Range(-boundary, boundary),
                Random.Range(-boundary, boundary)
            );
        }
    }

    void Generator()
    {
        fishes = new GameObject[gen];

        for (int i = 0; i < gen; i++)
        {
            Vector3 position = new Vector3(
                Random.Range(-boundary, boundary),
                Random.Range(-boundary, boundary),
                Random.Range(-boundary, boundary)
            );

            GameObject fish = (GameObject)Instantiate(prefab, position, Quaternion.identity);

            fish.transform.parent = this.transform;
            fishes[i] = fish;
        }
    }
}