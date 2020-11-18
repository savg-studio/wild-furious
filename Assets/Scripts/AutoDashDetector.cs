using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDashDetector : MonoBehaviour
{
    // Script for detecting other cars to make dash automatically
    // This script should be placed in a child object o the CPU car (IAController) with a trigger collider
    // For the parent object not detecting these collisions, this game object should have a knimatic rigidbody (https://answers.unity.com/questions/410711/trigger-in-child-object-calls-ontriggerenter-in-pa.html) (TODO: Improve this)

    private IAController car = null;

    void Start()
    {
        if (transform.parent != null)
        {
            car = transform.parent.GetComponent<IAController>();
        }
        if (car == null) Debug.LogError("Car cannot be null!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            Transform posPlayer = collision.GetComponent<Transform>();
            if (this.transform.position.y > posPlayer.transform.position.y)
            {
                car.dashAbajo = true;
            }
            else
            {
                car.dashArriba = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (this.transform.position.y > -73)
        {
            car.dashAbajo = true;
        }
        else if (this.transform.position.y < -92)
        {
            car.dashArriba = true;
        }
    }
}
