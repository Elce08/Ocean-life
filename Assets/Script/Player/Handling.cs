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

    private void Awake()
    {
    }

    private void Update()
    {
        rayTransform = new Vector3(transform.position.x, transform.position.y + 0.35f, transform.position.z);
        ray = new Ray(rayTransform, transform.forward * rayDistance);
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Raycast"))
            {
                rayHit = hit.collider.gameObject;
            }
        }
        else
        {
            rayHit = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(rayTransform, transform.forward * rayDistance);
    }

}
