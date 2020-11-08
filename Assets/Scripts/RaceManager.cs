using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    // IMPORTANT: This script must be placed inside the finish mark

    private const string DEFAULT_CIRCUIT = "UNKNOWN";
    private const string DEFAULT_CHARACTER = "Mario";
    private const string DEFAULT_PLAYER = "Anonymus";
    private const bool DEFAULT_REVERSE = false;

    [SerializeField]
    [Header("Number of Players+CPUs")]
    [Tooltip("How many cars are in the circuit? This is used in the special mode to calculate the reverse local position")]
    private int numberOfCars = 3;

    [SerializeField] private RankingController ranking = null;
    [SerializeField] private GameObject pauseMenu = null;

    private RaceInfo raceInfo = null;

    private long startTimeMillis;
    private int localPosition;

    void Start()
    {
        // Set initial local position
        localPosition = 1;

        // Record start time of the race
        startTimeMillis = getTimeMillis();

        // Get ranking controller reference
        if (ranking == null)
        {
            GameObject rankingObj = GameObject.Find("Ranking");
            if (rankingObj != null) ranking = rankingObj.GetComponent<RankingController>();
        }

        // Get race info reference
        if (raceInfo == null)
        {
            GameObject dataObj = GameObject.Find(SelectMenu.DATA_GAMEOBJECT_NAME);
            if (dataObj != null) raceInfo = dataObj.GetComponent<RaceInfo>();
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
            ctrl.verticalSpeed = 0;
            ctrl.dashSpeed = 0;

            // Calculate time
            float time = (endTimeMillis - startTimeMillis) / 1000.0f;

            // Disable pause menu so it doesn't interfeer with the ranking view
            DisablePauseMenu();

            // Open the ranking
            if (raceInfo != null)
            {
                ranking.ShowRanking(raceInfo.playerName, time, raceInfo.circuit, raceInfo.character, raceInfo.reverse ? numberOfCars - localPosition + 1 : localPosition, raceInfo.reverse);
            }
            else
            {
                ranking.ShowRanking(DEFAULT_PLAYER, time, DEFAULT_CIRCUIT, DEFAULT_CHARACTER, DEFAULT_REVERSE ? numberOfCars - localPosition + 1 : localPosition, DEFAULT_REVERSE);
            }
        }
        else if (collision.GetComponent<IAController>() != null)
        {
            // Check if the object is a CPU car
            IAController cpu = collision.GetComponent<IAController>();
            if (cpu != null)
            {
                // Stop the car
                cpu.speed = 0;
                
                // If the collider is a CPU car, increase the local position counter
                localPosition++;
            }
        }
    }

    private long getTimeMillis()
    {
        return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    }

    private void DisablePauseMenu()
    {
        if (pauseMenu == null) pauseMenu = GameObject.Find("PauseMenu");
        if (pauseMenu != null) pauseMenu.SetActive(false);
    }
}
