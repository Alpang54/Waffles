﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;


/** WorldMapManagerScript manages the world map logic on the application side regarding GameObjects
* @author Ang Jie Kai Alvis
**/
public class WorldMapManagerScript : MonoBehaviour
{
    [SerializeField] public static int worldSelected;

    public Sprite activeSprite;
    public Sprite lockedSprite;

    const double noOfWorldPerPage = 9.0;

    private int worldCount;
    private int worldProgress;
    private int pageNumber;

    private List<Tuple<int,string>> worldNames;

    private List<Tuple<int,int,string>> worldStageNames;
    private List<Tuple<int, int, string>> worldStageProgress;

    private List<Tuple<int, int>> worldCompletionPercentage;
    [SerializeField] private GameObject LeftButton;
    [SerializeField] private GameObject RightButton;


    public Text loadText;
    public GameObject[] worldMapButtons;
    public GameObject worldConfirmPanel;
    public GameObject worldSelect;
    public GameObject stageMapManager;



    WorldMapImplementation worldMapImplementor;
    private DataHandler datahandler;


    //Load world data from database and initialize the map
    async void Start()
    {

        datahandler = GameObject.Find("DataManager").GetComponent<DataHandler>();
        
        this.pageNumber = 1;
        worldMapImplementor = new WorldMapImplementation();

        await GetWorldAndUserProgressFromDatabase();
        GameObject loadingCircle = GameObject.Find("LoadingCircle");
        loadingCircle.SetActive(false);

        UpdateLeftRightButtons();




    }


    /**
     *Gets all the relevant information needed from the database, then calls processing methods to process the information
     **/
    private async Task GetWorldAndUserProgressFromDatabase()
    {
        GameObject DBHandler = GameObject.Find("DBHandler");
        DBHandler DBHandlerScript = DBHandler.GetComponent<DBHandler>();


        //Read in names of stages for every world
        await DBHandlerScript.ReadfromFirebase("World");
        DataSnapshot SnapshotOfWorld = DBHandlerScript.GetSnapshot();
      
        //Set List of WorldStageNames and list of WorldNames 
        ExtractWorldInformation(SnapshotOfWorld);

        //Set World Count
        ProcessWorldInformation(this.worldNames);

        //Read in World Progress 
        await DBHandlerScript.ReadfromFirebase("Progress");
        DataSnapshot snapshotOfUserProgress = DBHandlerScript.GetSnapshot();

        //Set WorldStageProgress
        ExtractUserWorldProgress(snapshotOfUserProgress);

        //Set WorldProgress
        ProcessUserProgessLogic(this.worldStageProgress);
        
        //Compute an array of completion Percentages for all of the worlds
        this.worldCompletionPercentage=worldMapImplementor.ComputeWorldCompletionPercentage(this.worldStageProgress, this.worldStageNames, this.worldCount);

        //Determine if user is able to proceed to next world
        DetermineIfUserWorldProgressShouldBeUpdated();
        
        //Determines which button to be active or not
        DeclareWorldMapButtons(worldProgress, worldCount,this.pageNumber);

    }


    /**
     *Updates the world buttons if previous world has over 70% completion
     **/
    private void DetermineIfUserWorldProgressShouldBeUpdated()
    {
        int temporaryworldCompletionPercentage = this.worldCompletionPercentage[worldProgress-1].Item2;

        if (temporaryworldCompletionPercentage >= 70&& this.worldProgress<this.worldCount)
        {
            this.worldProgress++;
            Tuple<int, int> aRecordOfWorldCompletion = new Tuple<int, int>(worldProgress, 0);
            worldCompletionPercentage.Add(aRecordOfWorldCompletion);
            DeclareWorldMapButtons(worldProgress, worldCount, this.pageNumber);
        }
    }



    /**
     *Sets the world values without considering user progress
     * @param snapShotOfWorld is a json parse tree obtained from the database
     **/
    private void ExtractWorldInformation(DataSnapshot snapShotOfWorld)
    {
        this.worldStageNames = worldMapImplementor.ExtractWorldInformationLogic(snapShotOfWorld);
        this.worldNames = worldMapImplementor.GetWorldNames();
    }

    /**Sets world Count without considering user
     * @params worldNames is a list of tuples that contains the names of the worlds
     * */
    private void ProcessWorldInformation(List<Tuple<int, string>> worldNames)
    {
        worldMapImplementor.ProcessWorldInformation(worldNames);
        this.worldCount = worldMapImplementor.GetWorldCount();      
    }


    /**Obtains user progress
     * @params snapShotOfUserProgress is a json parse tree obtained from the database
     * */
    private void ExtractUserWorldProgress(DataSnapshot snapShotOfUserProgress)
    {
        string userID = datahandler.GetFirebaseUserId();
        this.worldStageProgress = worldMapImplementor.ExtractUserProgressLogic(snapShotOfUserProgress, userID);
    }

    /**Processes user progress information in order to determine which world can be played
     *@params worldStageProgress is a list of tuples that contains the world,the stages of the world, and the user progress in that stage
     * */
    private void ProcessUserProgessLogic(List<Tuple<int, int, string>> worldStageProgress)
    {
        worldMapImplementor.ProcessUserProgressLogic(worldStageProgress);
        this.worldProgress = worldMapImplementor.GetWorldProgress();
       
    }

    /**determine which buttons are to be active or which to be inactive
     * @params worldProgress is the world the user has unlocked until, worldCount is the total number of worlds, pageNumber is the page of the world
     * */
    public void DeclareWorldMapButtons(int worldProgress, int worldCount,int pageNumber)
    {
        for (int i = 0; i < worldMapButtons.Length; i++)
        {

            Debug.Log("i is " + i);

            Debug.Log("worldProgress is " + worldProgress);
            Debug.Log("worldCount is " + worldCount);

            WorldMapButtonScript aWorldButtonScript = worldMapButtons[i].GetComponent<WorldMapButtonScript>();
            int result = worldMapImplementor.DeclareWorldButtonsLogic(worldProgress, worldCount, pageNumber, i);
            Debug.Log("result is " + result);

            if (result==1)
            {
                aWorldButtonScript.SetWorldButton(true);
                int worldLevelForAButton = worldMapImplementor.ComputeWorldNumber(pageNumber, noOfWorldPerPage, i);
  
                worldMapButtons[i].GetComponent<Button>().interactable = true;
                aWorldButtonScript.SetWorldButtonImage(activeSprite);
                aWorldButtonScript.SetWorldLevel(worldLevelForAButton);
                aWorldButtonScript.SetText(worldLevelForAButton.ToString());
               
         
            }
            else if(result == 2)
            {
                aWorldButtonScript.SetWorldButton(false);
                aWorldButtonScript.SetWorldButtonImage(lockedSprite);
                aWorldButtonScript.SetText("");

            }

            else
            {
                aWorldButtonScript.SetWorldButton(true);
                worldMapButtons[i].GetComponent<Button>().interactable = false;
                aWorldButtonScript.SetWorldButtonImage(lockedSprite);
                aWorldButtonScript.SetText("");

            }

        }
    }


   
    /**when a world is selected, show confirm panel
    * @params worldLevel is the world level of the button selected
    * */
    public void OnSelectWorldButton(int worldLevel)
    {

        WorldConfirmPanel confirmPanel = worldConfirmPanel.GetComponent<WorldConfirmPanel>();
        int selectedWorldCompletionPercent = 0;
        foreach(var aRecordOfWorldCompletionPercentage in this.worldCompletionPercentage)
        {
            if (worldLevel == aRecordOfWorldCompletionPercentage.Item1)
            {
                selectedWorldCompletionPercent = aRecordOfWorldCompletionPercentage.Item2;
            }
        }

        confirmPanel.confirmPanelAppear(this.worldNames[worldLevel-1].Item2, worldLevel,selectedWorldCompletionPercent);
    }

    /** when a world is selected and confirmed, pass to stage manager to load stage map and turn world map off
     *   * @params worldLevel is the world level of the button selected
    * */
    public void OnSelectWorldProceedButton(int worldLevel)
    {
       
        StageMapManagerScript stageManager = stageMapManager.GetComponent<StageMapManagerScript>();
        stageManager.LoadStageMap(worldLevel,this.worldStageNames,this.worldStageProgress);
        worldSelect.SetActive(false);

    }

    /** turns the world map off
    * */
    public void WorldSelectDisappear()
    {
        worldSelect.SetActive(false);
    }

    /** turns the world map on
    * */
    public void WorldSelectAppear()
    {
        worldSelect.SetActive(true);
    }

    /** returns the worldStageNames value which holds world, stage , and the name of the stage in a tuple, for an entire list
    * */
    public List<Tuple<int, int, string>> GetWorldStageNames()
    {
        return this.worldStageNames;
    }

    /** Sets the new world stage names value
     * @params newWorldStageNames 
    * */
    public void SetWorldStageNames(List<Tuple<int, int, string>> newWorldStageNames)
    {
        this.worldStageNames=newWorldStageNames;
    }

    /** Decides if left or/and right buttons appear to navigate between pages of the world map
    * */
    private void UpdateLeftRightButtons()
    {
        double noOfAcceptablePages = this.worldCount / noOfWorldPerPage;

        if (noOfAcceptablePages > this.pageNumber)
        {
            RightButton.SetActive(true);

        }
        else
        {
            RightButton.SetActive(false);
        }

        if (this.pageNumber <= 1)
        {
            LeftButton.SetActive(false);
        }
        else {
            LeftButton.SetActive(true); }
        DeclareWorldMapButtons(this.worldProgress, this.worldCount, this.pageNumber);
    }

    /** Increments the page number when the button is clicked and calls update the button method
  * */
    public void onSelectNextButton()

    {
        this.pageNumber++;
        UpdateLeftRightButtons();

    }

    /** Decrements the page number when the button is clicked and calls update the button method
  * */
    public void onSelectPreviousButton()
    {
        this.pageNumber--;
        UpdateLeftRightButtons();
    }

}



/** WorldMapImplementation manages the world map logic on the logic side which is used in worldMapManagerScript
* @author Ang Jie Kai Alvis
**/
public class WorldMapImplementation
{
    private int worldCount;
    private List<Tuple<int,string>> worldNames;
    private int worldProgress;

    /**Logic to determine the if a button should be declared or not.
     * @ params worldProgress is the user progress in the world, worldCount is the number of worlds available, pageNumber is the page of the world map, i is i'th world button in the current world map
     * */
    public int DeclareWorldButtonsLogic(int worldProgress, int worldCount, int pageNumber, int i)
    {


        i = i + 9 * (pageNumber-1)+1;


        if (i <= worldProgress && i <= worldCount)
        {
            return 1;
        }

        else if (i <= worldCount && i > worldProgress)
        {
          
            return 2;
        }
        else
        {
            return 0;
        }
    }

    /** Returns worldStageNames in ascending order in terms of worldNumber
     * @params snapShotOfWorld is a json parse tree from the firebase containing the snapshot of all available world
     * */
    public List<Tuple<int, int, string>>  ExtractWorldInformationLogic(DataSnapshot snapShotOfWorld)
    {
        List<Tuple<int,string>>worldNames = new List<Tuple<int,string>>();
        char[] world_seperator = { '-' };

        List<Tuple<int, int, string>> worldStageNames = new List<Tuple<int, int, string>>();

        //For each input in the Root World
        foreach (var world in snapShotOfWorld.Children)
        {
            //Add the world name into the list of WorldNames
            

            // To obtain world number
            String[] world_key_split = world.Key.Split(world_seperator);
            int worldNumber = Int32.Parse(world_key_split[0]);

            //Add the world name into the list of WorldNames
            Tuple<int, string> aRecordOfWorldNames = new Tuple<int, string>(worldNumber, world_key_split[1]);
            worldNames.Add(aRecordOfWorldNames);

            //Check if world is empty or not
            //If world is not empty, add every stage and its name to the worldStageNames 
            if (world.HasChild("Stage1"))
            {
               
                foreach (var stage in world.Children)
                {
                    int stageInAWorld = Int32.Parse(stage.Key.ToString().Substring(5));
                    Tuple<int, int, string> aRecordOfworldStageNames = new Tuple<int, int, string>(worldNumber, stageInAWorld, stage.Value.ToString());
                    worldStageNames.Add(aRecordOfworldStageNames);  
                }
            }

            //If empty, add Tuple <worldNumber,0, " "> to the list of worldStageNames
            else {
                Tuple<int, int, string> aRecordOfEmptyworldStageNames = new Tuple<int, int, string>(worldNumber, 0, "Empty");
                worldStageNames.Add(aRecordOfEmptyworldStageNames);
            }
        }

        //Next, we reorder the worldStageNames according to the worldNumber
        int loop = 1;
        int terminate = worldStageNames.Count;
        while (true)
        {
            if (loop >= terminate)
            {
                break;
            }
            else if(worldStageNames[loop-1].Item1 > worldStageNames[loop].Item1)
            {
                Tuple<int, int, string> temp = worldStageNames[loop];
                worldStageNames[loop] = worldStageNames[loop - 1];
                worldStageNames[loop - 1] = temp;
                loop = 1;
            }
            else { loop++; }
        }

        //Next, reorder the worldNames
        loop = 1;
        terminate = worldNames.Count;
        while (true)
        {
            if (loop == terminate)
            {
                break;
            }
            else if (worldNames[loop - 1].Item1 > worldNames[loop].Item1)
            {
                Tuple<int,string> temp = worldNames[loop];
                worldNames[loop] = worldNames[loop - 1];
                worldNames[loop - 1] = temp;
                loop = 1;
            }
            else { loop++; }
        }

        //Set World Names and return worldStageNames
        this.worldNames = worldNames;
        return worldStageNames;
      
    }

    /**Set Number of Worlds
     * @params worldNames is a list of tuple containing the list of the world names
     * */
    public void ProcessWorldInformation(List<Tuple<int, string>> worldNames)
    {
        int worldCount = 0;
        foreach(var aRecordOfWorldStageNames in worldNames)
        {
            worldCount=aRecordOfWorldStageNames.Item1;
        }
        this.worldCount = worldCount;
    }



    /**returns UserProgress in ascending order in terms of worldNumber
     * @params snapShotOfUserProgress is a json parse tree from the database which contains the user progress in the world map, userID is the ID of the user
     * */
    public List<Tuple<int, int, string>> ExtractUserProgressLogic(DataSnapshot snapShotOfUserProgress, string userID)
    {
        int worldProgress;
        int stageProgressInAWorld;
        List<Tuple<int, int, string>> worldStageProgress = new List<Tuple<int, int, string>>();

        
        foreach (var userid in snapShotOfUserProgress.Children)
        {
            //Checks for current user
            if (userid.Key.ToString() == userID)
            {
                //For the current user, get the world and stage progress.
                foreach (var world in userid.Children)
                {

                    worldProgress = Int32.Parse(world.Key.ToString().Substring(5));

                    foreach (var stage in world.Children)
                    {
                        stageProgressInAWorld = Int32.Parse(stage.Key.ToString().Substring(5));
                        Tuple<int, int, string> aRecordOfWorldStageProgress = new Tuple<int, int, string>(worldProgress, stageProgressInAWorld, stage.Value.ToString());
                        worldStageProgress.Add(aRecordOfWorldStageProgress);
                    }

                }
                break;
            }

        }

        //Checks if user has played the game before. If not, insert a record of worldStageProgress at world 1, stage 1.
        if (worldStageProgress.Count == 0)
        {
            Tuple<int, int, string> aRecordOfWorldStageProgress = new Tuple<int, int, string>(1, 1,"0");
            worldStageProgress.Add(aRecordOfWorldStageProgress);
        }

        //else simply reorder the list in an ascending order with respect to the worldNumber
        else
        {
            int loop = 1;
            int terminate = worldStageProgress.Count;
            while (true)
            {
                if (loop >= terminate)
                {
                    break;
                }
                else if (worldStageProgress[loop - 1].Item1 > worldStageProgress[loop].Item1)
                {
                    Tuple<int, int, string> temp = worldStageProgress[loop];
                    worldStageProgress[loop] = worldStageProgress[loop - 1];
                    worldStageProgress[loop - 1] = temp;
                    loop = 1;
                }
                else { loop++; }
            }
        }

        return worldStageProgress;
    }


    /** Process WorldProgress for user
     * @params worldStageProgress is a tuple containing a world, a stage and the user stage progress, wrapped in a list
     * */
    public void ProcessUserProgressLogic(List<Tuple<int, int, string>> worldStageProgress)
    {
        int worldProgress=1;

        //Get the highest worldProgress in the worldStageProgress as it is an ordered list
        foreach (var aRecordOfWorldStageProgress in worldStageProgress)
        {
            worldProgress = aRecordOfWorldStageProgress.Item1;
        }

        //Check if worldProgress is higher than total number of worlds, if so, lower the progress
        if (worldProgress > this.worldCount)
        {
            this.worldProgress = worldCount;
        }
        else
        {
            this.worldProgress = worldProgress;
        }
    }


    /** Compute the world number for buttons in different pages
     * @params pageNumber is the current page the user is on, noOfWorldperPage is a constant which specifies how many buttons there are on a page, i is the i'th button on the page
     * */
    public int ComputeWorldNumber(int pageNumber, double noOfWorldPerPage, int i)
    {
        int worldNumberText = (int) ((pageNumber-1) * noOfWorldPerPage + i + 1);
        return worldNumberText;
    }




    /** Returns an array of world Completion percentage for the user.
 * @params worldStageProgress is a list of tuples containing a world, a stage , and the user stage progress, worldStageNames is a list of tuples containing a world, a stage , and the names of the stage, worldCount is the total number of worlds
 * */
    public List<Tuple<int,int>> ComputeWorldCompletionPercentage(List<Tuple<int, int, string>> worldStageProgress, List<Tuple<int, int, string>> worldStageNames,int worldCount)
    {
        List<Tuple<int, int>> worldCompletionPercentage = new List<Tuple<int, int>>();
        int completionPercentage;
        
        for (int i = 1; i <= worldCount; i++)
        {
            int totalNoOfStages = 0;
            int totalScore = 0;


            foreach (var aRecordOfWorldStageNames in worldStageNames)
            {
                
                if (i == aRecordOfWorldStageNames.Item1)
                {
                    totalNoOfStages++;
                }
            }

            foreach (var aRecordOfWorldStageProgress in worldStageProgress) {
                if (i == aRecordOfWorldStageProgress.Item1)
                { 
                    totalScore += Int32.Parse(aRecordOfWorldStageProgress.Item3);
                }
            }
            if (totalNoOfStages != 0)
            {
                completionPercentage = totalScore / totalNoOfStages;
            }
            else
            {
                completionPercentage = 0;
            }

            Tuple<int, int> aRecordOfWorldCompletion = new Tuple<int, int>(i,completionPercentage);
            worldCompletionPercentage.Add(aRecordOfWorldCompletion);
        }

        return worldCompletionPercentage;
    }


    /** Returns the total number of worlds
     * */
    public int GetWorldCount()
    {
        return this.worldCount;
    }
    /** Sets the total number of worlds
     * @params worldCount is the total number of worlds to be set
  * */
    public void SetWorldCount(int worldCount)
    {
        this.worldCount = worldCount ;
    }

    /** Gets the total number of worlds
 * */
    public int GetWorldProgress()
    {
        return this.worldProgress;
    }
    /** Gets all the world and its names in a tuple within a list
* */
    public List<Tuple<int,string>> GetWorldNames()
    {
        return this.worldNames;
    }
}
