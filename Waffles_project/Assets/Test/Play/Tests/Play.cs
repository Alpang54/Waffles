using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.PerformanceTesting;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;

namespace Tests
{
    public class Play : MonoBehaviour
    {
       
        public static IEnumerable<string> LevelTestCases
        {
            get { return new List<string> { "StartScene","Main Menu","CharacterSel" }; }
        }

        [UnityTest]
        public IEnumerator LevelIsValid([ValueSource("LevelTestCases")] string levelName)
        {
            yield return LoadLevel(levelName);

            Assert.IsTrue(true);
        }

        [TearDown]
        public void UnloadLevel()
        {
            SceneManager.UnloadSceneAsync(sceneToUnload);
        }

        private string sceneToUnload;

        private IEnumerator LoadLevel(string levelName)
        {
            sceneToUnload = levelName;

            var loadSceneOperation = SceneManager.LoadSceneAsync(levelName);
            loadSceneOperation.allowSceneActivation = true;

            while (!loadSceneOperation.isDone)
                yield return null;
        }
        [PerformanceUnityTest]
        public IEnumerator Rendering_MainMenu()
        {
            using (Measure.Scope(new SampleGroupDefinition("Setup.LoadScene")))
            {
                SceneManager.LoadScene("Main Menu");
            }
            Debug.Log(LogType.Log.ToString());
            LogAssert.Expect(LogType.Log, "Log");
            yield return null;

            yield return Measure.Frames().Run();
        }
        [PerformanceUnityTest]
        public IEnumerator Rendering_StartScene()
        {
            using (Measure.Scope(new SampleGroupDefinition("Setup.LoadScene")))
            {
                SceneManager.LoadScene("StartScene");
            }
            Debug.Log(LogType.Log.ToString());
            LogAssert.Expect(LogType.Log, "Log");
            yield return null;

            yield return Measure.Frames().Run();
        }
        [PerformanceUnityTest]
        public IEnumerator Rendering_CharacterSel()
        {
            using (Measure.Scope(new SampleGroupDefinition("Setup.LoadScene")))
            {
                SceneManager.LoadScene("CharacterSel");
            }
            Debug.Log(LogType.Log.ToString());
            LogAssert.Expect(LogType.Log, "Log");
            yield return null;

            yield return Measure.Frames().Run();
        }
        [PerformanceUnityTest]
        public IEnumerator Rendering_CustomLobby()
        {
            using (Measure.Scope(new SampleGroupDefinition("Setup.LoadScene")))
            {
                SceneManager.LoadScene("Custom Lobby");
            }
            Debug.Log(LogType.Log.ToString());
            LogAssert.Expect(LogType.Log, "Log");
            yield return null;

            yield return Measure.Frames().Run();
        }
    }
}
