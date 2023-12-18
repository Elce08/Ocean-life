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
    public Canvas canvas; // UI를 표시할 Canvas
    public List<ItemManager> storages;
    //  public LayerMask obstacleLayer;
    public Handling handle;
    public Player player;
    public bool able;
    Vector3 setPosition;
    Collider[] inCollider;

    private void Awake()
    {
        playerAction = new();
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

    public void AddGameObject()
    {
        ray = new Ray(handle.transform.position, handle.transform.forward * 2.5f);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                setPosition = hit.point + (Vector3.up * 0.499f);
                if (CanPlaceObjectAtPoint())
                {
                    able = true;
                    ShowIndicator();
                }
                else
                {
                    able = false;
                    ShowIndicator();
                }
            }
        }
    }

    public void ShowIndicator()
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
        // 새로운 표시 오브젝트 생성
        currentIndicator = Instantiate(nowObject, setPosition, Quaternion.identity);
        currentIndicator.SetActive(true); // 새로운 오브젝트를 활성화
        if (!player.setStorage && !player.setWork)
        {
            Destroy(currentIndicator.gameObject);
        }
    }

    bool CanPlaceObjectAtPoint()
    {
        inCollider = Physics.OverlapBox(setPosition, new Vector3(0.5f, 0.5f, 0.5f));
        if (inCollider.Length > 2)
        {
            able = false;
            return false;
        }
        else if(hit.distance > 5.0f)
        {
            able = false;
            return false;
        }
        else
        {
            able = true;
            return true;
        }
    }

    /// <summary>
    /// 마우스 클릭시 실행되는 함수
    /// </summary>
    /// <param name="obj"></param>
    private void LeftDown(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (player.setStorage && able)  // 창고 생성이 활성화 중이라면
        {
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
            GameObject newGameObject = Instantiate(workStation, setPosition, Quaternion.identity);
            newGameObject.SetActive(true);  // 오브젝트 생성

            //      GameObject newUI = Instantiate(uiPrefab, canvas.transform.GetChild(1));
            //      newUI.SetActive(true);
            //      
            //      ItemManager newUIManager = newUI.GetComponent<ItemManager>();
            //      storages.Add(newUIManager); // UI생성
            //      
            //      foreach (ItemManager inven in storages)
            //      {
            //          inven.gameObject.name = $"Storage{storages.IndexOf(inven)}";
            //      
            //      }
            //      UIManager uiManager = newGameObject.AddComponent<UIManager>();
            //      uiManager.SetTargetSlot(newUIManager);  // UI 연결
            //      newUIManager.gameObject.SetActive(false);   // UI 닫기
            player.setWork = false;
            Destroy(currentIndicator);  // 생성되어 있는 임시 게임오브젝트 삭제
        }
    }
}
