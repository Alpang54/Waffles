using System;
using System.Collections;
using System.Collections.Generic;
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
        worldNames = new List<string>();
        worldStageProgress = new List<Tuple<int, string, string>>();
        worldStageNames = new List<Tuple<int,string,string>>();
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
        int worldCount = ExtractWorldInformation(SnapshotOfWorld);

        await DBHandlerScript.ReadfromFirebase("Progress");
        DataSnapshot snapshotOfUserProgress = DBHandlerScript.GetSnapshot();
        int worldProgress = ExtractUserWorldProgress(snapshotOfUserProgress);


        DeclareWorldMapButtons(worldProgress, worldCount);


    }

    private int ExtractWorldInformation(DataSnapshot snapShotOfWorld)
    {
        this.worldStageNames = worldMapImplementor.ExtractWorldInformationLogic(snapShotOfWorld);
        int worldCount = worldMapImplementor.GetNoOfWorlds();
        Debug.Log("No of Worlds" + worldCount);
        return worldCount;
    }


    //method to handle data from database
    private int ExtractUserWorldProgress(DataSnapshot snapShotOfUserProgress)
    {
        string userID = datahandler.GetFirebaseUserId();
        this.worldStageProgress = worldMapImplementor.ExtractUserProgressLogic(snapShotOfUserProgress, userID);
        this.worldNames = worldMapImplementor.GetWorldNames();

        Debug.Log("world progress is" + worldMapImplementor.GetWorldProgress());
        return worldMapImplementor.GetWorldProgress();
    }




    //determine which buttons are to be active or which to be inactive
    public void DeclareWorldMapButtons(int worldProgress, int worldCount)
    {
        for (int i = 0; i < worldMapButtons.Length; i++)
        {

            WorldMapButtonScript aWorldButtonScript = worldMapButtons[i].GetComponent<WorldMapButtonScript>();
            WorldMapImplementation worldMapeImplentor = new WorldMapImplementation();

            Debug.Log(" i is " + i + "worldProgress is + " + worldProgress + "worldCount is + " + worldCount);

            if (worldMapeImplentor.DeclareWorldButtonsLogic(worldProgress,worldCount,i))
            {
                aWorldButtonScript.SetWorldButtonImage(activeSprite);
                worldMapButtons[i].GetComponent<Button>().interactable = true;
                aWorldButtonScript.SetWorldName(worldNames[i]);
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
    public void OnSelectWorldButton(string worldName, int worldLevel)
    {
        Debug.Log(worldLevel);
        WorldConfirmPanel confirmPanel = worldConfirmPanel.GetComponent<WorldConfirmPanel>();
        confirmPanel.confirmPanelAppear(worldName, worldLevel);
    }

    // when a world is selected and confirmed, pass to stage manager to load stage map and turn world map off
    public void OnSelectWorldProceedButton(int worldLevel)
    {
        Debug.Log(worldLevel);
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
    private int noOfWorld;
    private List<string> worldNames;
    private int worldProgress;

    public bool DeclareWorldButtonsLogic(int worldProgress, int worldCount, int i)
    {
        if (i < worldProgress)
        {
            return true;
        }
    
        return false;
    }

    public List<Tuple<int, string, string>>  ExtractWorldInformationLogic(DataSnapshot snapShotOfWorld)
    {

        int worldNumber = 0;

        if (snapShotOfWorld == null)
        {
            this.noOfWorld = worldNumber;
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
        this.noOfWorld = worldNumber;
        Debug.Log("noOfWorld" + worldNumber);
        return worldStageNames;
      
    }

    public List<Tuple<int, string, string>> ExtractUserProgressLogic(DataSnapshot snapShotOfUserProgress, string userID)
    {
      

        this.worldNames = new List<string>();
        int worldProgress = 1;
        List<Tuple<int, string, string>> worldStageProgress = new List<Tuple<int, string, string>>();

        if (snapShotOfUserProgress == null || userID == null)
        {
           
            this.worldProgress = worldProgress ;
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
        this.worldProgress = worldProgress;
        return worldStageProgress;
    }


    public int GetNoOfWorlds()
    {
        return this.noOfWorld;
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
