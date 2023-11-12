using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handling : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    public float rayDistance = 2.5f;
    public GameObject rayHit;
    Vector3 rayTransform;
    Player player;
    SphereCollider distance;
    Collider[] colls;

    private void Awake()
    {
        distance = GetComponent<SphereCollider>();
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        rayTransform = new Vector3(transform.position.x, transform.position.y + 0.35f, transform.position.z);
        ray = new Ray(rayTransform, transform.forward * rayDistance);
        //      colls = Physics.OverlapSphere(transform.position, rayDistance);
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
        //      if(player.storageWindow)
        //      {
        //          for (int i = 0; i < colls.Length; i++)
        //          {
        //              if(colls[i].gameObject.tag == "ObjectStorage")
        //              {
        //      
        //              }
        //              else
        //              {
        //                  player.storage.SetActive(false);
        //                  player.storageWindow = false;
        //              }
        //          }
        //      }
        //      else if(player.workWindow)
        //      {
        //          for (int i = 0; i < colls.Length; i++)
        //          {
        //              if (colls[i].gameObject.tag == "ObjectWork")
        //              {
        //      
        //              }
        //              else
        //              {
        //                  player.work.SetActive(false);
        //                  player.workWindow = false;
        //              }
        //          }
        //      }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(player.storageWindow)
        {
            if (other.tag == "ObjectStorage")
            {
                player.storage.SetActive(false);
                player.storageWindow = false;
            }
        }
        else if(player.workWindow)
        {
            if(other.tag == "ObjectWork")
            {
                player.work.SetActive(false);
                player.workWindow = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (player.storageWindow)
        {
            if (collision.gameObject.tag == "ObjectStorage")
            {
                player.storage.SetActive(false);
                player.storageWindow = false;
            }
        }
        else if (player.workWindow)
        {
            if (collision.gameObject.tag == "ObjectWork")
            {
                player.work.SetActive(false);
                player.workWindow = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(rayTransform, transform.forward * rayDistance);
    }

}
