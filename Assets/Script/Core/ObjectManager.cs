using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ObjectManager : MonoBehaviour
{
    public GameObject gameObjPrefab; // �̸� �����ε� ���� ������Ʈ ������
    public GameObject uiPrefab; // �̸� �����ε� UI ������
    public Canvas canvas; // UI�� ǥ���� Canvas

    // ���ο� ���� ������Ʈ�� �߰��� �� ȣ��Ǵ� �Լ�
    public void AddGameObject()
    {
        GameObject newGameObject = Instantiate(gameObjPrefab, transform);

        // ���� ������Ʈ�� ���� �߰����� ���� �۾� ����

        // UI ���� �� ����
        GameObject newUI = Instantiate(uiPrefab, canvas.transform);
        UIManager uiManager = newUI.GetComponent<UIManager>();

        if (uiManager != null)
        {
            // UI�� ���� ����
            uiManager.SetTargetObject(newGameObject);
        }
    }
}
