﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] public GameObject pauseMenuPanel = null;

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
                ShowAndPause();
            }
            else
            {
                HideAndResume();
            }
        }
    }

    // PAUSE MENU BUTTONS' LISTENERS

    public void ShowAndPause()
    {
        pauseMenuPanel.SetActive(true);
        Pause();
    }

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

    // PAUSE AND RESUME FUNCTIONS

    void Pause()
    {
        Time.timeScale = 0;
    }

    void Resume()
    {
        Time.timeScale = 1;
    }
}