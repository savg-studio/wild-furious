using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePowerUp : MonoBehaviour, PowerUp
{
    void Start()
    {
        // Add collider trigger if not pressent
        Collider2D collider = gameObject.GetComponent<Collider2D>();
        if (collider == null) collider = gameObject.AddComponent<BoxCollider2D>();
        if (collider != null) collider.isTrigger = true;
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
    protected abstract void OnExpired(GameObject player);
    protected abstract int GetExpirationInSeconds();
}
