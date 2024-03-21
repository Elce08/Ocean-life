using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;

public class ObjectManager : MonoBehaviour
{
    public GameObject ableGameObjPrefab; // 설치 가능한 위치를 나타내는 파란색 오브젝트 프리팹
    public GameObject disableGameObjPrefab; // 설치 불가능한 위치를 나타내는 빨간색 오브젝트 프리팹
    public GameObject storage; // 미리 디자인된 게임 오브젝트 프리팹
    public GameObject workStation;  // 미리 디자인된 제작대 프리팹
    PlayerInputAtions playerAction;

    public GameObject currentIndicator;     // 표시되는 오브젝트
    GameObject nowObject;
    RaycastHit hit;
    Ray ray;

    public GameObject uiPrefab; // 미리 디자인된 UI 프리팹
    Canvas canvas; // UI를 표시할 Canvas
    public List<ItemManager> storages;
    //  public LayerMask obstacleLayer;
    Handling handle;
    Player player;
    public bool able;
    Vector3 setPosition;
    Collider[] inCollider;

    ItemManager playerInven;

    private void Awake()
    {
        handle = FindObjectOfType<Handling>();
        player = FindObjectOfType<Player>();
        canvas = FindObjectOfType<Canvas>();
        playerAction = new();
        playerInven = FindObjectOfType<Canvas>().GetComponentInChildren<ItemManager>();
    }

    private void Update()
    {
        if (player.setStorage || player.setWork)
        {
            AddGameObject();
        }
    }
    private void OnEnable()
    {
        playerAction.Storage.Enable();
        playerAction.Storage.Set.performed += LeftDown;
    }

    private void OnDisable()
    {
        playerAction.Storage.Set.performed -= LeftDown;
        playerAction.Storage.Disable();
    }

    /// <summary>
    /// 레이를 이용해 오브젝트가 생성될 위치를 정하는 함수
    /// </summary>
    public void AddGameObject()
    {
        ray = new Ray(handle.transform.position, handle.transform.forward * 2.5f);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, (-1) - (1<<8)))
        {
            // 레이 충돌
            setPosition = hit.point + (Vector3.up * 0.499f);
            ShowIndicator();    // 임시 오브젝트 표시
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("House"))
            {
                if (CanPlaceObjectAtPoint())
                {
                    able = true;
                }
                else
                {
                    able = false;
                }
            }
            else
            {
                able = false;
            }
        }
    }

    /// <summary>
    /// 설치될 지역에 임시 오브젝트를 표시하는 함수
    /// </summary>
    public void ShowIndicator()
    {
        if (able)
        {
            // able이 true면 설치 가능
            nowObject = ableGameObjPrefab;
        }
        else
        {
            // able이 false면 설치 불가능
            nowObject = disableGameObjPrefab;
        }

        if (currentIndicator != null)
        {
            // 기존 임시 오브젝트 삭제
            Destroy(currentIndicator);
        }
        // 새로운 표시 오브젝트 생성
        currentIndicator = Instantiate(nowObject, setPosition, Quaternion.identity);
        currentIndicator.SetActive(true); // 새로운 오브젝트를 활성화

        if (!player.setStorage && !player.setWork)
        {
            // Storage와 WorkStation 설치 모드가 활성화 중이지 않다면
            Destroy(currentIndicator.gameObject); // 삭제
        }
    }

    /// <summary>
    /// 설치 가능한지 확인하는 bool
    /// </summary>
    /// <returns></returns>
    bool CanPlaceObjectAtPoint()
    {
        if(PayCheck())
        {
            // 설치시 필요한 재료가 있는지 확인
            inCollider = Physics.OverlapBox(setPosition, new Vector3(0.5f, 0.5f, 0.5f));
            if (inCollider.Length > 3)
            {
                // 임시 오브젝트, 바닥, 그외 다른것들
                // Debug.Log("콜라이더 3개이상");
                able = false;
                return false;
            }
            else if (hit.distance > 5.0f)
            {
                // Debug.Log("거리 5이상");
                able = false;
                return false;
            }
            else
            {
                // Debug.Log("가능");
                able = true;
                return true;
            }
        }
        else
        {
            able = false;
            return false;
        }
    }

    /// <summary>
    /// 마우스 클릭시 실행되는 함수
    /// </summary>
    /// <param name="obj"></param>
    private void LeftDown(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (player.setStorage && able)  // 창고 생성이 활성화 중이고 설치 가능한 상태라면
        {
            Pay();  // 재료 소모
            GameObject newGameObject = Instantiate(storage, setPosition, Quaternion.identity);
            newGameObject.SetActive(true);  // 오브젝트 생성

            GameObject newUI = Instantiate(uiPrefab, canvas.transform.GetChild(2));
            newUI.SetActive(true);

            ItemManager newUIManager = newUI.GetComponent<ItemManager>();
            storages.Add(newUIManager); // UI생성

            foreach (ItemManager inven in storages)
            {
                inven.gameObject.name = $"Storage{storages.IndexOf(inven)}";
            
            }
            UIManager uiManager = newGameObject.AddComponent<UIManager>();  
            uiManager.SetTargetSlot(newUIManager);  // UI 연결
            newUIManager.gameObject.SetActive(false);   // UI 닫기
            player.setStorage = false;
            Destroy(currentIndicator);  // 생성되어 있는 임시 게임오브젝트 삭제
        }

        else if (player.setWork && able)     // 제작대 생성이 활성화중이라면
        {
            Pay();  // 재료 소모
            GameObject newGameObject = Instantiate(workStation, setPosition, Quaternion.identity);
            newGameObject.SetActive(true);  // 오브젝트 생성
            player.setWork = false;
            Destroy(currentIndicator);  // 생성되어 있는 임시 게임오브젝트 삭제
        }
    }

    /// <summary>
    /// 재료를 소모하는 함수
    /// </summary>
    private void Pay()
    {
        playerInven.Remove(Item.titanium);
        playerInven.Remove(Item.titanium);
        playerInven.Remove(Item.titanium);
        playerInven.Remove(Item.titanium);
    }

    /// <summary>
    /// 필요한 재료가 있는지 확인하는 bool
    /// </summary>
    /// <returns></returns>
    private bool PayCheck()
    {
        int titaniumNeeded = 4;                                                                 // 소모값
        int titaniumCount = 0;                                                                  // 소모값 카운트
        foreach (Item item in playerInven.items) if (item == Item.titanium) titaniumCount++;    // 플레이어 인벤의 티타늄 카운트
        if (titaniumCount < titaniumNeeded) return false;
        else return true;
    }
}
