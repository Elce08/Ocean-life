using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handling : MonoBehaviour
{
    public Ray ray;
    RaycastHit hit;
    public float rayDistance = 2.5f;
    public GameObject rayHit;
    Vector3 rayTransform;
    Player player;
    Collider[] colls;
    Crafting craft;
    bool findTag = false;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        craft = FindObjectOfType<Crafting>();
    }

    private void Update()
    {
        Raycast();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(rayTransform, transform.forward * rayDistance);
    }

    private void Raycast()
    {
        rayTransform = new Vector3(transform.position.x, transform.position.y + 0.35f, transform.position.z);
        ray = new Ray(rayTransform, transform.forward * rayDistance);
        colls = Physics.OverlapSphere(transform.position, rayDistance + 0.5f);
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Raycast"))
            {
                rayHit = hit.collider.gameObject;
            }
        }
        else
        {
            rayHit = null;
        }
        if (player.storageWindow)
        {
            foreach (Collider col in colls)
            {
                if (col.CompareTag("ObjectStorage"))
                {
                    findTag = true;
                    break;
                }
                else
                {
                    findTag = false;
                }
            }
            if (!findTag)
            {
                player.InvenState = Player.Inven.Close;
                player.storageWindow = false;
            }
        }
        else if (player.workWindow)
        {
            foreach (Collider col in colls)
            {
                if (col.CompareTag("ObjectWork"))
                {
                    findTag = true;
                    break;
                }
                else
                {
                    findTag = false;
                }
            }
            if (!findTag)
            {
                player.InvenState = Player.Inven.Close;
                player.workWindow = false;
            }
        }
    }
}
