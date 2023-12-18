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
    public GameObject ableGameObjPrefab; // ��ġ ������ ��ġ�� ��Ÿ���� �Ķ��� ������Ʈ ������
    public GameObject disableGameObjPrefab; // ��ġ �Ұ����� ��ġ�� ��Ÿ���� ������ ������Ʈ ������
    public GameObject storage; // �̸� �����ε� ���� ������Ʈ ������
    public GameObject workStation;  // �̸� �����ε� ���۴� ������
    PlayerInputAtions playerAction;

    public GameObject currentIndicator;     // ǥ�õǴ� ������Ʈ
    GameObject nowObject;
    RaycastHit hit;
    Ray ray;

    public GameObject uiPrefab; // �̸� �����ε� UI ������
    public Canvas canvas; // UI�� ǥ���� Canvas
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
        // ���ο� ǥ�� ������Ʈ ����
        currentIndicator = Instantiate(nowObject, setPosition, Quaternion.identity);
        currentIndicator.SetActive(true); // ���ο� ������Ʈ�� Ȱ��ȭ
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
    /// ���콺 Ŭ���� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="obj"></param>
    private void LeftDown(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (player.setStorage && able)  // â�� ������ Ȱ��ȭ ���̶��
        {
            GameObject newGameObject = Instantiate(storage, setPosition, Quaternion.identity);
            newGameObject.SetActive(true);  // ������Ʈ ����

            GameObject newUI = Instantiate(uiPrefab, canvas.transform.GetChild(2));
            newUI.SetActive(true);

            ItemManager newUIManager = newUI.GetComponent<ItemManager>();
            storages.Add(newUIManager); // UI����

            foreach (ItemManager inven in storages)
            {
                inven.gameObject.name = $"Storage{storages.IndexOf(inven)}";
            
            }
            UIManager uiManager = newGameObject.AddComponent<UIManager>();  
            uiManager.SetTargetSlot(newUIManager);  // UI ����
            newUIManager.gameObject.SetActive(false);   // UI �ݱ�
            player.setStorage = false;
            Destroy(currentIndicator);  // �����Ǿ� �ִ� �ӽ� ���ӿ�����Ʈ ����
        }

        else if (player.setWork && able)     // ���۴� ������ Ȱ��ȭ���̶��
        {
            GameObject newGameObject = Instantiate(workStation, setPosition, Quaternion.identity);
            newGameObject.SetActive(true);  // ������Ʈ ����

            //      GameObject newUI = Instantiate(uiPrefab, canvas.transform.GetChild(1));
            //      newUI.SetActive(true);
            //      
            //      ItemManager newUIManager = newUI.GetComponent<ItemManager>();
            //      storages.Add(newUIManager); // UI����
            //      
            //      foreach (ItemManager inven in storages)
            //      {
            //          inven.gameObject.name = $"Storage{storages.IndexOf(inven)}";
            //      
            //      }
            //      UIManager uiManager = newGameObject.AddComponent<UIManager>();
            //      uiManager.SetTargetSlot(newUIManager);  // UI ����
            //      newUIManager.gameObject.SetActive(false);   // UI �ݱ�
            player.setWork = false;
            Destroy(currentIndicator);  // �����Ǿ� �ִ� �ӽ� ���ӿ�����Ʈ ����
        }
    }
}
