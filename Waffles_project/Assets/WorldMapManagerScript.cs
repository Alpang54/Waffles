using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
public class WorldMapManagerScript : MonoBehaviour
{
    [SerializeField] public static int worldSelected;

    public Sprite activeSprite;
    public Sprite lockedSprite;

    private string[] worldNames;

    public Text loadText;
    public GameObject[] worldMapButtons;
    public GameObject worldConfirmPanel;
    public GameObject worldSelect;
    public GameObject stageMapManager;


    
    //Load world data from database and initialize the map
    async void Start()
    {
        loadText.text = "Loading..";
        await GetWorldNamesFromDatabase();
        Debug.Log("rest api should have finished by now");
        DeclareWorldMapButtons();
        loadText.text = "";

    }

    //determine which buttons are to be active or which to be inactive
    public void DeclareWorldMapButtons()
    {   
        for (int i = 0; i < worldMapButtons.Length; i++)
        {
    
            WorldMapButtonScript aWorldButtonScript= worldMapButtons[i].GetComponent<WorldMapButtonScript>();
      

            if (i<worldNames.Length)
            {
                aWorldButtonScript.SetWorldButtonImage(activeSprite);
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
    public async void OnSelectWorldProceedButton(int worldLevel)
    {
        Debug.Log(worldLevel);
        StageMapManagerScript stageManager = stageMapManager.GetComponent<StageMapManagerScript>();
        await stageManager.LoadStageMap(worldLevel);
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
    private async Task GetWorldNamesFromDatabase()
    {
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage response = await client.GetAsync("https://cz3003-waffles.firebaseio.com/WorldNames/.json"))
            {
                using (HttpContent content = response.Content)
                {
                    string mycontent = await content.ReadAsStringAsync();
                    print("what is gotten from REST API:"+mycontent);
                    HandleRestAPICallOnWorldNames(mycontent);
                }
            }
        }
    }

    //method to handle data from database
    public void HandleRestAPICallOnWorldNames(string result)
    {
        if (result != "null")
        {
            result = result.Substring(1, result.Length - 2);
            string[] temp_allWorlds = result.Split(',');
            int temp_totalWorldMapNodes = temp_allWorlds.Length;

            worldNames = new string[temp_totalWorldMapNodes];
            string[] worldKeyAndValueArray = result.Split(new Char[] { ',', ':' },
                    StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < worldKeyAndValueArray.Length; i = i + 2)
            {   
                this.worldNames[i / 2] = worldKeyAndValueArray[i + 1].Substring(1, worldKeyAndValueArray[i + 1].Length - 2);
            }
            for (int j = 0; j < worldNames.Length; j++)
            {
                Debug.Log(worldNames[j]);
            }
        }

    }


}
