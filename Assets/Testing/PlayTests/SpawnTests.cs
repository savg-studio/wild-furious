using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace SpawnTests
{
    public class SpawnerTests
    {
        // TODO: Check position Y axis

        private const string SPAWNER_NAME = "Spawner";
        private const string TEMPLATE_1_NAME = "TEMPLATE_1";
        private const string TEMPLATE_2_NAME = "TEMPLATE_2";
        private const int TEMPLATE_1_NUM = 1;
        private const int TEMPLATE_2_NUM = 4;
        private const int ZONE_SIZE = 10;

        private const string CLONE_SUFIX = "(Clone)";
        private const int ROUNDS = 10;

        private GameObject spawner;
        private SpawnController spawnCtrl;

        private void Clean()
        {
            foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
            {
                if (obj.name.Contains(TEMPLATE_1_NAME) || obj.name.Contains(TEMPLATE_2_NAME) || obj.name == SPAWNER_NAME)
                {
                    GameObject.Destroy(obj);
                }
            }
        }

        [UnityTest]
        public IEnumerator ShouldSpawnObjects()
        {
            Clean();

            // SPAWN OBJECTS

            SpawnController.SpawnableObject template1 = new SpawnController.SpawnableObject();
            template1.prefab = new GameObject(TEMPLATE_1_NAME);
            template1.instances = TEMPLATE_1_NUM;
            SpawnController.SpawnableObject template2 = new SpawnController.SpawnableObject();
            template2.prefab = new GameObject(TEMPLATE_2_NAME);
            template2.instances = TEMPLATE_2_NUM;

            GameObject zone = new GameObject();
            zone.transform.position = Vector3.zero;
            zone.transform.localScale = new Vector3(ZONE_SIZE, ZONE_SIZE, 1);

            spawner = new GameObject(SPAWNER_NAME);
            spawner.SetActive(false);
            spawnCtrl = spawner.AddComponent<SpawnController>();
            spawnCtrl.spawnableZone = zone.transform;
            spawnCtrl.spawnableObjects = new List<SpawnController.SpawnableObject>();
            spawnCtrl.spawnableObjects.Add(template1);
            spawnCtrl.spawnableObjects.Add(template2);
            spawner.SetActive(true);

            // NEXT FRAME

            yield return null;

            // CHECK IF OBJECTS WERE SPAWNED

            int existing1 = 0;
            int existing2 = 0;
            foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
            {
                if (obj.name == TEMPLATE_1_NAME + CLONE_SUFIX) existing1++;
                if (obj.name == TEMPLATE_2_NAME + CLONE_SUFIX) existing2++;
            }

            Assert.AreEqual(TEMPLATE_1_NUM, existing1);
            Assert.AreEqual(TEMPLATE_2_NUM, existing2);
        }

        [UnityTest]
        public IEnumerator ShouldSpawnObjectsRandomly()
        {
            for (int i = 0; i < ROUNDS; i++)
            {
                Clean();

                // SPAWN OBJECTS

                SpawnController.SpawnableObject template1 = new SpawnController.SpawnableObject();
                template1.prefab = new GameObject(TEMPLATE_1_NAME);
                template1.method = SpawnController.SpawnMethod.RANDOM;
                template1.instances = TEMPLATE_1_NUM;
                SpawnController.SpawnableObject template2 = new SpawnController.SpawnableObject();
                template2.prefab = new GameObject(TEMPLATE_2_NAME);
                template2.method = SpawnController.SpawnMethod.RANDOM;
                template2.instances = TEMPLATE_2_NUM;

                GameObject zone = new GameObject();
                zone.transform.position = Vector3.zero;
                zone.transform.localScale = new Vector3(ZONE_SIZE, ZONE_SIZE, 1);

                spawner = new GameObject(SPAWNER_NAME);
                spawner.SetActive(false);
                spawnCtrl = spawner.AddComponent<SpawnController>();
                spawnCtrl.spawnableZone = zone.transform;
                spawnCtrl.spawnableObjects = new List<SpawnController.SpawnableObject>();
                spawnCtrl.spawnableObjects.Add(template1);
                spawnCtrl.spawnableObjects.Add(template2);
                spawner.SetActive(true);

                // NEXT FRAME

                yield return null;

                // CHECK IF OBJECTS WERE SPAWNED

                float minAllowedX = zone.transform.position.x - (zone.transform.lossyScale.x / 2);
                float maxAllowedX = zone.transform.position.x + (zone.transform.lossyScale.x / 2);

                foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
                {
                    if (obj.name == TEMPLATE_1_NAME + CLONE_SUFIX || obj.name == TEMPLATE_2_NAME + CLONE_SUFIX)
                    {
                        Assert.GreaterOrEqual(obj.transform.position.x, minAllowedX);
                        Assert.LessOrEqual(obj.transform.position.x, maxAllowedX);
                    }
                }
            }
        }

        [UnityTest]
        public IEnumerator ShouldSpawnObjectsInCenter()
        {
            for (int i=0; i < ROUNDS; i++)
            {
                Clean();

                // SPAWN OBJECTS

                SpawnController.SpawnableObject template1 = new SpawnController.SpawnableObject();
                template1.prefab = new GameObject(TEMPLATE_1_NAME);
                template1.method = SpawnController.SpawnMethod.CENTER;
                template1.instances = TEMPLATE_1_NUM;
                SpawnController.SpawnableObject template2 = new SpawnController.SpawnableObject();
                template2.prefab = new GameObject(TEMPLATE_2_NAME);
                template2.method = SpawnController.SpawnMethod.CENTER;
                template2.instances = TEMPLATE_2_NUM;

                GameObject zone = new GameObject();
                zone.transform.position = Vector3.zero;
                zone.transform.localScale = new Vector3(ZONE_SIZE, ZONE_SIZE, 1);

                spawner = new GameObject(SPAWNER_NAME);
                spawner.SetActive(false);
                spawnCtrl = spawner.AddComponent<SpawnController>();
                spawnCtrl.spawnableZone = zone.transform;
                spawnCtrl.spawnableObjects = new List<SpawnController.SpawnableObject>();
                spawnCtrl.spawnableObjects.Add(template1);
                spawnCtrl.spawnableObjects.Add(template2);
                spawner.SetActive(true);

                // NEXT FRAME

                yield return null;

                // CHECK IF OBJECTS WERE SPAWNED

                float minAllowedX = zone.transform.position.x - (zone.transform.lossyScale.x / 4);
                float maxAllowedX = zone.transform.position.x + (zone.transform.lossyScale.x / 4);

                foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
                {
                    if (obj.name == TEMPLATE_1_NAME + CLONE_SUFIX || obj.name == TEMPLATE_2_NAME + CLONE_SUFIX)
                    {
                        Assert.GreaterOrEqual(obj.transform.position.x, minAllowedX, obj.name);
                        Assert.LessOrEqual(obj.transform.position.x, maxAllowedX, obj.name);
                    }
                }
            }
        }

        [UnityTest]
        public IEnumerator ShouldSpawnObjectsAtStart()
        {
            for (int i = 0; i < ROUNDS; i++)
            {
                Clean();

                // SPAWN OBJECTS

                SpawnController.SpawnableObject template1 = new SpawnController.SpawnableObject();
                template1.prefab = new GameObject(TEMPLATE_1_NAME);
                template1.method = SpawnController.SpawnMethod.START;
                template1.instances = TEMPLATE_1_NUM;
                SpawnController.SpawnableObject template2 = new SpawnController.SpawnableObject();
                template2.prefab = new GameObject(TEMPLATE_2_NAME);
                template2.method = SpawnController.SpawnMethod.START;
                template2.instances = TEMPLATE_2_NUM;

                GameObject zone = new GameObject();
                zone.transform.position = Vector3.zero;
                zone.transform.localScale = new Vector3(ZONE_SIZE, ZONE_SIZE, 1);

                spawner = new GameObject(SPAWNER_NAME);
                spawner.SetActive(false);
                spawnCtrl = spawner.AddComponent<SpawnController>();
                spawnCtrl.spawnableZone = zone.transform;
                spawnCtrl.spawnableObjects = new List<SpawnController.SpawnableObject>();
                spawnCtrl.spawnableObjects.Add(template1);
                spawnCtrl.spawnableObjects.Add(template2);
                spawner.SetActive(true);

                // NEXT FRAME

                yield return null;

                // CHECK IF OBJECTS WERE SPAWNED

                float minAllowedX = zone.transform.position.x - (zone.transform.lossyScale.x / 2);
                float maxAllowedX = zone.transform.position.x;

                foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
                {
                    if (obj.name == TEMPLATE_1_NAME + CLONE_SUFIX || obj.name == TEMPLATE_2_NAME + CLONE_SUFIX)
                    {
                        Assert.GreaterOrEqual(obj.transform.position.x, minAllowedX);
                        Assert.LessOrEqual(obj.transform.position.x, maxAllowedX);
                    }
                }
            }
        }

        [UnityTest]
        public IEnumerator ShouldSpawnObjectsAtEnd()
        {
            for (int i = 0; i < ROUNDS; i++)
            {
                Clean();

                // SPAWN OBJECTS

                SpawnController.SpawnableObject template1 = new SpawnController.SpawnableObject();
                template1.prefab = new GameObject(TEMPLATE_1_NAME);
                template1.method = SpawnController.SpawnMethod.END;
                template1.instances = TEMPLATE_1_NUM;
                SpawnController.SpawnableObject template2 = new SpawnController.SpawnableObject();
                template2.prefab = new GameObject(TEMPLATE_2_NAME);
                template2.method = SpawnController.SpawnMethod.END;
                template2.instances = TEMPLATE_2_NUM;

                GameObject zone = new GameObject();
                zone.transform.position = Vector3.zero;
                zone.transform.localScale = new Vector3(ZONE_SIZE, ZONE_SIZE, 1);

                spawner = new GameObject(SPAWNER_NAME);
                spawner.SetActive(false);
                spawnCtrl = spawner.AddComponent<SpawnController>();
                spawnCtrl.spawnableZone = zone.transform;
                spawnCtrl.spawnableObjects = new List<SpawnController.SpawnableObject>();
                spawnCtrl.spawnableObjects.Add(template1);
                spawnCtrl.spawnableObjects.Add(template2);
                spawner.SetActive(true);

                // NEXT FRAME

                yield return null;

                // CHECK IF OBJECTS WERE SPAWNED

                float minAllowedX = zone.transform.position.x;
                float maxAllowedX = zone.transform.position.x + (zone.transform.lossyScale.x / 2);

                foreach (GameObject obj in GameObject.FindObjectsOfType(typeof(GameObject)))
                {
                    if (obj.name == TEMPLATE_1_NAME + CLONE_SUFIX || obj.name == TEMPLATE_2_NAME + CLONE_SUFIX)
                    {
                        Assert.GreaterOrEqual(obj.transform.position.x, minAllowedX);
                        Assert.LessOrEqual(obj.transform.position.x, maxAllowedX);
                    }
                }
            }
        }
    }
}
