using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UITests
{
    public class PauseMenuTests
    {
        GameObject pauseMenu;
        PauseMenu pauseMenuCtrl;

        [SetUp]
        public void Setup()
        {
            pauseMenu = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/PauseMenu"), Vector3.zero, Quaternion.identity);
            pauseMenuCtrl = pauseMenu.GetComponent<PauseMenu>();
        }

        [Test]
        public void PauseMenuIsShown()
        {
            pauseMenuCtrl.ShowAndPause();
            Assert.IsTrue(pauseMenuCtrl.pauseMenuPanel.activeInHierarchy);
        }

        [Test]
        public void PauseMenuIsHidden()
        {
            pauseMenuCtrl.HideAndResume();
            Assert.IsFalse(pauseMenuCtrl.pauseMenuPanel.activeInHierarchy);
        }

        [Test]
        public void TimeIsFrozen()
        {
            pauseMenuCtrl.ShowAndPause();
            Assert.AreEqual(0, Time.timeScale);
        }

        [Test]
        public void TimeIsRestored()
        {
            pauseMenuCtrl.HideAndResume();
            Assert.AreEqual(1, Time.timeScale);
        }
    }
}