using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public GameObject[] Item;
    ItemManager playerInven;
    public GameObject Fish;

    public Vector2[] titaniumSpawnPos;
    public Vector2[] copperSpawnPos;
    public Vector2[] coalSpawnPos;
    public Vector2[] quartzSpawnPos;
    public Vector2[] fishSpawnPos;
    Ray ray;

    private void Awake()
    {
        playerInven = FindObjectOfType<ItemManager>();
    }

    private void Start()
    {
        if(titaniumSpawnPos != null & Item[0] != null) Spawn(Item[0], titaniumSpawnPos);
        if(copperSpawnPos != null & Item[1] != null) Spawn(Item[1], copperSpawnPos);
        if(coalSpawnPos != null & Item[2] != null) Spawn(Item[2], coalSpawnPos);
        if(quartzSpawnPos != null & Item[3] != null)Spawn(Item[3], quartzSpawnPos);
        if (fishSpawnPos != null & Fish != null) SpawnFish();
    }
    
    private void Spawn(GameObject item, Vector2[] spawnPos)
    {
        int groundRayer = 1 << LayerMask.NameToLayer("Ground");
        foreach (Vector2 pos in spawnPos)
        {
            Vector3 Pos = new(pos.x, transform.position.y, pos.y);
            ray = new Ray(Pos, -transform.up);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundRayer))
            {
                Items items = GameObject.Instantiate(item, hit.point, Quaternion.identity, transform.GetChild(0)).GetComponent<Items>();
                items.inven = playerInven;
                items.name = item.name;
            }
        }
    }

    private void SpawnFish()
    {
        int groundRayer = 1 << LayerMask.NameToLayer("Ground");
        foreach(Vector2 pos in fishSpawnPos)
        {
            Vector3 Pos = new(pos.x, transform.position.y,pos.y);
            ray = new Ray (Pos, -transform.up);
            if(Physics.Raycast(ray,out RaycastHit hit, Mathf.Infinity, groundRayer))
            {
                if(hit.point.y < 0)
                {
                    Vector3 spawnPoint = new(pos.x, Random.Range(0, hit.point.y), pos.y);
                    GameObject.Instantiate(Fish, spawnPoint, Quaternion.identity, transform.GetChild(1));
                }
            }
        }
    }
}
