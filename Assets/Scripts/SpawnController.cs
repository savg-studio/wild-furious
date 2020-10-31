using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnController : MonoBehaviour
{
    [SerializeField] Transform spawnableZone = null;
    [SerializeField] List<SpawnableObject> spawnableObjects = null;

    void Start()
    {
        if (spawnableZone == null || spawnableObjects == null) return;

        float minX = spawnableZone.position.x - (spawnableZone.lossyScale.x / 2);
        float maxX = spawnableZone.position.x + (spawnableZone.lossyScale.x / 2);
        float minY = spawnableZone.position.y - (spawnableZone.lossyScale.y / 2);
        float maxY = spawnableZone.position.y + (spawnableZone.lossyScale.y / 2);
        float valZ = spawnableZone.position.z;

        foreach (SpawnableObject obj in spawnableObjects)
        {
            for (int n = 0; n < obj.instances; n++)
            {
                Vector3 position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), valZ);
                Instantiate(obj.prefab, position, Quaternion.identity);
            }
        }
    }

    [Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;
        public int instances;
    }
}
