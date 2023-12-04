using System.Diagnostics;
using UnityEngine;

public class Cluster : MonoBehaviour
{
    [Header("물고기")]
    public GameObject prefab;
    [Header("물고기 수")]
    public int gen = 10;

    public float clusterMoveSpeed = 3.0f;
    public float clusterRoateSpeed = 0.1f;
    public int boundary = 7;
    public GameObject[] fishes;
    public Vector3 moveDir = Vector3.zero;

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
        float roty = Random.value;
        transform.Translate(Vector3.forward * Time.deltaTime);
        transform.Rotate(Vector3.up * Time.deltaTime * clusterRoateSpeed * roty);
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