using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{   public class StageTest {

        [Test]
        public void TestProgressInitializationOfStageWhenProgressIsLesserThanContent()
        {
            List<Tuple<int, string, string>> worldStageNames= new List<Tuple<int, string, string>>();
            List<Tuple<int, string, string>> worldStageProgress = new List<Tuple<int, string, string>>();
            worldStageNames.Add(new Tuple<int,string,string>(1,"1","World1Stage1"));
            worldStageNames.Add(new Tuple<int, string, string>(1, "2", "World1Stage2"));
            worldStageProgress.Add(new Tuple<int, string, string>(1, "1", "100"));
            StageMapManagerImplementation stageMapImplementor = new StageMapManagerImplementation();
            stageMapImplementor.InitializeNoOfStagesAndUserProgress(worldStageNames, worldStageProgress, 1);
            Assert.AreEqual(1, stageMapImplementor.GetStageProgress());
        }


        [Test]
        public void TestStageCountInitializationOfStageWhenProgressIsLesserThanContent()
        {
            List<Tuple<int, string, string>> worldStageNames = new List<Tuple<int, string, string>>();
            List<Tuple<int, string, string>> worldStageProgress = new List<Tuple<int, string, string>>();
            worldStageNames.Add(new Tuple<int, string, string>(1, "1", "World1Stage1"));
            worldStageNames.Add(new Tuple<int, string, string>(1, "2", "World1Stage2"));
            worldStageProgress.Add(new Tuple<int, string, string>(1, "1", "100"));
            StageMapManagerImplementation stageMapImplementor = new StageMapManagerImplementation();
            stageMapImplementor.InitializeNoOfStagesAndUserProgress(worldStageNames, worldStageProgress, 1);
            Assert.AreEqual(2, stageMapImplementor.GetStageCount());
        }

    }
    public class WorldTest
    {

        
        // A Test behaves as an ordinary method
        [Test]
       public void TestForWorldMapButtonResultWhenBetweenNoOfWorldAndProgress()
        {
            WorldMapImplementation worldMapImplementor = new WorldMapImplementation();
            bool result=worldMapImplementor.DeclareWorldButtonsLogic(1, 3, 2);
            Assert.False(result);
        }

        [Test]
        public void TestForWorldMapButtonResultWhenLessThanProgressAndNoOfWorld()
        {
            WorldMapImplementation worldMapImplementor = new WorldMapImplementation();
            bool result = worldMapImplementor.DeclareWorldButtonsLogic(3, 5, 2);
            Assert.True(result);
        }

        [Test]
        public void TestForWorldMapButtonResultWhenLargerThanProgressAndNoOfWorld()
        {
            WorldMapImplementation worldMapImplementor = new WorldMapImplementation();
            bool result = worldMapImplementor.DeclareWorldButtonsLogic(3, 5, 8);
            Assert.False(result);
        }

        [Test]
        public void TestForExtractWorldInfoNoWorldWhenNoWorlds()
        {
            WorldMapImplementation worldMapImplementor = new WorldMapImplementation();
            worldMapImplementor.ExtractWorldInformationLogic(null);
            Assert.AreEqual(0, worldMapImplementor.GetWorldCount());
        }

        [Test]
        public void TestForExtractUserProgressWhenNoUserProgress()
        {
            WorldMapImplementation worldMapImplementor = new WorldMapImplementation();
            worldMapImplementor.ExtractUserProgressLogic(null, null);
            
            Assert.AreEqual(1, worldMapImplementor.GetWorldProgress());
        }


    }
}
