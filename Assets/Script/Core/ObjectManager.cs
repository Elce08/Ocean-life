using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ObjectManager : MonoBehaviour
{
    public GameObject gameObjPrefab; // 미리 디자인된 게임 오브젝트 프리팹
    public GameObject uiPrefab; // 미리 디자인된 UI 프리팹
    public Canvas canvas; // UI를 표시할 Canvas

    // 새로운 게임 오브젝트가 추가될 때 호출되는 함수
    public void AddGameObject()
    {
        GameObject newGameObject = Instantiate(gameObjPrefab, transform);

        // 게임 오브젝트에 대한 추가적인 설정 작업 수행

        // UI 생성 및 연결
        GameObject newUI = Instantiate(uiPrefab, canvas.transform);
        UIManager uiManager = newUI.GetComponent<UIManager>();

        if (uiManager != null)
        {
            // UI에 정보 전달
            uiManager.SetTargetObject(newGameObject);
        }
    }
}
