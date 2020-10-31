using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel = null;

    // PAUSE AND RESUME FUNCTIONS

    public static void Pause()
    {
        Time.timeScale = 0;
    }

    public static void Resume()
    {
        Time.timeScale = 1;
    }

    // HIDE PAUSE MENU ON START

    void Start()
    {
        pauseMenuPanel.SetActive(false);
    }

    // TOGGLE PAUSE MENU WITH "ESC" KEY

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (!pauseMenuPanel.activeInHierarchy)
            {
                pauseMenuPanel.SetActive(true);
                Pause();
            }
            else
            {
                pauseMenuPanel.SetActive(false);
                Resume();
            }
        }
    }

    // PAUSE MENU BUTTONS' LISTENERS

    public void HideAndResume()
    {
        pauseMenuPanel.SetActive(false);
        Resume();
    }

    public void OpenSettings()
    {
        Debug.Log("Settings menu not implemented!");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
