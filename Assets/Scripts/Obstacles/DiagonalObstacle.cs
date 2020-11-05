using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalObstacle : MonoBehaviour, Obstacle
{
    [SerializeField] public float maxY = 8;
    [SerializeField] public float minY = -8;
    [SerializeField] public float speed = 1;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, -45);
    }

    void Update()
    {
        if (transform.position.y >= maxY)
        {
            transform.rotation = Quaternion.Euler(0, 0, -135);
        }
        else if (transform.position.y <= minY)
        {
            transform.rotation = Quaternion.Euler(0, 0, -45);
        }

        transform.position += transform.up * speed * Time.deltaTime;
    }
}
