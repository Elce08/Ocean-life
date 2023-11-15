using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ObjectManager : MonoBehaviour
{
    public GameObject ableGameObjPrefab; // ��ġ ������ ��ġ�� ��Ÿ���� �Ķ��� ������Ʈ ������
    public GameObject disableGameObjPrefab; // ��ġ �Ұ����� ��ġ�� ��Ÿ���� ������ ������Ʈ ������
    public GameObject gameObjPrefab; // �̸� �����ε� ���� ������Ʈ ������
    GameObject currentIndicator;     // ǥ�õǴ� ������Ʈ
    public GameObject uiPrefab; // �̸� �����ε� UI ������
    public Canvas canvas; // UI�� ǥ���� Canvas
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

    // ���ο� ���� ������Ʈ�� �߰��� �� ȣ��Ǵ� �Լ�
    public void AddGameObject()
    {
        // ���� ������Ʈ�� ���� �߰����� ���� �۾� ����
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
                        // ���� ������Ʈ ����
                        GameObject newGameObject = Instantiate(gameObjPrefab, hit.point, Quaternion.identity);
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
        Collider[] colliders = prefab.GetComponentsInChildren<Collider>(); // ������Ʈ �������� �ݶ��̴� ��������

        foreach (Collider collider in colliders)
        {
            Vector3 size = collider.bounds.size; // �ݶ��̴��� ũ�� ��������
            Collider[] overlappingColliders = Physics.OverlapBox(point, size / 2.0f);

            // ��ġ�Ϸ��� ��ġ �ֺ��� �ٸ� �ݶ��̴��� �ִ��� Ȯ��
            if (overlappingColliders.Length > 1) // ���� �ڽ��� �ݶ��̴��� �����ϰ� �ٸ� �ݶ��̴��� �ִٸ�
            {
                return false; // ��ġ �Ұ���
            }
        }

        return true; // ��ġ ����
    }

    void ShowIndicator(GameObject indicatorPrefab, Vector3 position)
    {
        if (currentIndicator != null)
        {
            Destroy(currentIndicator);
        }

        // ���ο� ǥ�� ������Ʈ ����
        currentIndicator = Instantiate(indicatorPrefab, position, Quaternion.identity);
        currentIndicator.SetActive(true); // ���ο� ������Ʈ�� Ȱ��ȭ�մϴ�.
    }
}
