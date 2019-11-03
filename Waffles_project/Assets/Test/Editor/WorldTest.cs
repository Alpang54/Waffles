using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class StageTest
    {

         [Test]
         public void TestCorrectStageInitializeStageProgressTestWhenStageProgressIsMoreThanZeroInstances()
         {
             
             List<Tuple<int, int, string>> worldStageProgress = new List<Tuple<int, int, string>>();
            worldStageProgress.Add(new Tuple<int,int,string>(1,1,"80"));
            worldStageProgress.Add(new Tuple<int, int, string>(2, 1, "60"));
            worldStageProgress.Add(new Tuple<int, int, string>(3, 1, "50"));
            StageMapManagerImplementation stageMapImplementor = new StageMapManagerImplementation();
            List < Tuple<int, string> >result=stageMapImplementor.InitializeStageProgress(worldStageProgress, 1);
            Debug.Log(result);
             Assert.AreEqual(1, result[0].Item1);
            Assert.AreEqual("80", result[0].Item2);
        }


         [Test]
         public void TestCorrectStageInitializeStageProgressTestWheNoStageProgress()
         {
            List<Tuple<int, int, string>> worldStageProgress = new List<Tuple<int, int, string>>();
            StageMapManagerImplementation stageMapImplementor = new StageMapManagerImplementation();
            List<Tuple<int, string>> result = stageMapImplementor.InitializeStageProgress(worldStageProgress, 1);
            Assert.AreEqual(1, result[0].Item1);
            Assert.AreEqual("0", result[0].Item2);
        }


        [Test]
        public void TestCorrectStageInitializeStageProgressTestWhenStageProgressIsUnOrdered()
        {
            List<Tuple<int, int, string>> worldStageProgress = new List<Tuple<int, int, string>>();
            StageMapManagerImplementation stageMapImplementor = new StageMapManagerImplementation();
            worldStageProgress.Add(new Tuple<int, int, string>(1, 2, "80"));
            worldStageProgress.Add(new Tuple<int, int, string>(1, 1, "60"));
            worldStageProgress.Add(new Tuple<int, int, string>(2, 1, "50"));
            List<Tuple<int, string>> result = stageMapImplementor.InitializeStageProgress(worldStageProgress, 1);
            Assert.AreEqual(1, result[0].Item1);
            Assert.AreEqual("60", result[0].Item2);
            Assert.AreEqual(2, result[1].Item1);
            Assert.AreEqual("80", result[1].Item2);
        }

        [Test]
        public void TestCorrectStageInitializeStageNamesTestWhenStageNamesIsMoreThanZeroInstances()
        {
            List<Tuple<int, int, string>> worldStageNames = new List<Tuple<int, int, string>>();
            StageMapManagerImplementation stageMapImplementor = new StageMapManagerImplementation();
            worldStageNames.Add(new Tuple<int, int, string>(1, 1, "Stage1"));
            worldStageNames.Add(new Tuple<int, int, string>(1, 2, "Stage2"));
            worldStageNames.Add(new Tuple<int, int, string>(2, 1, "Stage3"));
            List<Tuple<int, string>> result = stageMapImplementor.InitializeStageNames(worldStageNames, 1);
            Assert.AreEqual(1, result[0].Item1);
            Assert.AreEqual("Stage1", result[0].Item2);
            Assert.AreEqual(2, result[1].Item1);
            Assert.AreEqual("Stage2", result[1].Item2);
        }

        [Test]
        public void TestCorrectStageInitializeStageNamesTestWhenStageNamesAreUnordered()
        {
            List<Tuple<int, int, string>> worldStageNames = new List<Tuple<int, int, string>>();
            worldStageNames.Add(new Tuple<int, int, string>(2, 1, "Stage1"));
            worldStageNames.Add(new Tuple<int, int, string>(1, 2, "Stage2"));
            worldStageNames.Add(new Tuple<int, int, string>(1, 1, "Stage3"));
            StageMapManagerImplementation stageMapImplementor = new StageMapManagerImplementation();
            List<Tuple<int, string>> result = stageMapImplementor.InitializeStageNames(worldStageNames, 1);
            Assert.AreEqual(1, result[0].Item1);
            Assert.AreEqual("Stage3", result[0].Item2);
            Assert.AreEqual(2, result[1].Item1);
            Assert.AreEqual("Stage2", result[1].Item2);
           
        }

        [Test]
        public void TestComputeStageNumberWhenAllValid()
        {
           
            StageMapManagerImplementation stageMapImplementor = new StageMapManagerImplementation();
            int stageNumber= stageMapImplementor.ComputeStageNumber(1,9.0,0);
            Assert.AreEqual(1,stageNumber);

        }
        [Test]
        public void TestComputeStageNumberWhenpageNumberisInValid()
        {

            StageMapManagerImplementation stageMapImplementor = new StageMapManagerImplementation();
            int stageNumber = stageMapImplementor.ComputeStageNumber(0, 9.0, 0);
            Assert.AreEqual(1, stageNumber);

        }

        [Test]
        public void TestComputeStageNumberWhennoOfStagesPerPageisInValid()
        {

            StageMapManagerImplementation stageMapImplementor = new StageMapManagerImplementation();
            int stageNumber = stageMapImplementor.ComputeStageNumber(1, 0, 0);
            Assert.AreEqual(1, stageNumber);

        }
        [Test]
        public void TestComputeStageNumberWhenCounterIsInValid()
        {

            StageMapManagerImplementation stageMapImplementor = new StageMapManagerImplementation();
            int stageNumber = stageMapImplementor.ComputeStageNumber(1, 9.0, -1);
            Assert.AreEqual(1, stageNumber);

        }
        [Test]
        public void TestDeclareStageMapWhenButtonNumberIsLessThanProgressAndAllowedStages()
        {

            StageMapManagerImplementation stageMapImplementor = new StageMapManagerImplementation();
            int x = stageMapImplementor.DeclareStageMapButton(1, 3, 1, 0);
            Assert.AreEqual(1, x);

        }

        [Test]
        public void TestDeclareStageMapWhenButtonNumberIsBetweenProgressAndAllowedStages()
        {

            StageMapManagerImplementation stageMapImplementor = new StageMapManagerImplementation();
            int x = stageMapImplementor.DeclareStageMapButton(1, 4, 1, 2);
            Assert.AreEqual(2, x);

        }

        [Test]
        public void TestDeclareStageMapWhenButtonNumberIsLargerThanProgressAndAllowedStages()
        {

            StageMapManagerImplementation stageMapImplementor = new StageMapManagerImplementation();
            int x = stageMapImplementor.DeclareStageMapButton(1, 4, 1, 5);
            Assert.AreEqual(0, x);

        }

    }
    public class WorldTest
    {


        
       [Test]
       public void TestForWorldMapButtonResultWhenBetweenNoOfWorldAndProgress()
        {
            WorldMapImplementation worldMapImplementor = new WorldMapImplementation();
            int result=worldMapImplementor.DeclareWorldButtonsLogic(1, 4, 1,2);
            Assert.AreEqual(2, result);
        }

        [Test]
        public void TestForWorldMapButtonResultWhenLessThanProgressAndNoOfWorld()
        {
           WorldMapImplementation worldMapImplementor = new WorldMapImplementation();
            int result=worldMapImplementor.DeclareWorldButtonsLogic(3, 4, 1,2);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void TestForWorldMapButtonResultWhenLargerThanProgressAndNoOfWorld()
        {
            WorldMapImplementation worldMapImplementor = new WorldMapImplementation();
            int result = worldMapImplementor.DeclareWorldButtonsLogic(1, 4, 1, 5);
            Assert.AreEqual(0, result);
        }
        
        [Test]
        public void TestForProcessWorldInformation()
        {
            List<Tuple<int, string>> worldNames = new List<Tuple<int, string>>();
            Tuple<int, string> x = new Tuple<int, string>(1, "world1");
            Tuple<int, string> y = new Tuple<int, string>(2, "world2");
            worldNames.Add(x);
            worldNames.Add(y);
            WorldMapImplementation worldMapImplementor = new WorldMapImplementation();
            worldMapImplementor.ProcessWorldInformation(worldNames);
            Assert.AreEqual(2, worldMapImplementor.GetWorldCount());
        }

        [Test]
        public void TestForProcessUserProgressLogicWhenWorldCountIsLargerThanProgress()
        {

            List<Tuple<int,int, string>> worldStageProgress = new List<Tuple<int,int, string>>();
            Tuple<int,int, string> x = new Tuple<int,int, string>(1,1, "80");
            Tuple<int,int, string> y = new Tuple<int,int, string>(1,2, "100");
            worldStageProgress.Add(x);
            worldStageProgress.Add(y);
            WorldMapImplementation worldMapImplementor = new WorldMapImplementation();
            worldMapImplementor.SetWorldCount(3);
            worldMapImplementor.ProcessUserProgressLogic(worldStageProgress);
            Assert.AreEqual(1, worldMapImplementor.GetWorldProgress());
        }


        [Test]
        public void TestForProcessUserProgressLogicWhenWorldCountIsLesserThanProgress()
        {

            List<Tuple<int, int, string>> worldStageProgress = new List<Tuple<int, int, string>>();
            Tuple<int, int, string> x = new Tuple<int, int, string>(1, 1, "80");
            Tuple<int, int, string> y = new Tuple<int, int, string>(2, 1, "100");
            worldStageProgress.Add(x);
            worldStageProgress.Add(y);
            WorldMapImplementation worldMapImplementor = new WorldMapImplementation();
            worldMapImplementor.SetWorldCount(1);
            worldMapImplementor.ProcessUserProgressLogic(worldStageProgress);
            Assert.AreEqual(1, worldMapImplementor.GetWorldProgress());
        }


    }
}
