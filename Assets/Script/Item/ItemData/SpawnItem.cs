using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public Items Item;
    protected ItemManager playerInven;
    public Fish2 Fish;

    public Vector2[] itemSpawnPos;
    public Vector2[] fishSpawnPos;
    protected Ray ray;

    protected void Awake()
    {
        playerInven = FindObjectOfType<ItemManager>();
    }

    private void Start()
    {
        SpawnSystem();
    }

    protected void SpawnSystem()
    {
        if (itemSpawnPos != null & Item != null) Spawn();
        if (fishSpawnPos != null & Fish != null) SpawnFish();
    }
    
    protected void Spawn()
    {
        int groundRayer = 1 << LayerMask.NameToLayer("Ground");
        foreach (Vector2 pos in itemSpawnPos)
        {
            Vector3 Pos = new(pos.x, transform.position.y, pos.y);
            ray = new Ray(Pos, -transform.up);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundRayer))
            {
                Items items = GameObject.Instantiate(Item, hit.point, Quaternion.identity, transform.GetChild(0)).GetComponent<Items>();
                items.inven = playerInven;
                items.name = Item.name;
            }
        }
    }

    protected void SpawnFish()
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
