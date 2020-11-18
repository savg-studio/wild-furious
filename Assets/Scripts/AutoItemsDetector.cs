using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoItemsDetector : MonoBehaviour
{
    // Script for detecting items near the car to try to pick them automatically
    // This script should be placed in a child object o the CPU car (IAController) with a trigger collider
    // For the parent object not detecting these collisions, this game object should have a knimatic rigidbody (https://answers.unity.com/questions/410711/trigger-in-child-object-calls-ontriggerenter-in-pa.html) (TODO: Improve this)

    private const float MARGIN = 2f;

    private IAController car = null;

    private Coroutine selectTargetTask;

    private Transform targetItem;
    private List<Transform> goodCloseItems;
    private List<Transform> badCloseItems;

    void OnEnable()
    {
        if (transform.parent != null)
        {
            car = transform.parent.GetComponent<IAController>();
        }
        if (car == null) Debug.LogError("Car cannot be null!");

        targetItem = null;
        goodCloseItems = new List<Transform>();
        badCloseItems = new List<Transform>();

        selectTargetTask = StartCoroutine(SelectTarget());
    }

    private void OnDisable()
    {
        if (selectTargetTask != null)
        {
            StopCoroutine(selectTargetTask);
        }
    }

    void Update()
    {
        // If there is a selected target item, try to get aligned with it
        if (targetItem != null)
        {
            if (targetItem.position.y < car.transform.position.y - MARGIN)
            {
                car.verticalAxis = -1;
            }
            else if (targetItem.position.y > car.transform.position.y + MARGIN)
            {
                car.verticalAxis = +1;
            }
            else
            {
                car.verticalAxis = 0;
            }
        }
        else
        {
            car.verticalAxis = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If one item enters the trigger collider, add it to the corresponding list
        PowerUp item = collision.GetComponent<PowerUp>();
        if (item != null)
        {
            if (item is SpinachPowerUp)
            {
                goodCloseItems.Add(collision.transform);
            }
            else if (item is CannabisPowerUp || item is ObstaclePowerUp)
            {
                badCloseItems.Add(collision.transform);
            }
        }
    }

    private IEnumerator SelectTarget()
    {
        while(true)
        {
            // If item has already been passed, get new target
            if (targetItem == null || targetItem.position.x < car.transform.position.x) 
            {
                // Calculate closest item to the car
                Transform closest = null;
                float closestDistance = Mathf.Infinity;
                foreach (Transform item in goodCloseItems.ToArray())
                {
                    if (item.position.x > car.transform.position.x) 
                    {
                        float itemDistance = Vector3.Distance(car.transform.position, item.position);
                        if (itemDistance < closestDistance)
                        {
                            closest = item;
                            closestDistance = itemDistance;
                        }
                    }
                    else
                    {
                        // If item is behind us, remove it from the list
                        goodCloseItems.Remove(item); 
                    }
                }

                targetItem = closest;
            }

            // Schedule next execution
            yield return new WaitForSeconds(1.0f);
        }
    }
}
