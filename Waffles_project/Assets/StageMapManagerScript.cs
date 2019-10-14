using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class StageMapManagerScript : MonoBehaviour
{
    public GameObject stageSelect;
    public GameObject[] stageMapButtons;
    public GameObject stageConfirmPanel;
    public Sprite activeSprite2;
    public Sprite lockedSprite2;
    public Text loadText;


    public GameObject toggleDifficulty;
    public Text difficultyText;

    private int worldLevel;
    private string[] stageNames;

    //turn stage map on
    public void SetActive()
    {
        stageSelect.SetActive(true);
    }
    // turn stage map off
    public void SetInactive()
    {
        stageSelect.SetActive(false);
    }

    //when a stage is selected, show confirm panel
    public void OnSelectStageButton(int stageLevel, string stageName)
    {
        StageConfirmPanel confirmPanel = stageConfirmPanel.GetComponent<StageConfirmPanel>();
        confirmPanel.confirmPanelAppear(stageName, worldLevel, stageLevel);

    }

    //when a stage is selected and confirmed, load next scene
    public void OnSelectStagePlayButton(int stageLevel)
    {
        //change depengind on ameplay,ideally load scene
    }


    //Loads stage map
    public async Task LoadStageMap(int worldLevel)
    {

        this.worldLevel = worldLevel;
        loadText.text = "Loading";
        await GetStageNamesFromDatabase();
        stageSelect.SetActive(true);
        Debug.Log("rest apis should have finished by now");
        DeclareStageMapButtons();
        loadText.text = "";
        
    }


    //Handle data from database
    private async Task GetStageNamesFromDatabase()
    {
       
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage response = await client.GetAsync("https://cz3003-waffles.firebaseio.com/StageNames/World" + this.worldLevel + "/.json"))
            {
                using (HttpContent content = response.Content)
                {
                    string mycontent = await content.ReadAsStringAsync();
                    print(mycontent);
                 
                    HandleRestAPICallOnStageNames(mycontent);

                }
            }
        }
    }

    //handle data from database
    public void HandleRestAPICallOnStageNames(string result)
    {
        if (result != "null")
        {
            result = result.Substring(1, result.Length - 2);
            string[] temp_allStages = result.Split(',');
            int temp_totalStageMapNodes = temp_allStages.Length;

            stageNames = new string[temp_totalStageMapNodes];
            string[] worldKeyAndValueArray = result.Split(new Char[] { ',', ':' },
                    StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < worldKeyAndValueArray.Length; i = i + 2)
            {
                this.stageNames[i / 2] = worldKeyAndValueArray[i + 1].Substring(1, worldKeyAndValueArray[i + 1].Length - 2);
            }
            for (int j = 0; j < stageNames.Length; j++)
            {
                Debug.Log(stageNames[j]);
            }
        }

    }

    public void ToggleDifficulty()
    {
        if (difficultyText.text == "Normal")
        {
            for(int i = 0; i < 9; i++)
            {
                stageMapButtons[i].GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            }
            difficultyText.text = "Hard";
            this.toggleDifficulty.GetComponent<Image>().color= new Color32(255, 0, 0, 255);
        }
        else if(difficultyText.text == "Hard")
        {
            for (int i = 0; i < 9; i++)
            {
                stageMapButtons[i].GetComponent<Image>().color = new Color32(104, 3, 0, 255);
            }
            difficultyText.text = "Extreme";
            this.toggleDifficulty.GetComponent<Image>().color = new Color32(104, 3, 0, 255);

        }

        else
        {
            for (int i = 0; i < 9; i++)
            {
                stageMapButtons[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
            difficultyText.text = "Normal";
            this.toggleDifficulty.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }


   

    //Determine which stages are available to the user.
    public void DeclareStageMapButtons()
    {
        Debug.Log(Login.userID);
        for (int i = 0; i < stageMapButtons.Length; i++)
        {
            StageMapButtonScript aStageButton = stageMapButtons[i].GetComponent<StageMapButtonScript>();

            if (i < stageNames.Length)
            {
             
                aStageButton.SetStageButtonImage(activeSprite2);
                aStageButton.SetStageName(stageNames[i]);
                aStageButton.SetStageButton(true);
            }

            else
            {
                aStageButton.SetStageButtonImage(lockedSprite2);
                aStageButton.SetStageButton(false);
            }
        }
    }
}



  