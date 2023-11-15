using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ObjectManager : MonoBehaviour
{
    public GameObject ableGameObjPrefab; // 설치 가능한 위치를 나타내는 파란색 오브젝트 프리팹
    public GameObject disableGameObjPrefab; // 설치 불가능한 위치를 나타내는 빨간색 오브젝트 프리팹
    public GameObject gameObjPrefab; // 미리 디자인된 게임 오브젝트 프리팹
    GameObject currentIndicator;     // 표시되는 오브젝트
    public GameObject uiPrefab; // 미리 디자인된 UI 프리팹
    public Canvas canvas; // UI를 표시할 Canvas
    public List<ItemManager> storages;
    //  public LayerMask obstacleLayer;
    public Handling handle;
    public Player player;

    private void Update()
    {
        if(player.setStorage)
        {
            AddGameObject();
        }
    }

    // 새로운 게임 오브젝트가 추가될 때 호출되는 함수
    public void AddGameObject()
    {
        // 게임 오브젝트에 대한 추가적인 설정 작업 수행
        Ray ray = new Ray(handle.transform.position, handle.transform.forward * 2.5f);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground") )
            {
                if (CanPlaceObjectAtPoint(hit.point, gameObjPrefab))
                {
                    ShowIndicator(ableGameObjPrefab, hit.point);
                    if (Input.GetMouseButtonDown(0))
                    {
                        // 게임 오브젝트 생성
                        GameObject newGameObject = Instantiate(gameObjPrefab, hit.point, Quaternion.identity);
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
                    }
                }
                else
                {
                    ShowIndicator(disableGameObjPrefab, hit.point);
                }
            }
        }
        else
        {
            ShowIndicator(disableGameObjPrefab, hit.point);
        }
    }

    bool CanPlaceObjectAtPoint(Vector3 point, GameObject prefab)
    {
        Collider[] colliders = prefab.GetComponentsInChildren<Collider>(); // 오브젝트 프리팹의 콜라이더 가져오기

        foreach (Collider collider in colliders)
        {
            Vector3 size = collider.bounds.size; // 콜라이더의 크기 가져오기
            Collider[] overlappingColliders = Physics.OverlapBox(point, size / 2.0f);

            // 설치하려는 위치 주변에 다른 콜라이더가 있는지 확인
            if (overlappingColliders.Length > 1) // 현재 자신의 콜라이더를 제외하고 다른 콜라이더가 있다면
            {
                return false; // 설치 불가능
            }
        }

        return true; // 설치 가능
    }

    void ShowIndicator(GameObject indicatorPrefab, Vector3 position)
    {
        if (currentIndicator != null)
        {
            Destroy(currentIndicator);
        }

        // 새로운 표시 오브젝트 생성
        currentIndicator = Instantiate(indicatorPrefab, position, Quaternion.identity);
        currentIndicator.SetActive(true); // 새로운 오브젝트를 활성화합니다.
    }
}
