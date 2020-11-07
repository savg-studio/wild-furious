using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectMenu : MonoBehaviour
{
    private const string PLAYER_NAME_KEY = "name";
    private const string PLAYER_NAME_DEFAULT = "anonymus";

    [SerializeField] private Button[] playButtons = new Button[0];
    [SerializeField] private Image[] playButtonImages = new Image[0];
    [SerializeField] private Color enabledButtonColor = new Color();
    [SerializeField] private Color disabledButtonColor = new Color();
    [SerializeField] private InputField nameInput = null;

    private string selectedCharacter = null;

    // UNITY METHODS

    private void Start()
    {
        // Disable all play buttons
        foreach (Button btn in playButtons)
        {
            btn.enabled = false;
        }
        foreach (Image img in playButtonImages)
        {
            img.color = disabledButtonColor;
        }

        // Get player's name
        ReadPlayerName();
    }

    // BUTTON METHODS

    public void SelectCharacter(string name)
    {
        // Enable all play buttons
        if (selectedCharacter == null && name != null)
        {
            foreach (Button btn in playButtons)
            {
                btn.enabled = true;
            }
            foreach (Image img in playButtonImages)
            {
                img.color = enabledButtonColor;
            }
        }

        // Save selected character
        selectedCharacter = name;
    }

    public void NormalMode()
    {
        StopMusic();
        SavePlayerName();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SpecialMode()
    {
        StopMusic();
        SavePlayerName();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    // PRIVATE METHODS

    private void ReadPlayerName()
    {
        string namePlayer = PlayerPrefs.GetString(PLAYER_NAME_KEY);

        if (namePlayer != null && nameInput != null)
        {
            nameInput.text = namePlayer;
        }
    }

    private void SavePlayerName()
    {
        string namePlayer = nameInput == null || string.IsNullOrWhiteSpace(nameInput.text) ? PLAYER_NAME_DEFAULT : nameInput.text;

        PlayerPrefs.SetString(PLAYER_NAME_KEY, namePlayer);
        PlayerPrefs.Save();
    }

    private void StopMusic()
    {
        MusicController music = FindObjectOfType<MusicController>();
        if (music != null) music.Stop();
    }
}
