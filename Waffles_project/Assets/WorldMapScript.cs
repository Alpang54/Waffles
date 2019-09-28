using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Proyecto26;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Networking;
using Facebook.Unity;
using System;
using System.Threading.Tasks;

public class WorldMapScript : MonoBehaviour
{

    
    [SerializeField]
    private int totalWorldMapNodes;
    [SerializeField]
    private string[] worldNames;

    public Text worldLevelText;
    public Text confirmWorldLevelText;
    public int worldLevel;


    public GameObject worldConfirmPanel;

  
    public Sprite activeSprite;
    public Sprite lockedSprite;
    private Image buttonImage;
    private Button worldButton;


   async void Start()
    {
        
       await getWorldNamesfromDatabase();
   

        buttonImage = GetComponent<Image>();
        worldButton = GetComponent<Button>();
        InitializeButtons();

    }

    void InitializeButtons()
    {
      
        if (worldLevel<worldNames.Length)
        {
            buttonImage.sprite = activeSprite;
            worldButton.enabled = true;
            worldLevelText.text = "" + worldLevel;

        }
        else
        {
            buttonImage.sprite = lockedSprite;
            worldButton.enabled = false;
        }
    }
    void WorldConfirmPanel()

    {
        worldConfirmPanel.SetActive(true);
     
    }





    private async Task getWorldNamesfromDatabase()
    {
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage response = await client.GetAsync("https://cz3003-waffles.firebaseio.com/WorldNames/.json"))
            {
                using (HttpContent content = response.Content)
                {
                    string mycontent = await content.ReadAsStringAsync();
                    print(mycontent);
                    Debug.Log("Maybe");
                    if (mycontent != "null") {

                        mycontent = mycontent.Substring(1, mycontent.Length - 2);
                        string[] allWorlds = mycontent.Split(',');
                        totalWorldMapNodes = allWorlds.Length;

                        worldNames = new string[totalWorldMapNodes];
                        string[] worldKeyAndValueArray = mycontent.Split(new Char[] { ',', ':' },
                                StringSplitOptions.RemoveEmptyEntries); 

                        for(int i=0;i<worldKeyAndValueArray.Length; i=i+2)
                        {
                            worldNames[i / 2] = worldKeyAndValueArray[i + 1];}
                        for(int j = 0; j < worldNames.Length; j++)
                        {
                            Debug.Log(worldNames[j]);
                        }
                    }
                    }
                }
            }
        }
    


    public void OnSelectWorld()
    {
        print("start");
        print("working");
        confirmWorldLevelText.text = "" + worldLevel + "-" + worldNames[worldLevel].Substring(1,worldNames[worldLevel].Length-2);
        worldConfirmPanel.SetActive(true);

    }
}

/*
 
     private async void getStageNamesFromDatabase(string worldname)
    {
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage response = await client.GetAsync("https://cz3003-waffles.firebaseio.com/StageNames/.json"))
            {
                using (HttpContent content = response.Content)
                {
                    string mycontent = await content.ReadAsStringAsync();
                    print(mycontent);


                }

            }
        }
    }

    public void getStageNames()
    {
        Debug.Log("Working another");
        getStageNamesFromDatabase("äli");
    }
*/
