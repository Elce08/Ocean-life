using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Handling : MonoBehaviour
{
    public Ray ray;
    RaycastHit hit;
    float rayDistance = 2.5f;   // Ray�� �Ÿ�
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

    /// <summary>
    /// ���� ǥ��
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(rayTransform, transform.forward * rayDistance);
    }

    /// <summary>
    /// Ray
    /// </summary>
    private void Raycast()
    {
        rayTransform = new Vector3(transform.position.x, transform.position.y + 0.35f, transform.position.z);   // ray�� ������ġ
        ray = new Ray(rayTransform, transform.forward * rayDistance);   // ray �߻�
        colls = Physics.OverlapSphere(transform.position, rayDistance + 0.5f);
        if (Physics.Raycast(ray, out hit, rayDistance, (-1) - ((1 << 3) | (1 << 4))))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Raycast"))
            {
                // ���̿� �浹�� ������Ʈ�� Raycast ���̾�(��ȣ�ۿ��� ������ ������Ʈ)
                rayHit = hit.collider.gameObject;
                shader = rayHit.GetComponent<Outline>();
                shader.OutlineMode = Outline.Mode.OutlineAll;   // �ش� ������Ʈ outline ǥ��
                if (player.InvenState != Player.Inven.Close)
                {
                    // �÷��̾��� �κ�â�� �����ִ� ���¶��
                    shader.OutlineMode = Outline.Mode.Noting;   // �ƿ����� ǥ�� ���� ����
                }
                if (!viewText && player.InvenState == Player.Inven.Close)
                {
                    // viewText�� ���� �κ��丮�� �������� ���� ���¸�
                    child.gameObject.SetActive(true);
                    text.text = rayHit.gameObject.name;
                    viewText = true;
                }
                else if(player.InvenState != Player.Inven.Close)
                {
                    // �κ��丮�� �����ִ� ���¶��
                    child.gameObject.SetActive(false);
                    viewText = false;
                }
                if(oldObject != rayHit)
                {
                    if(oldObject != null)
                    {
                        // ���� ������Ʈ�� rayHit�� ������Ʈ�� �ٸ��ٸ�
                        oldShader = oldObject.GetComponent<Outline>();
                        oldShader.OutlineMode = Outline.Mode.Noting;    // ���� ������Ʈ �ƿ����� ����
                    }
                }
                oldObject = rayHit.gameObject; // ���� ������Ʈ�� ���� ������Ʈ ����
                
            }
        }
        else
        {
            if ((shader != null) && (rayHit != null))  //rayHit != null && 
            {
                shader.OutlineMode = Outline.Mode.Noting; // ���õ��� �ʾ��� ��
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
            // â�� â�� �����ִ� ����
            foreach (Collider col in colls)
            {
                if (col.CompareTag("ObjectStorage"))
                {
                    // ���� ���� �ȿ� storage�� �ִٸ� true
                    findTag = true;
                    break;
                }
                else
                {
                    // ���� ���� �ȿ� storage�� ���ٸ� false
                    findTag = false;
                }
            }
            if (!findTag)
            {
                // ���� ���� ����� â�� â ����
                player.InvenState = Player.Inven.Close;
                player.storageWindow = false;
            }
        }
        else if (player.workWindow)
        {
            // �۾��� â�� �����ִ� ����
            foreach (Collider col in colls)
            {
                if (col.CompareTag("ObjectWork"))
                {
                    // ���� ���� �ȿ� �۾��밡 �ִٸ� true
                    findTag = true;
                    break;
                }
                else
                {
                    // ���� ���� �ȿ� �۾��밡 ���ٸ� false
                    findTag = false;
                }
            }
            if (!findTag)
            {
                // ���� ���� ����� �۾��� â ����
                player.InvenState = Player.Inven.Close;
                player.workWindow = false;
            }
        }
    }
}
