using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public GameObject[] Item;

    public Vector2[] titaniumSpawnPos;
    public Vector2[] copperSpawnPos;
    public Vector2[] coalSpawnPos;
    public Vector2[] quartzSpawnPos;
    public int groundRayer = 6;
    Ray ray;

    private void Start()
    {
        Spawn(Item[0], titaniumSpawnPos);
        Spawn(Item[1], copperSpawnPos);
        Spawn(Item[2], coalSpawnPos);
        Spawn(Item[3], quartzSpawnPos);
    }
    
    private void Spawn(GameObject item, Vector2[] spawnPos)
    {
        foreach (Vector2 pos in spawnPos)
        {
            Vector3 Pos = new(pos.x, transform.position.y, pos.y);
            ray = new Ray(Pos, -transform.up);
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, groundRayer))
            {
                GameObject.Instantiate(item, hit.point, Quaternion.identity);
            }
        }
    }
}
