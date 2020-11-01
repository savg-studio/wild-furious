using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalObstacle : MonoBehaviour
{
    [SerializeField] public float maxY = 16;
    [SerializeField] public float minY = -16;
    [SerializeField] public float speed = 1;

    void Update()
    {
        if (transform.position.y >= maxY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        }
        else
        {
            transform.Translate(transform.up * speed * Time.deltaTime);
        }
    }
}
