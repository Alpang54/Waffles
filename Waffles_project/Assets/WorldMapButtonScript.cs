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

public class WorldMapButtonScript : MonoBehaviour
{

    public int worldLevel;
    private string worldName;

    public Text worldButtonText;
    private Image worldButtonImage;
    private Button worldButton;

    public GameObject worldMapManager;


 void Start()
    {
       
        worldButtonImage = GetComponent<Image>();
        worldButton = GetComponent<Button>();
        worldButtonText.text = "" + worldLevel;

    }

    public void SetWorldName(string worldName)
    {
        this.worldName = worldName;
        

    }
    public void SetWorldButtonImage(Sprite activeOrInactiveSprite)
    {
        worldButtonImage.sprite = activeOrInactiveSprite;

    }
    public void SetWorldButton(Boolean enabledOrNot)
    {
        worldButton.enabled = enabledOrNot;
    }


    public void OnSelectWorld()
    {

        WorldMapManagerScript mapManager = worldMapManager.GetComponent<WorldMapManagerScript>();
        
        mapManager.OnSelectWorldButton(this.worldName, this.worldLevel);

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
