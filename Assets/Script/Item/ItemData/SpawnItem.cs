using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public GameObject[] Item;
    ItemManager playerInven;

    public Vector2[] titaniumSpawnPos;
    public Vector2[] copperSpawnPos;
    public Vector2[] coalSpawnPos;
    public Vector2[] quartzSpawnPos;
    Ray ray;

    private void Awake()
    {
        playerInven = FindObjectOfType<ItemManager>();
    }

    private void Start()
    {
        if(titaniumSpawnPos != null) Spawn(Item[0], titaniumSpawnPos);
        if(copperSpawnPos != null)Spawn(Item[1], copperSpawnPos);
        if(coalSpawnPos != null)Spawn(Item[2], coalSpawnPos);
        if(quartzSpawnPos != null)Spawn(Item[3], quartzSpawnPos);
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
                Items items = GameObject.Instantiate(item, hit.point, Quaternion.identity, transform).GetComponent<Items>();
                items.inven = playerInven;
                items.name = item.name;
            }
        }
    }
}
