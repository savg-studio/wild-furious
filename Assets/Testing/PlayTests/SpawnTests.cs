using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace SpawnTests
{
    public class SpawnerTests
    {
        private const string TEMPLATE_1_NAME = "TEMPLATE_1";
        private const string TEMPLATE_2_NAME = "TEMPLATE_2";
        private const int TEMPLATE_1_NUM = 1;
        private const int TEMPLATE_2_NUM = 4;

        private GameObject spawner;
        private SpawnController spawnCtrl;

        [UnityTest]
        public IEnumerator ShouldSpawnObjects()
        {
            // SPAWN OBJECTS

            SpawnController.SpawnableObject template1 = new SpawnController.SpawnableObject();
            template1.prefab = new GameObject(TEMPLATE_1_NAME);
            template1.instances = TEMPLATE_1_NUM;
            SpawnController.SpawnableObject template2 = new SpawnController.SpawnableObject();
            template2.prefab = new GameObject(TEMPLATE_2_NAME);
            template2.instances = TEMPLATE_2_NUM;

            GameObject zone = new GameObject();
            zone.transform.localScale = new Vector3(1000, 1000, 1);

            spawner = new GameObject("Spawner");
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
                if (obj.name.Contains(TEMPLATE_1_NAME)) existing1++;
                if (obj.name.Contains(TEMPLATE_2_NAME)) existing2++;
            }

            Assert.AreEqual(TEMPLATE_1_NUM + 1, existing1);
            Assert.AreEqual(TEMPLATE_2_NUM + 1, existing2);
        }
    }
}
