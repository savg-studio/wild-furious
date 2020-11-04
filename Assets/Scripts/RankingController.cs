using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RankingController : MonoBehaviour
{
    private const string BASE_URL = "http://server.pruebas.xoanweb.com:8081";

    [SerializeField] private GameObject rankingPanel = null;
    [SerializeField] private Text placeLabel = null;
    [SerializeField] private Text scoreLabel = null;
    [SerializeField] private GameObject[] tableRows = null;

    // HIDE RANKING PANEL ON START

    void Start()
    {
        if (rankingPanel == null || placeLabel == null || scoreLabel == null || tableRows == null)
        {
            Debug.LogError("RankingController error: rankingPanel, placeLabel, scoreLabel and tableRows cannot be null");
        }

        //rankingPanel.SetActive(false);
        StartCoroutine(GetRanking("TEST-1"));

        /*var r1 = new Ranking();
        r1.id = "yewRtu4ifsM83QbVlPEy";
        r1.circuit = "TEST-1";
        StartCoroutine(GetGlobalPosition(r1));

        var r2 = new Ranking();
        r2.name = "Test";
        r2.time = 14.5f;
        r2.character = "Mario";
        r2.circuit = "TEST-1";
        StartCoroutine(SaveRaking(r2));*/
    }

    // SHOW RANKING PANEL WITH RESULTS

    public void ShowRanking(Ranking you)
    {
        placeLabel.text = "?";
        scoreLabel.text = you.ToString() + " sec";

        rankingPanel.SetActive(true);
    }

    public void HideRanking()
    {
        rankingPanel.SetActive(false);
    }

    // GET RANKING FROM SERVER

    private IEnumerator GetRanking(string circuit)
    {
        UnityWebRequest www = UnityWebRequest.Get(BASE_URL + "/ranking?size=10&circuit=" + circuit);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogWarning(www.error);
        }

        // Walkaround for unity deserializer not being able of convert a JSON array into a list
        RankingArray top10 = (RankingArray) JsonUtility.FromJson("{\"content\":" + www.downloadHandler.text + "}", typeof(RankingArray));
        
        // Show ranking in table
        for (int i = 0; i < tableRows.Length; i++)
        {
            if (i < top10.content.Length)
            {
                SetTextInChild(tableRows[i], "position", (i + 1).ToString());
                SetTextInChild(tableRows[i], "name", top10.content[i].name);
                SetTextInChild(tableRows[i], "time", top10.content[i].time + "s");
            }
            else
            {
                tableRows[i].SetActive(false);
            }
        }
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

    // SAVE PLAYER RANKING IN SERVER

    private IEnumerator SaveRaking(Ranking entry)
    {
        UnityWebRequest www = new UnityWebRequest(BASE_URL + "/ranking", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(entry));
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogWarning(www.error);
        }

        entry = (Ranking) JsonUtility.FromJson(www.downloadHandler.text, typeof(Ranking));
    }

    // GET GLOBAL RANKING POSITION FROM SERVER

    private IEnumerator GetGlobalPosition(Ranking entry)
    {
        UnityWebRequest www = UnityWebRequest.Get(BASE_URL + "/ranking/" + entry.id + "/position?circuit=" + entry.circuit);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogWarning(www.error);
        }

        RankingPosition positionObj = (RankingPosition) JsonUtility.FromJson(www.downloadHandler.text, typeof(RankingPosition));
    }

    private class RankingPosition
    {
        public int position = -1;
    }
}
