using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject targetObject; // UI가 연결된 게임 오브젝트

    public void SetTargetObject(GameObject obj)
    {
        targetObject = obj;
    }

  
    public void UpdateUI()
    {
        // UI 업데이트  
    }
}
