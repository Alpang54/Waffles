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


    private int worldCount;
    private int worldProgress;

    private List<string> worldNames;

    private List<Tuple<int,string,string>> worldStageNames;
    private List<Tuple<int, string, string>> worldStageProgress;


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
        loadText.text = "Loading..";
        worldMapImplementor = new WorldMapImplementation();

        await GetWorldAndUserProgressFromDatabase();
        loadText.text = "";



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


        DeclareWorldMapButtons(worldProgress, worldCount);


    }

    private void ExtractWorldInformation(DataSnapshot snapShotOfWorld)
    {
        this.worldStageNames = worldMapImplementor.ExtractWorldInformationLogic(snapShotOfWorld);
        Debug.Log("this.worldstagenames is" + this.worldStageNames);

    }

    private void ProcessWorldInformation(List<Tuple<int, string, string>> worldStageNames)
    {
        worldMapImplementor.ProcessWorldInformation(worldStageNames);
        this.worldCount = worldMapImplementor.GetWorldCount();
        Debug.Log("this.worldcount is" + this.worldCount);
    }

    //method to handle data from database
    private void ExtractUserWorldProgress(DataSnapshot snapShotOfUserProgress)
    {
        string userID = datahandler.GetFirebaseUserId();
        this.worldStageProgress = worldMapImplementor.ExtractUserProgressLogic(snapShotOfUserProgress, userID);
        Debug.Log("this.worldstageprogress is" + this.worldStageProgress);
        this.worldNames = worldMapImplementor.GetWorldNames();
        Debug.Log("this.worldnames is" + this.worldNames);
        

      
    }
    
    private void ProcessUserProgessLogic(List<Tuple<int, string, string>> worldStageProgress)
    {
        worldMapImplementor.ProcessUserProgressLogic(worldStageProgress);
        this.worldProgress = worldMapImplementor.GetWorldProgress();
        Debug.Log("this.worldprogress is" + this.worldProgress);
    }




    //determine which buttons are to be active or which to be inactive
    public void DeclareWorldMapButtons(int worldProgress, int worldCount)
    {
        for (int i = 0; i < worldMapButtons.Length; i++)
        {

            WorldMapButtonScript aWorldButtonScript = worldMapButtons[i].GetComponent<WorldMapButtonScript>();

           

            if (worldMapImplementor.DeclareWorldButtonsLogic(worldProgress,worldCount,i))
            {
                aWorldButtonScript.SetWorldButtonImage(activeSprite);
                worldMapButtons[i].GetComponent<Button>().interactable = true;

                aWorldButtonScript.SetWorldButton(true);
         
            }
            else
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
        confirmPanel.confirmPanelAppear(this.worldNames[worldLevel-1], worldLevel);
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






   
}


public class WorldMapImplementation
{
    private int worldCount;
    private List<string> worldNames;
    private int worldProgress;

    public bool DeclareWorldButtonsLogic(int worldProgress, int worldCount, int i)
    {
        if (i < worldProgress && i<worldCount)
        {
            return true;
        }
    
        return false;
    }

    public List<Tuple<int, string, string>>  ExtractWorldInformationLogic(DataSnapshot snapShotOfWorld)
    {


        int worldNumber=0;
        if (snapShotOfWorld == null)
        {
            return null;
        }
        List<Tuple<int, string, string>> worldStageNames = new List<Tuple<int, string, string>>();
        foreach (var world in snapShotOfWorld.Children)
        {   
            worldNumber++;
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
        this.worldNames = new List<string>();
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
                    this.worldNames.Add(world.Key.ToString());
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
        int worldProgress = 0;
        foreach (var aRecordOfWorldStageProgress in worldStageProgress)
        {
            worldProgress = aRecordOfWorldStageProgress.Item1;
        }
        this.worldProgress = worldProgress;
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
