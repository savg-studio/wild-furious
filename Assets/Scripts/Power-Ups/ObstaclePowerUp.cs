using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePowerUp : BasePowerUp
{
    [SerializeField] private int speedDivider = 4;

    // POWER-UP FUNCTIONS

    protected override void OnActivated(GameObject player)
    {
        PlayerController ctrl = player.GetComponent<PlayerController>();
        if (ctrl != null)
        {
            ctrl.speed /= speedDivider;
        }
    }
}
