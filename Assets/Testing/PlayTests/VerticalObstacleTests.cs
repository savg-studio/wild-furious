using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ObstacleTests
{
    public class VerticalObstacleTests
    {
        private const string VERTICAL_OBSTACLE_PREFAB_URI = "Prefabs/obstacle_example_2";
        private const int VERTICAL_OBSTACLE_MAX_Y = 16;
        private const int VERTICAL_OBSTACLE_MIN_Y = 0;

        private GameObject obstacle;
        private VerticalObstacle obstacleCtrl;

        [SetUp]
        public void Setup()
        {
            obstacle = GameObject.Instantiate((GameObject)Resources.Load(VERTICAL_OBSTACLE_PREFAB_URI), Vector3.zero, Quaternion.identity);

            obstacleCtrl = obstacle.GetComponent<VerticalObstacle>();
            obstacleCtrl.maxY = VERTICAL_OBSTACLE_MAX_Y;
            obstacleCtrl.minY = VERTICAL_OBSTACLE_MIN_Y;
            // TODO: Those verisables should be private (use reflection on tests??)
        }

        [UnityTest]
        public IEnumerator ShouldMoveVertically()
        {
            // SET PREVIOUS POSITION

            float prev = obstacleCtrl.minY;
            obstacle.transform.position = new Vector3(obstacle.transform.position.x, prev, obstacle.transform.position.z);

            // NEXT FRAME

            yield return null;

            // CHECK CURRENT POSITION

            Assert.Greater(obstacle.transform.position.y, prev);
        }

        [UnityTest]
        public IEnumerator ShouldGoBackVertically()
        {
            // SET PREVIOUS POSITION

            float prev = obstacleCtrl.maxY;
            obstacle.transform.position = new Vector3(obstacle.transform.position.x, prev, obstacle.transform.position.z);

            // NEXT FRAME

            yield return null;

            // CHECK CURRENT POSITION

            Assert.Less(obstacle.transform.position.y, prev);
        }
    }
}
