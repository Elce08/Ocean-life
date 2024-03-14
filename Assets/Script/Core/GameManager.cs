using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pause;

    private void Awake()
    {
        pause = GameObject.FindGameObjectWithTag("Pause");
        pause.SetActive(false);
    }

    public void Resume()
    {
        Debug.Log("Resume 실행");
        pause.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Quit()
    {
        Debug.Log("Quit 실행");
        Application.Quit();
    }
}
