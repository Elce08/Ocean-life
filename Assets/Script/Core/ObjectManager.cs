using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ObjectManager : MonoBehaviour
{
    public GameObject ableGameObjPrefab; // ��ġ ������ ��ġ�� ��Ÿ���� �Ķ��� ������Ʈ ������
    public GameObject disableGameObjPrefab; // ��ġ �Ұ����� ��ġ�� ��Ÿ���� ������ ������Ʈ ������
    public GameObject gameObjPrefab; // �̸� �����ε� ���� ������Ʈ ������

    GameObject currentIndicator;     // ǥ�õǴ� ������Ʈ
    GameObject nowObject;
    RaycastHit hit;

    public GameObject uiPrefab; // �̸� �����ε� UI ������
    public Canvas canvas; // UI�� ǥ���� Canvas
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
            // ���� ������Ʈ ����
            GameObject newGameObject = Instantiate(gameObjPrefab, setPosition, Quaternion.identity);
            newGameObject.SetActive(true); // ���ο� ���� ������Ʈ�� Ȱ��ȭ�մϴ�.

            // UI ������ ���� �� Ȱ��ȭ
            GameObject newUI = Instantiate(uiPrefab, canvas.transform.GetChild(1));
            newUI.SetActive(true); // ���ο� UI�� Ȱ��ȭ�մϴ�.
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

    // ���ο� ���� ������Ʈ�� �߰��� �� ȣ��Ǵ� �Լ�
    public void AddGameObject()
    {
        // ���� ������Ʈ�� ���� �߰����� ���� �۾� ����
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
        Collider collider = prefab.GetComponent<Collider>(); // ������Ʈ �������� �ݶ��̴� ��������

        // �ݶ��̴��� ũ�⸦ ������� ������Ʈ ��ġ �ֺ��� �ڽ� ����
        Vector3 size = collider.bounds.size;
        Collider[] otherColliders = Physics.OverlapBox(setPosition, size / 2.0f, Quaternion.identity);

        foreach (Collider otherCollider in otherColliders)
        {
            // ���� ������Ʈ�� �ݶ��̴��� ��ġ�� �ݶ��̴� �� Ground ���̾ �ƴ� ���
            if (otherCollider != collider && otherCollider.gameObject.layer != LayerMask.NameToLayer("Ground"))
            {
                return false; // ��ġ �Ұ���
            }
        }

        return true; // ��ġ ����
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
            // ���ο� ǥ�� ������Ʈ ����
            currentIndicator = Instantiate(nowObject, setPosition, Quaternion.identity);
            currentIndicator.SetActive(true); // ���ο� ������Ʈ�� Ȱ��ȭ�մϴ�.
        }
        else
        {
            Destroy(currentIndicator);
        }
    }
}
