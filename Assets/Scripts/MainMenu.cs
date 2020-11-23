using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Text savgsTitle = null;
    [SerializeField] private Text marioCarTitle = null;
    [SerializeField] private Text wildAndFuriousTitle = null;
    [SerializeField] private Color textColor = new Color();

    [SerializeField] private Text playButtonText = null;
    [SerializeField] private Text exitButtonText = null;
    [SerializeField] private Image playButtonImage = null;
    [SerializeField] private Image exitButtonImage = null;
    [SerializeField] private Color buttonTextColor = new Color();
    [SerializeField] private Color buttonImageColor = new Color();

    void Start()
    {
        // Start audio
        MusicController music = FindObjectOfType<MusicController>();
        if (music != null) music.Play();

        // Set initial color
        Color initialTextColor = GetAlphaColor(textColor, 0);
        savgsTitle.color = initialTextColor;
        marioCarTitle.color = initialTextColor;
        wildAndFuriousTitle.color = initialTextColor;
        Color initialButtonTextColor = GetAlphaColor(buttonTextColor, 0);
        playButtonText.color = initialButtonTextColor;
        exitButtonText.color = initialButtonTextColor;
        Color initialButtonImageColor = GetAlphaColor(buttonImageColor, 0);
        playButtonImage.color = initialButtonImageColor;
        exitButtonImage.color = initialButtonImageColor;

        // Start animation
        StartCoroutine(AnimateTitles());
    }

    void Update()
    {
        
    }

    private IEnumerator AnimateTitles()
    {
        // FADE IN TITLES (ONE BY ONE)
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            savgsTitle.color = GetAlphaColor(textColor, i);
            yield return null;
        }
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            marioCarTitle.color = GetAlphaColor(textColor, i);
            yield return null;
        }
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            wildAndFuriousTitle.color = GetAlphaColor(textColor, i);
            yield return null;
        }

        // FADE IN BUTTONS (SIMULANEOUSLY)
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            playButtonText.color = GetAlphaColor(buttonTextColor, i);
            exitButtonText.color = GetAlphaColor(buttonTextColor, i);
            playButtonImage.color = GetAlphaColor(buttonImageColor, i);
            exitButtonImage.color = GetAlphaColor(buttonImageColor, i);
            yield return null;
        }
    }

    private Color GetAlphaColor(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    // BUTTON METHODS

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
