using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    Canvas canvas;
    TextMeshProUGUI text;
    Transform child;
    bool viewText = false;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        craft = FindObjectOfType<Crafting>();
        canvas = FindObjectOfType<Canvas>();
        child = canvas.transform.GetChild(5);
        Transform gChild = child.transform.GetChild(1);
        text = gChild.transform.GetComponent<TextMeshProUGUI>();
        child.gameObject.SetActive(false);
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
        if (Physics.Raycast(ray, out hit, rayDistance, (-1) - ((1 << 3) | (1 << 4))))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Raycast"))
            {
                rayHit = hit.collider.gameObject;
                shader = rayHit.GetComponent<Outline>();
                shader.OutlineMode = Outline.Mode.OutlineAll;
                if (player.InvenState != Player.Inven.Close) shader.OutlineMode = Outline.Mode.Noting;
                if (!viewText && player.InvenState == Player.Inven.Close)
                {
                    child.gameObject.SetActive(true);
                    text.text = rayHit.gameObject.name;
                    viewText = true;
                }
                else if(player.InvenState != Player.Inven.Close)
                {
                    child.gameObject.SetActive(false);
                    viewText = false;
                }
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
            if(viewText)
            {
                child.gameObject.SetActive(false);
                viewText = false;
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
