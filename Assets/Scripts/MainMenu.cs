using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Text savgsTitle;
    [SerializeField] private Text marioCarTitle;
    [SerializeField] private Text wildAndFuriousTitle;
    [SerializeField] private Color textColor = new Color();
    //[SerializeField] private float fadeDuration = 1;

    void Start()
    {
        Color initialColor = GetAlphaColor(textColor, 0);
        savgsTitle.color = initialColor;
        marioCarTitle.color = initialColor;
        wildAndFuriousTitle.color = initialColor;

        StartCoroutine(AnimateTitles());
    }

    void Update()
    {
        
    }

    private IEnumerator AnimateTitles()
    {
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
    }

    private Color GetAlphaColor(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
}
