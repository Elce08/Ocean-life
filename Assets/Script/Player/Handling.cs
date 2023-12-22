using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handling : MonoBehaviour
{
    public Ray ray;
    RaycastHit hit;
    float rayDistance = 2.5f;
    public GameObject rayHit;
    Vector3 rayTransform;
    Player player;
    Collider[] colls;
    Crafting craft;
    bool findTag = false;

    Outline shader;
    Outline oldShader;
    GameObject oldObject = null;

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
        if (Physics.Raycast(ray, out hit, rayDistance, (-1) - (1 << 3)))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Raycast"))
            {
                rayHit = hit.collider.gameObject;
                shader = rayHit.GetComponent<Outline>();
                shader.OutlineMode = Outline.Mode.OutlineAll;
                if(oldObject != rayHit)
                {
                    if(oldObject != null)
                    {
                        oldShader = oldObject.GetComponent<Outline>();
                        oldShader.OutlineMode = Outline.Mode.Noting;
                    }
                }
                oldObject = rayHit.gameObject;
            }
        }
        else
        {
            if ((shader != null) && (rayHit != null))  //rayHit != null && 
            {
                shader.OutlineMode = Outline.Mode.Noting; // 선택되지 않았을 때
                shader = null;
            }
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
