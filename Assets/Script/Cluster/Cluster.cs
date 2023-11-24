using System.Diagnostics;
using UnityEngine;

public class Cluster : MonoBehaviour
{
    [Header("물고기")]
    public GameObject prefab;
    [Header("물고기 수")]
    public int gen = 10;

    public float clusterMoveSpeed;
    public static int m_Boundary = 7;
    public static GameObject[] m_Fishes;
    public static Vector3 m_TargetPosition = Vector3.zero;

    private void Start()
    {
        Generator();
    }

    void ClusterMove()
    {
        if (Random.Range(1, 10000) < 50)
        {
            m_TargetPosition = new Vector3(
                Random.Range(-m_Boundary, m_Boundary),
                Random.Range(-m_Boundary, m_Boundary),
                Random.Range(-m_Boundary, m_Boundary)
            );
        }
    }

    void Generator()
    {
        for(int i = 0; i < gen; i++)
        {
            GameObject newObject = Instantiate(prefab, transform.position, Quaternion.identity);
            newObject.transform.parent = transform;
        }
    }
}