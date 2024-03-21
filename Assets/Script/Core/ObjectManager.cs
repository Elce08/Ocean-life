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
    Canvas canvas; // UI�� ǥ���� Canvas
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
    /// ���̸� �̿��� ������Ʈ�� ������ ��ġ�� ���ϴ� �Լ�
    /// </summary>
    public void AddGameObject()
    {
        ray = new Ray(handle.transform.position, handle.transform.forward * 2.5f);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, (-1) - (1<<8)))
        {
            // ���� �浹
            setPosition = hit.point + (Vector3.up * 0.499f);
            ShowIndicator();    // �ӽ� ������Ʈ ǥ��
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
    /// ��ġ�� ������ �ӽ� ������Ʈ�� ǥ���ϴ� �Լ�
    /// </summary>
    public void ShowIndicator()
    {
        if (able)
        {
            // able�� true�� ��ġ ����
            nowObject = ableGameObjPrefab;
        }
        else
        {
            // able�� false�� ��ġ �Ұ���
            nowObject = disableGameObjPrefab;
        }

        if (currentIndicator != null)
        {
            // ���� �ӽ� ������Ʈ ����
            Destroy(currentIndicator);
        }
        // ���ο� ǥ�� ������Ʈ ����
        currentIndicator = Instantiate(nowObject, setPosition, Quaternion.identity);
        currentIndicator.SetActive(true); // ���ο� ������Ʈ�� Ȱ��ȭ

        if (!player.setStorage && !player.setWork)
        {
            // Storage�� WorkStation ��ġ ��尡 Ȱ��ȭ ������ �ʴٸ�
            Destroy(currentIndicator.gameObject); // ����
        }
    }

    /// <summary>
    /// ��ġ �������� Ȯ���ϴ� bool
    /// </summary>
    /// <returns></returns>
    bool CanPlaceObjectAtPoint()
    {
        if(PayCheck())
        {
            // ��ġ�� �ʿ��� ��ᰡ �ִ��� Ȯ��
            inCollider = Physics.OverlapBox(setPosition, new Vector3(0.5f, 0.5f, 0.5f));
            if (inCollider.Length > 3)
            {
                // �ӽ� ������Ʈ, �ٴ�, �׿� �ٸ��͵�
                // Debug.Log("�ݶ��̴� 3���̻�");
                able = false;
                return false;
            }
            else if (hit.distance > 5.0f)
            {
                // Debug.Log("�Ÿ� 5�̻�");
                able = false;
                return false;
            }
            else
            {
                // Debug.Log("����");
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
    /// ���콺 Ŭ���� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="obj"></param>
    private void LeftDown(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (player.setStorage && able)  // â�� ������ Ȱ��ȭ ���̰� ��ġ ������ ���¶��
        {
            Pay();  // ��� �Ҹ�
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
            Pay();  // ��� �Ҹ�
            GameObject newGameObject = Instantiate(workStation, setPosition, Quaternion.identity);
            newGameObject.SetActive(true);  // ������Ʈ ����
            player.setWork = false;
            Destroy(currentIndicator);  // �����Ǿ� �ִ� �ӽ� ���ӿ�����Ʈ ����
        }
    }

    /// <summary>
    /// ��Ḧ �Ҹ��ϴ� �Լ�
    /// </summary>
    private void Pay()
    {
        playerInven.Remove(Item.titanium);
        playerInven.Remove(Item.titanium);
        playerInven.Remove(Item.titanium);
        playerInven.Remove(Item.titanium);
    }

    /// <summary>
    /// �ʿ��� ��ᰡ �ִ��� Ȯ���ϴ� bool
    /// </summary>
    /// <returns></returns>
    private bool PayCheck()
    {
        int titaniumNeeded = 4;                                                                 // �Ҹ�
        int titaniumCount = 0;                                                                  // �Ҹ� ī��Ʈ
        foreach (Item item in playerInven.items) if (item == Item.titanium) titaniumCount++;    // �÷��̾� �κ��� ƼŸ�� ī��Ʈ
        if (titaniumCount < titaniumNeeded) return false;
        else return true;
    }
}
