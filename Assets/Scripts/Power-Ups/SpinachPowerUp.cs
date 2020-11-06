using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinachPowerUp : BasePowerUp
{
    [SerializeField] private int expirationInSeconds = 5;
    [SerializeField] private int speedMultiplier = 2;
    [SerializeField] private int accelerationMultiplier = 4;

    // POWER-UP FUNCTIONS

    protected override int GetExpirationInSeconds()
    {
        return expirationInSeconds;
    }

    protected override void OnActivated(GameObject player)
    {
        PlayerController ctrl = player.GetComponent<PlayerController>();
        if (ctrl != null)
        {
            ctrl.maxSpeed *= speedMultiplier;
            ctrl.acceleration *= accelerationMultiplier;
        }
    }

    protected override void OnExpired(GameObject player)
    {
        PlayerController ctrl = player.GetComponent<PlayerController>();
        if (ctrl != null)
        {
            ctrl.maxSpeed /= speedMultiplier;
            ctrl.acceleration /= accelerationMultiplier;

            if (ctrl.speed > ctrl.maxSpeed) ctrl.speed = ctrl.maxSpeed;
        }
    }
}
