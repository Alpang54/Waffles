using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;
public class WorldMapManagerScript : MonoBehaviour
{
    [SerializeField] public static int worldSelected;

    public Sprite activeSprite;
    public Sprite lockedSprite;

    const double noOfWorldPerPage = 9.0;

    private int worldCount;
    private int worldProgress;
    private int pageNumber;

    private List<string> worldNames;

    private List<Tuple<int,string,string>> worldStageNames;
    private List<Tuple<int, string, string>> worldStageProgress;

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


    //method to handle data from database
    private async Task GetWorldAndUserProgressFromDatabase()
    {
        GameObject DBHandler = GameObject.Find("DBHandler");
        DBHandler DBHandlerScript = DBHandler.GetComponent<DBHandler>();

        await DBHandlerScript.ReadfromFirebase("World");
        DataSnapshot SnapshotOfWorld = DBHandlerScript.GetSnapshot();
        ExtractWorldInformation(SnapshotOfWorld);
        ProcessWorldInformation(this.worldStageNames);
        

        await DBHandlerScript.ReadfromFirebase("Progress");
        DataSnapshot snapshotOfUserProgress = DBHandlerScript.GetSnapshot();
        ExtractUserWorldProgress(snapshotOfUserProgress);
        ProcessUserProgessLogic(this.worldStageProgress);

        this.worldCompletionPercentage=worldMapImplementor.ComputeWorldCompletionPercentage(this.worldStageProgress, this.worldStageNames, this.worldCount);

        DeclareWorldMapButtons(worldProgress, worldCount,this.pageNumber);

        




    }

    private void ExtractWorldInformation(DataSnapshot snapShotOfWorld)
    {
        this.worldStageNames = worldMapImplementor.ExtractWorldInformationLogic(snapShotOfWorld);
       
        this.worldNames = worldMapImplementor.GetWorldNames();
     

    }

    private void ProcessWorldInformation(List<Tuple<int, string, string>> worldStageNames)
    {
        worldMapImplementor.ProcessWorldInformation(worldStageNames);
        this.worldCount = worldMapImplementor.GetWorldCount();
       
    }

    //method to handle data from database
    private void ExtractUserWorldProgress(DataSnapshot snapShotOfUserProgress)
    {
        string userID = datahandler.GetFirebaseUserId();
        this.worldStageProgress = worldMapImplementor.ExtractUserProgressLogic(snapShotOfUserProgress, userID);
        
        
        

      
    }
    
    private void ProcessUserProgessLogic(List<Tuple<int, string, string>> worldStageProgress)
    {
        worldMapImplementor.ProcessUserProgressLogic(worldStageProgress);
        this.worldProgress = worldMapImplementor.GetWorldProgress();
       
    }




    //determine which buttons are to be active or which to be inactive
    public void DeclareWorldMapButtons(int worldProgress, int worldCount,int pageNumber)
    {
        for (int i = 0; i < worldMapButtons.Length; i++)
        {

            WorldMapButtonScript aWorldButtonScript = worldMapButtons[i].GetComponent<WorldMapButtonScript>();

            int result = worldMapImplementor.DeclareWorldButtonsLogic(worldProgress, worldCount, pageNumber, i);

            if (result==1)
            {
                // To calculate the worldNumber Text for different pages
                int worldLevelForAButton = worldMapImplementor.ComputeWorldNumber(pageNumber, noOfWorldPerPage, i);
                Debug.Log("worldLevelForAButton" + worldLevelForAButton);
                worldMapButtons[i].GetComponent<Button>().interactable = true;
                aWorldButtonScript.SetWorldButtonImage(activeSprite);
                aWorldButtonScript.SetWorldLevel(worldLevelForAButton);

                aWorldButtonScript.SetWorldButton(true);
         
            }
            else if(result == 2)
            {

                aWorldButtonScript.SetWorldButtonImage(lockedSprite);
                aWorldButtonScript.SetWorldButton(false);
            }


        }
    }


    //when a world is selected, show confirm panel
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

        confirmPanel.confirmPanelAppear(this.worldNames[worldLevel-1], worldLevel,selectedWorldCompletionPercent);
    }

    // when a world is selected and confirmed, pass to stage manager to load stage map and turn world map off
    public void OnSelectWorldProceedButton(int worldLevel)
    {
       
        StageMapManagerScript stageManager = stageMapManager.GetComponent<StageMapManagerScript>();
        stageManager.LoadStageMap(worldLevel,this.worldStageNames,this.worldStageProgress);
        worldSelect.SetActive(false);

    }

    //turn world map off
    public void WorldSelectDisappear()
    {
        worldSelect.SetActive(false);
    }

    //turn world map on
    public void WorldSelectAppear()
    {
        worldSelect.SetActive(true);
    }

    public List<Tuple<int, string, string>> GetWorldStageNames()
    {
        return this.worldStageNames;
    }

    public void SetWorldStageNames(List<Tuple<int, string, string>> newWorldStageNames)
    {
        this.worldStageNames=newWorldStageNames;
    }

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

        if (noOfAcceptablePages <= 1)
        {
            LeftButton.SetActive(false);
        }
        else {
            LeftButton.SetActive(true); }
    }


    public void onSelectNextButton()

    {
        double noOfAcceptablePages = this.worldCount / noOfWorldPerPage;
        

        if (noOfAcceptablePages>this.pageNumber)
        {
            this.pageNumber++;
            DeclareWorldMapButtons(this.worldProgress, this.worldCount, this.pageNumber);
        }

        UpdateLeftRightButtons();

    }

    public void onSelectPreviousButton()
    {   if (this.pageNumber <= 1)
        { }
        else
        {
            this.pageNumber--;
            DeclareWorldMapButtons(this.worldProgress, this.worldCount, this.pageNumber);
        }
        UpdateLeftRightButtons();
    }






}




public class WorldMapImplementation
{
    private int worldCount;
    private List<string> worldNames;
    private int worldProgress;

    public int DeclareWorldButtonsLogic(int worldProgress, int worldCount, int pageNumber, int i)
    {
        i = i + 9 * (pageNumber-1)+1;


        if (i <= worldProgress && i < worldCount)
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
    public List<Tuple<int, string, string>>  ExtractWorldInformationLogic(DataSnapshot snapShotOfWorld)
    {

        this.worldNames = new List<string>();
        int worldNumber=0;
        if (snapShotOfWorld == null)
        {
            return null;
        }
        List<Tuple<int, string, string>> worldStageNames = new List<Tuple<int, string, string>>();
        foreach (var world in snapShotOfWorld.Children)
        {   
            worldNumber++;
            this.worldNames.Add(world.Key.ToString());
            foreach (var stage in world.Children)
            {
                Tuple<int, string, string> aRecordOfworldStageNames = new Tuple<int, string, string>(worldNumber, stage.Key.ToString(), stage.Value.ToString());
                worldStageNames.Add(aRecordOfworldStageNames);
            }
        }
        
 
        return worldStageNames;
      
    }

    public void ProcessWorldInformation(List<Tuple<int, string, string>> worldStageNames)
    {
        int worldCount = 0;
        foreach(var aRecordOfWorldStageNames in worldStageNames)
        {
            worldCount=aRecordOfWorldStageNames.Item1;
        }
        this.worldCount = worldCount;
    }

    public List<Tuple<int, string, string>> ExtractUserProgressLogic(DataSnapshot snapShotOfUserProgress, string userID)
    {

        int worldProgress;
        
        List<Tuple<int, string, string>> worldStageProgress = new List<Tuple<int, string, string>>();

        if (snapShotOfUserProgress == null || userID == null)
        {
            return null;
        }


            foreach (var userid in snapShotOfUserProgress.Children)
        {
            if (userid.Key.ToString() == userID)
            {
                foreach (var world in userid.Children)
                {
                    
                    worldProgress = Int32.Parse(world.Key.ToString().Substring(5));

                    foreach (var stage in world.Children)
                    {
                        Tuple<int, string, string> aRecordOfWorldStageProgress = new Tuple<int, string, string>(worldProgress, stage.Key.ToString(), stage.Value.ToString());
                        worldStageProgress.Add(aRecordOfWorldStageProgress);
                    }
                    
                }
                break;
            }
        }
       
        return worldStageProgress;
    }

    public void ProcessUserProgressLogic(List<Tuple<int, string, string>> worldStageProgress)
    {
        int worldProgress = 1;
        foreach (var aRecordOfWorldStageProgress in worldStageProgress)
        {
            worldProgress = aRecordOfWorldStageProgress.Item1;
        }
        this.worldProgress = worldProgress;
    }


    public int ComputeWorldNumber(int pageNumber, double noOfWorldPerPage, int i)
    {
        int worldNumberText = (int) ((pageNumber-1) * noOfWorldPerPage + i + 1);
        return worldNumberText;
    }

    public List<Tuple<int,int>> ComputeWorldCompletionPercentage(List<Tuple<int, string, string>> worldStageProgress, List<Tuple<int, string, string>> worldStageNames,int worldCount)
    {
        List<Tuple<int, int>> worldCompletionPercentage = new List<Tuple<int, int>>();
        
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
                    Debug.Log(Int32.Parse(aRecordOfWorldStageProgress.Item3));
                    
                    totalScore += Int32.Parse(aRecordOfWorldStageProgress.Item3);


                }
            }
            int completionPercentage = totalScore / totalNoOfStages;

            Tuple<int, int> aRecordOfWorldCompletion = new Tuple<int, int>(i,completionPercentage);
            worldCompletionPercentage.Add(aRecordOfWorldCompletion);

        }

        return worldCompletionPercentage;
    }


    public int GetWorldCount()
    {
        return this.worldCount;
    }
    public int GetWorldProgress()
    {
        return this.worldProgress;
    }

    public List<string> GetWorldNames()
    {
        return this.worldNames;
    }
}
