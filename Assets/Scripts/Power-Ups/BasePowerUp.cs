using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePowerUp : MonoBehaviour, PowerUp
{
    private Collider2D trigger = null;

    void Start()
    {
        // Add collider trigger if not pressent
        trigger = gameObject.GetComponent<Collider2D>();
        if (trigger == null) trigger = gameObject.AddComponent<BoxCollider2D>();
        if (trigger != null) trigger.isTrigger = true;
    }

    public void OnCollected(GameObject player)
    {
        // Schedule expiration listener
        int expirationInSeconds = GetExpirationInSeconds();
        if (expirationInSeconds > 0)
        {
            StartCoroutine(ScheduleExpiration(player, expirationInSeconds));
        }

        // Hide power-up icon
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        if (renderer != null) renderer.enabled = false;

        // Disable trigger
        if (trigger != null)
        {
            trigger.enabled = false;
        }

        // Call activated listener
        OnActivated(player);
    }

    private IEnumerator ScheduleExpiration(GameObject player, int expirationInSeconds)
    {
        // Wait for X seconds and then call expired listener
        yield return new WaitForSeconds(expirationInSeconds);
        OnExpired(player);
    }

    // FUNCTIONS TO BE IMPLEMENTED BY THE POWER-UP

    protected abstract void OnActivated(GameObject player);
    protected virtual void OnExpired(GameObject player) { }
    protected virtual int GetExpirationInSeconds() { return -1;  }
}
