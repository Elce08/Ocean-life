using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ObjectManager : MonoBehaviour
{
    public GameObject ableGameObjPrefab; // 설치 가능한 위치를 나타내는 파란색 오브젝트 프리팹
    public GameObject disableGameObjPrefab; // 설치 불가능한 위치를 나타내는 빨간색 오브젝트 프리팹
    public GameObject gameObjPrefab; // 미리 디자인된 게임 오브젝트 프리팹

    GameObject currentIndicator;     // 표시되는 오브젝트
    GameObject nowObject;
    RaycastHit hit;

    public GameObject uiPrefab; // 미리 디자인된 UI 프리팹
    public Canvas canvas; // UI를 표시할 Canvas
    public List<ItemManager> storages;
    //  public LayerMask obstacleLayer;
    public Handling handle;
    public Player player;
    bool able;
    Vector3 setPosition;

    private void Update()
    {
        if(player.setStorage)
        {
            AddGameObject();
        }
        if(able == true)
        {
            leftClick();
        }
    }

    private void leftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 게임 오브젝트 생성
            GameObject newGameObject = Instantiate(gameObjPrefab, setPosition, Quaternion.identity);
            newGameObject.SetActive(true); // 새로운 게임 오브젝트를 활성화합니다.

            // UI 프리팹 생성 및 활성화
            GameObject newUI = Instantiate(uiPrefab, canvas.transform.GetChild(1));
            newUI.SetActive(true); // 새로운 UI를 활성화합니다.
            ItemManager newUIManager = newUI.GetComponent<ItemManager>();
            storages.Add(newUIManager);
            foreach (ItemManager inven in storages)
            {
                inven.gameObject.name = $"Storage{storages.IndexOf(inven)}";
            }

            UIManager uiManager = newGameObject.AddComponent<UIManager>();
            uiManager.SetTargetSlot(newUIManager);
            newUIManager.gameObject.SetActive(false);
            player.setStorage = false;
            ShowIndicator(hit.point);
        }
    }

    // 새로운 게임 오브젝트가 추가될 때 호출되는 함수
    public void AddGameObject()
    {
        // 게임 오브젝트에 대한 추가적인 설정 작업 수행
        Ray ray = new Ray(handle.transform.position, handle.transform.forward * 2.5f);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground") )
            {
                ShowIndicator(hit.point);
                if (CanPlaceObjectAtPoint(currentIndicator))
                {
                    able = true;
                }
                else
                {
                    able = false;
                }
            }
        }
    }

    bool CanPlaceObjectAtPoint(GameObject prefab)
    {
        Collider collider = prefab.GetComponent<Collider>(); // 오브젝트 프리팹의 콜라이더 가져오기

        // 콜라이더의 크기를 기반으로 오브젝트 위치 주변에 박스 생성
        Vector3 size = collider.bounds.size;
        Collider[] otherColliders = Physics.OverlapBox(setPosition, size / 2.0f, Quaternion.identity);

        foreach (Collider otherCollider in otherColliders)
        {
            // 현재 오브젝트의 콜라이더와 겹치는 콜라이더 중 Ground 레이어가 아닌 경우
            if (otherCollider != collider && otherCollider.gameObject.layer != LayerMask.NameToLayer("Ground"))
            {
                return false; // 설치 불가능
            }
        }

        return true; // 설치 가능
    }

    void ShowIndicator(Vector3 position)
    {
        if(player.setStorage)
        {
            if (able)
            {
                nowObject = ableGameObjPrefab;
            }
            else
            {
                nowObject = disableGameObjPrefab;
            }
            if (currentIndicator != null)
            {
                Destroy(currentIndicator);
            }
            setPosition = (position + (Vector3.up * 0.5f));
            // 새로운 표시 오브젝트 생성
            currentIndicator = Instantiate(nowObject, setPosition, Quaternion.identity);
            currentIndicator.SetActive(true); // 새로운 오브젝트를 활성화합니다.
        }
        else
        {
            Destroy(currentIndicator);
        }
    }
}
