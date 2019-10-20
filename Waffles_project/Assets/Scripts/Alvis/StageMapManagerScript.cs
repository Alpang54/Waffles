using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Firebase.Database;
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

    private int stageCount;
    private List<string> stageNames;
    List<Tuple<int, string, string>> worldStageNames;
    List<Tuple<int, string, string>> worldStageProgress;

    private DataHandler datahandler;



    public void LoadStageMap(int worldLevel, List<Tuple<int, string, string>> worldStageNames, List<Tuple<int, string, string>> worldStageProgress)
    {
        datahandler = GameObject.Find("DataManager").GetComponent<DataHandler>();
        this.stageNames = new List<string>();
        this.worldStageNames = worldStageNames;
        this.worldStageProgress = worldStageProgress;
        this.worldLevel = worldLevel;
       
        DeclareStageMapButtons();
        stageSelect.SetActive(true);
        DontDestroyOnLoad(this.gameObject);
    }

 



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
        
        Tuple<int, int> worldAndStageLevel = new Tuple<int, int>(this.worldLevel, stageLevel);
        datahandler.SetWorldAndStageLevel(worldAndStageLevel);
        //SceneManager.LoadScene(nextScene);
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
        this.stageCount = 0;
        int stageProgress = 0;
        
       foreach(var entry in worldStageProgress)
        {
            if (entry.Item1 == this.worldLevel)
            {   
                stageProgress++;
               

            }
        }

       foreach(var entry in worldStageNames)
        {
            if (entry.Item1 == this.worldLevel)
            {
                this.stageCount++;
                stageNames.Add(entry.Item2);

            }

        }




        for (int i = 0; i < stageMapButtons.Length; i++)
        {
            StageMapButtonScript aStageButton = stageMapButtons[i].GetComponent<StageMapButtonScript>();

            if (i < stageProgress)
            {
             
                aStageButton.SetStageButtonImage(activeSprite2);
                aStageButton.SetStageName(stageNames[i]);
                stageMapButtons[i].GetComponent<Button>().interactable = true;
                aStageButton.SetStageButton(true);
            }

            else if (i >= stageProgress && i<stageCount) 
            {
                aStageButton.SetStageButtonImage(lockedSprite2);
                aStageButton.SetStageButton(false);
            }
        }
    }
}



  