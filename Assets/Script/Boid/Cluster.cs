using System.Diagnostics;
using UnityEngine;

public class Cluster : MonoBehaviour
{
    [Header("물고기")]
    public GameObject prefab;
    [Header("물고기 수")]
    public int gen = 10;

    private void Start()
    {
        Generator();
    }

    void ClusterMove()
    {

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