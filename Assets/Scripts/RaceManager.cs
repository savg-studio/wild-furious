using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    // IMPORTANT: This script must be placed inside the finish mark

    [SerializeField] private RankingController ranking = null;
    [SerializeField] private string circuitName = "UNKNOWN";

    private long startTimeMillis;
    private int localPosition;

    void Start()
    {
        // Set initial local position
        localPosition = 1;

        // Record start time of the race
        startTimeMillis = getTimeMillis();

        if (ranking == null)
        {
            GameObject rankingObj = GameObject.Find("Ranking");
            if (rankingObj != null) ranking = rankingObj.GetComponent<RankingController>();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Save end time as soon as possible in case this is the player
        long endTimeMillis = getTimeMillis();
        
        // If the object has a component "PlayerController", it's the player
        PlayerController ctrl = collision.GetComponent<PlayerController>();
        if (ctrl != null)
        {
            // Stop the car
            ctrl.acceleration = 0;
            ctrl.maxSpeed = 0;
            ctrl.speed = 0;

            // Prevent player from moving the car
            ctrl.brakes = true;

            // Open the ranking
            float time = (endTimeMillis - startTimeMillis) / 1000.0f;
            ranking.ShowRanking("Anonymus", time, circuitName, "Mario", localPosition);
        }
        else if (collision.GetComponent<IAController>() != null)
        {
            // If the collider is a CPU car, increase the local position counter
            localPosition++;
        }
    }

    private long getTimeMillis()
    {
        return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    }
}
