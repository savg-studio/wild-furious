using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnController : MonoBehaviour
{
    [SerializeField] public Transform spawnableZone = null;
    [SerializeField] public List<SpawnableObject> spawnableObjects = null;

    void Start()
    {
        if (spawnableZone == null || spawnableObjects == null) return;

        float difX = spawnableZone.lossyScale.x / 2;
        float minX = spawnableZone.position.x - difX;
        float maxX = spawnableZone.position.x + difX;
        float difY = spawnableZone.lossyScale.y / 2;
        float minY = spawnableZone.position.y - difY;
        float maxY = spawnableZone.position.y + difY;
        float valZ = spawnableZone.position.z;

        foreach (SpawnableObject obj in spawnableObjects)
        {
            for (int n = 0; n < obj.instances; n++)
            {
                Vector3 position = new Vector3();
                switch(obj.method)
                {
                    default:
                    case SpawnMethod.RANDOM:
                        position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), valZ);
                        break;

                    case SpawnMethod.CENTER:
                        position = new Vector3(Random.Range(minX + (difX/2), maxX - (difX/2)), Random.Range(minY, maxY), valZ);
                        break;

                    case SpawnMethod.START:
                        position = new Vector3(Random.Range(minX, maxX - difX), Random.Range(minY, maxY), valZ);
                        break;

                    case SpawnMethod.END:
                        position = new Vector3(Random.Range(minX + difX, maxX), Random.Range(minY, maxY), valZ);
                        break;
                }
                
                Instantiate(obj.prefab, position, Quaternion.identity);
            }
        }
    }

    // SPAWNABLE OBJECT

    [Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;
        public SpawnMethod method;
        public int instances;
    }

    public enum SpawnMethod
    {
        RANDOM,
        CENTER,
        START,
        END
    }
}
