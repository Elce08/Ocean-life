using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Player;

public class Pause : MonoBehaviour
{
    Button resume;
    Button quit;
    Player player;

    private void Awake()
    {
        resume = transform.GetChild(0).GetComponent<Button>();
        quit = transform.GetChild(1).GetComponent<Button>();
        player = FindObjectOfType<Player>();
        resume.onClick.AddListener(Resume);
        quit.onClick.AddListener(Quit);
    }

    /// <summary>
    /// 게임 재개
    /// </summary>
    private void Resume()
    {
        player.Close_State();
        player.currentGameState = Player.GameState.Resume;
        Time.timeScale = 1;
    }

    /// <summary>
    /// 게임 종료
    /// </summary>
    private void Quit()
    {
        Application.Quit();
    }
}
