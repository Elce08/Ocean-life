using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Handling : MonoBehaviour
{
    public Ray ray;
    RaycastHit hit;
    float rayDistance = 2.5f;   // Ray의 거리
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
    /// 레이 표시
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
        rayTransform = new Vector3(transform.position.x, transform.position.y + 0.35f, transform.position.z);   // ray의 시작위치
        ray = new Ray(rayTransform, transform.forward * rayDistance);   // ray 발사
        colls = Physics.OverlapSphere(transform.position, rayDistance + 0.5f);
        if (Physics.Raycast(ray, out hit, rayDistance, (-1) - ((1 << 3) | (1 << 4))))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Raycast"))
            {
                // 레이에 충돌한 오브젝트가 Raycast 레이어(상호작용이 가능한 오브젝트)
                rayHit = hit.collider.gameObject;
                shader = rayHit.GetComponent<Outline>();
                shader.OutlineMode = Outline.Mode.OutlineAll;   // 해당 오브젝트 outline 표시
                if (player.InvenState != Player.Inven.Close)
                {
                    // 플레이어의 인벤창이 켜져있는 상태라면
                    shader.OutlineMode = Outline.Mode.Noting;   // 아웃라인 표시 하지 않음
                }
                if (!viewText && player.InvenState == Player.Inven.Close)
                {
                    // viewText가 없고 인벤토리가 켜져있지 않은 상태면
                    child.gameObject.SetActive(true);
                    text.text = rayHit.gameObject.name;
                    viewText = true;
                }
                else if(player.InvenState != Player.Inven.Close)
                {
                    // 인벤토리가 켜져있는 상태라면
                    child.gameObject.SetActive(false);
                    viewText = false;
                }
                if(oldObject != rayHit)
                {
                    if(oldObject != null)
                    {
                        // 기존 오브젝트와 rayHit의 오브젝트가 다르다면
                        oldShader = oldObject.GetComponent<Outline>();
                        oldShader.OutlineMode = Outline.Mode.Noting;    // 기존 오브젝트 아웃라인 삭제
                    }
                }
                oldObject = rayHit.gameObject; // 기존 오브젝트에 현재 오브젝트 넣음
                
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
            // 창고 창이 켜져있는 상태
            foreach (Collider col in colls)
            {
                if (col.CompareTag("ObjectStorage"))
                {
                    // 일정 범위 안에 storage가 있다면 true
                    findTag = true;
                    break;
                }
                else
                {
                    // 일정 범위 안에 storage가 없다면 false
                    findTag = false;
                }
            }
            if (!findTag)
            {
                // 일정 범위 벗어나면 창고 창 닫음
                player.InvenState = Player.Inven.Close;
                player.storageWindow = false;
            }
        }
        else if (player.workWindow)
        {
            // 작업대 창이 켜져있는 상태
            foreach (Collider col in colls)
            {
                if (col.CompareTag("ObjectWork"))
                {
                    // 일정 범위 안에 작업대가 있다면 true
                    findTag = true;
                    break;
                }
                else
                {
                    // 일정 범위 안에 작업대가 없다면 false
                    findTag = false;
                }
            }
            if (!findTag)
            {
                // 일정 범위 벗어나면 작업대 창 닫음
                player.InvenState = Player.Inven.Close;
                player.workWindow = false;
            }
        }
    }
}
