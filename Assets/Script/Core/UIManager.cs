using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject targetObject; // UI�� ����� ���� ������Ʈ

    public void SetTargetObject(GameObject obj)
    {
        targetObject = obj;
    }

  
    public void UpdateUI()
    {
        // UI ������Ʈ  
    }
}
