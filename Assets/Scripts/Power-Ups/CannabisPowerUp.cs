using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannabisPowerUp : BasePowerUp
{
    [SerializeField] private int expirationInSeconds = 5;

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
            ctrl.inverted = true;
        }
    }

    protected override void OnExpired(GameObject player)
    {
        PlayerController ctrl = player.GetComponent<PlayerController>();
        if (ctrl != null)
        {
            ctrl.inverted = false;
        }
    }
}
