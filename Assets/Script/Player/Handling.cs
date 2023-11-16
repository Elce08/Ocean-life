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
    bool findTag = false;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        raycast();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(rayTransform, transform.forward * rayDistance);
    }

    private void raycast()
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
                Cursor.lockState = CursorLockMode.Locked;
                player.storage.gameObject.SetActive(false);
                player.storage.gameObject.SetActive(false);
                player.inventorys.SetActive(false);
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
                player.work.SetActive(false);
                player.workWindow = false;
            }
        }
    }
}
