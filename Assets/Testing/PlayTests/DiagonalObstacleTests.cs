using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ObstacleTests
{
    public class DiagonalObstacleTests
    {
        private const string VERTICAL_OBSTACLE_PREFAB_URI = "Prefabs/diagonal_obstacle_example";
        private const int VERTICAL_OBSTACLE_MAX_Y = 8;
        private const int VERTICAL_OBSTACLE_MIN_Y = 0;

        private GameObject obstacle;
        private DiagonalObstacle obstacleCtrl;

        [SetUp]
        public void Setup()
        {
            obstacle = GameObject.Instantiate((GameObject)Resources.Load(VERTICAL_OBSTACLE_PREFAB_URI), Vector3.zero, Quaternion.identity);

            obstacleCtrl = obstacle.GetComponent<DiagonalObstacle>();
            obstacleCtrl.maxY = VERTICAL_OBSTACLE_MAX_Y;
            obstacleCtrl.minY = VERTICAL_OBSTACLE_MIN_Y;
            // TODO: Those verisables should be private (use reflection on tests??)
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(obstacle);
        }

        [UnityTest]
        public IEnumerator ShouldMoveUpRight()
        {
            // RESTART POSITION

            obstacle.transform.position = new Vector3(obstacle.transform.position.x, obstacleCtrl.maxY, obstacle.transform.position.z);
            yield return null;
            obstacle.transform.position = new Vector3(obstacle.transform.position.x, obstacleCtrl.minY, obstacle.transform.position.z);
            yield return null;

            // GET PREVIOUS POSITION

            float prevX = obstacle.transform.position.x;
            float prevY = obstacle.transform.position.y;

            // NEXT FRAME

            yield return null;

            // CHECK CURRENT POSITION

            Assert.Greater(obstacle.transform.position.x, prevX);
            Assert.Greater(obstacle.transform.position.y, prevY);
        }

        [UnityTest]
        public IEnumerator ShouldMoveDownRight()
        {
            // RESTART POSITION

            obstacle.transform.position = new Vector3(obstacle.transform.position.x, obstacleCtrl.minY, obstacle.transform.position.z);
            yield return null;
            obstacle.transform.position = new Vector3(obstacle.transform.position.x, obstacleCtrl.maxY, obstacle.transform.position.z);
            yield return null;

            // GET PREVIOUS POSITION

            float prevX = obstacle.transform.position.x;
            float prevY = obstacle.transform.position.y;

            // NEXT FRAME

            yield return null;

            // CHECK CURRENT POSITION

            Assert.Greater(obstacle.transform.position.x, prevX);
            Assert.Less(obstacle.transform.position.y, prevY);
        }
    }
}
