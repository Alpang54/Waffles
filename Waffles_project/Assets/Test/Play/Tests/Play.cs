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
            get { return new List<string> { "StartScene","Main Menu","Main Stage","CharacterSel", "Maps", "Custom_Main", "Create_Custom", "Custom_Manage", "Custom Stage","Edit_Custom","Leaderboard","Custom Lobby"}; }
        }

        [UnityTest]
        public IEnumerator LevelIsValid([ValueSource("LevelTestCases")] string levelName)
        {
            yield return LoadLevel(levelName);

            Assert.IsTrue(true);
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
        public IEnumerator Rendering_CustomStage()
        {
            using (Measure.Scope(new SampleGroupDefinition("Setup.LoadScene")))
            {
                SceneManager.LoadScene("Custom Stage");
            }
            Debug.Log(LogType.Log.ToString());
            LogAssert.Expect(LogType.Log, "Log");
            yield return null;

            yield return Measure.Frames().Run();
        }
        [PerformanceUnityTest]
        public IEnumerator Rendering_CustomEdit()
        {
            using (Measure.Scope(new SampleGroupDefinition("Setup.LoadScene")))
            {
                SceneManager.LoadScene("Edit_Custom");
            }
            Debug.Log(LogType.Log.ToString());
            LogAssert.Expect(LogType.Log, "Log");
            yield return null;

            yield return Measure.Frames().Run();
        }
        [PerformanceUnityTest]
        public IEnumerator Rendering_CustomManage()
        {
            using (Measure.Scope(new SampleGroupDefinition("Setup.LoadScene")))
            {
                SceneManager.LoadScene("Custom_Manage");
            }
            Debug.Log(LogType.Log.ToString());
            LogAssert.Expect(LogType.Log, "Log");
            yield return null;

            yield return Measure.Frames().Run();
        }
        [PerformanceUnityTest]
        public IEnumerator Rendering_MainStage()
        {
            using (Measure.Scope(new SampleGroupDefinition("Setup.LoadScene")))
            {
                SceneManager.LoadScene("Main Stage");
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
