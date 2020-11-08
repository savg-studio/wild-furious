using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankingController : MonoBehaviour
{
    private const string BASE_URL = "https://ranking-api.savgs.xoanweb.com";

    [SerializeField] private GameObject rankingPanel = null;
    [SerializeField] private Text placeLabel = null;
    [SerializeField] private Text scoreLabel = null;
    [SerializeField] private GameObject[] tableRows = null;
    [SerializeField] private Color yourRowColor = new Color(1, 255, 34, 192);
    [SerializeField] private Color otherRowColor = new Color(35, 255, 246, 192);

    // HIDE RANKING PANEL ON START

    void Start()
    {
        if (rankingPanel == null || placeLabel == null || scoreLabel == null || tableRows == null)
        {
            Debug.LogError("RankingController error: rankingPanel, placeLabel, scoreLabel and tableRows cannot be null");
        }
        else
        {
            rankingPanel.SetActive(false);
        }

        // Example of usage: `ShowRanking("Integration Test", 10.5f, "TEST-3", "Mario", 1);`
    }

    // PRESS ESC/SPACE/ENTER/FIRE/JUMP TO GO BACK TO MENU

    void Update()
    {
        if (rankingPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Insert) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) 
                || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3") || Input.GetButtonDown("Jump"))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    // 1º) SHOW RANKING PANEL WITH RESULTS

    public void ShowRanking(string name, float time, string circuit, string character, int localPosition, bool reverse)
    {
        placeLabel.text = localPosition.ToString();
        scoreLabel.text = time + " sec";

        Ranking you = new Ranking();
        you.name = name;
        you.time = time;
        you.circuit = circuit;
        you.character = character;

        // Next step
        StartCoroutine(SaveRaking(you, reverse));
    }

    public void HideRanking()
    {
        rankingPanel.SetActive(false);
    }

    // 2º) SAVE PLAYER RANKING IN SERVER

    private IEnumerator SaveRaking(Ranking you, bool reverse)
    {
        UnityWebRequest www = new UnityWebRequest(BASE_URL + "/ranking", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(you));
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogWarning(www.error);
        }
        else
        {
            you = (Ranking)JsonUtility.FromJson(www.downloadHandler.text, typeof(Ranking));
        }

        // Next step
        StartCoroutine(GetGlobalPosition(you, reverse));
    }

    // 3º) GET GLOBAL RANKING POSITION FROM SERVER

    private IEnumerator GetGlobalPosition(Ranking you, bool reverse)
    {
        UnityWebRequest www = UnityWebRequest.Get(BASE_URL + "/ranking/" + you.id + "/position?circuit=" + you.circuit + "&reverse=" + reverse.ToString().ToLower());
        yield return www.SendWebRequest();

        int position = -1;
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogWarning(www.error);
        }
        else
        {
            RankingPosition positionObj = (RankingPosition) JsonUtility.FromJson(www.downloadHandler.text, typeof(RankingPosition));
            if (positionObj != null) position = positionObj.position;
        }

        // Next step
        StartCoroutine(GetRanking(you, position, reverse));
    }

    private class RankingPosition
    {
        public int position = -1;
    }

    // 4º) GET RANKING FROM SERVER

    private IEnumerator GetRanking(Ranking you, int globalPosition, bool reverse)
    {
        UnityWebRequest www = UnityWebRequest.Get(BASE_URL + "/ranking?size=" + tableRows.Length + "&circuit=" + you.circuit + "&reverse=" + reverse.ToString().ToLower());
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogWarning(www.error);
        }
        else
        {
            // Walkaround for unity deserializer not being able of convert a JSON array into a list
            RankingArray top10 = (RankingArray)JsonUtility.FromJson("{\"content\":" + www.downloadHandler.text + "}", typeof(RankingArray));

            // Show ranking in table
            bool currentRaceShown = false;
            for (int i = 0; i < tableRows.Length; i++)
            {
                if (i < top10.content.Length)
                {
                    SetTextInChild(tableRows[i], "position", (i + 1).ToString());
                    SetTextInChild(tableRows[i], "name", top10.content[i].name + " (" + top10.content[i].character + ")");
                    SetTextInChild(tableRows[i], "time", top10.content[i].time + "s");

                    if (top10.content[i].id.Equals(you.id))
                    {
                        currentRaceShown = true;
                        SetImageColor(tableRows[i], yourRowColor);
                    }
                    else
                    {
                        SetImageColor(tableRows[i], otherRowColor);
                    }
                }
                else
                {
                    tableRows[i].SetActive(false);
                }
            }

            // If your global position is not in the TOP, show it in the last row (replacing the content)
            if (!currentRaceShown)
            {
                GameObject row = tableRows[tableRows.Length - 1];
                SetTextInChild(row, "position", globalPosition.ToString());
                SetTextInChild(row, "name", you.name + " (" + you.character + ")");
                SetTextInChild(row, "time", you.time + "s");
                SetImageColor(row, yourRowColor);
            }
        }

        // Next step
        rankingPanel.SetActive(true);
    }

    private void SetTextInChild(GameObject parent, string name, string value)
    {
        Transform child = parent.transform.Find(name);
        if (child != null)
        {
            Text label = child.GetComponent<Text>();
            if (label != null)
            {
                label.text = value;
            }
        }
    }

    private void SetImageColor(GameObject panel, Color color)
    {
        Image img = panel.GetComponent<Image>();
        if (img != null)
        {
            img.color = color;
        }
    }
}
