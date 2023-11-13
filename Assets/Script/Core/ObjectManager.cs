using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ObjectManager : MonoBehaviour
{
    public GameObject gameObjPrefab; // �̸� �����ε� ���� ������Ʈ ������
    public GameObject uiPrefab; // �̸� �����ε� UI ������
    public Canvas canvas; // UI�� ǥ���� Canvas
    public List<ItemManager> storages;

    // ���ο� ���� ������Ʈ�� �߰��� �� ȣ��Ǵ� �Լ�
    public void AddGameObject()
    {
        GameObject newGameObject = Instantiate(gameObjPrefab, transform);

        // ���� ������Ʈ�� ���� �߰����� ���� �۾� ����

        // UI ���� �� ����
        ItemManager newUI = Instantiate(uiPrefab, canvas.transform.GetChild(1)).GetComponent<ItemManager>();
        storages.Add(newUI);
        foreach (ItemManager inven in storages)
        {
            inven.gameObject.name = $"Storage{storages.IndexOf(inven)}";
        }
        UIManager uiManager = newGameObject.AddComponent<UIManager>();
        uiManager.SetTargetSlot(newUI);
        newUI.gameObject.SetActive(false);
    }
}
