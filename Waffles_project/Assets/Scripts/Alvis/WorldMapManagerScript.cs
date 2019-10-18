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




    private Login LoginManagerScript;


    //Load world data from database and initialize the map
    async void Start()
    {

        LoginManagerScript = GameObject.Find("LoginManager").GetComponent<Login>();
        loadText.text = "Loading..";
        worldNames = new List<string>();

        worldStageProgress = new List<Tuple<int, string, string>>();
        worldStageNames = new List<Tuple<int,string,string>>();

       await GetWorldProgressFromDatabase();
        loadText.text = "";



    }

    //determine which buttons are to be active or which to be inactive
    public void DeclareWorldMapButtons(int worldProgress, int worldCount)
    {
        for (int i = 0; i < worldMapButtons.Length; i++)
        {

            WorldMapButtonScript aWorldButtonScript = worldMapButtons[i].GetComponent<WorldMapButtonScript>();


            if (i < worldProgress)
            {

                aWorldButtonScript.SetWorldButtonImage(activeSprite);
                worldMapButtons[i].GetComponent<Button>().interactable = true;
                aWorldButtonScript.SetWorldName(worldNames[i]);
                aWorldButtonScript.SetWorldButton(true);
            }
            else if (i >= worldProgress && i < worldCount)
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


    //method to handle data from database
    private async Task GetWorldProgressFromDatabase()
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
        int worldCount = 0;
        foreach (var world in snapShotOfWorld.Children)
        {
            worldCount++;
            foreach(var stage in world.Children)
            { Tuple<int, string, string> aRecordOfworldStageNames = new Tuple<int, string, string>(worldCount, stage.Key.ToString(), stage.Value.ToString());
                this.worldStageNames.Add(aRecordOfworldStageNames);
            }
        }
        Debug.Log("worldcount" + "  " + worldCount);
        return worldCount;
    }




    //method to handle data from database
    private int ExtractUserWorldProgress(DataSnapshot snapShotOfUserProgress)
    {

        int worldProgress = 1;

        string userID = LoginManagerScript.GetUserID();
        foreach (var userid in snapShotOfUserProgress.Children)
        {
            if (userid.Key.ToString() == userID)
            {
                foreach (var world in userid.Children) //values in indiQuestion
                {
                   this.worldNames.Add(world.Key.ToString());
                    worldProgress = Int32.Parse(world.Key.ToString().Substring(5));

                    foreach(var stage in world.Children)
                    { Tuple<int, string, string> aRecordOfWorldStageProgress = new Tuple<int, string, string>( worldProgress, stage.Key.ToString(), stage.Value.ToString());
                        this.worldStageProgress.Add(aRecordOfWorldStageProgress);
                    }
                }
                break;
            }
            else
            {
                
            }
        }

        return worldProgress;
    }

   
}
