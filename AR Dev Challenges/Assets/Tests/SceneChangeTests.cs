using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class SceneChangeTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void SceneChangeChecksDestination()
        {
            bool result = SceneChanger.changeScene("ThisSceneShouldnOtExist");
            Assert.AreEqual(result, false, "Changing to a non existent scene should fail - instead succeeded");
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]

        // Making sure that the scene change works and switches to another scene
        public IEnumerator SceneChangeResultsInNewScene()
        {
            Debug.Log(SceneManager.GetActiveScene().name + " " + SceneManager.sceneCount
                + " " + SceneManager.sceneCountInBuildSettings);

            string targetScene = "TestDestScene";

            Assert.AreNotEqual(SceneManager.GetActiveScene().name, targetScene, "Test is starting in the wrong scene.");
            yield return null;

            bool result = SceneChanger.changeScene(targetScene);
            Assert.AreEqual(result, true, "Attempt to change scene failed.");

            // Give a few frames for new scene to load.
            yield return null;
            yield return null;
            yield return null;

            Assert.AreEqual(SceneManager.GetActiveScene().name, targetScene, "Destination scene not reached after scene change");
        }
    }
}
