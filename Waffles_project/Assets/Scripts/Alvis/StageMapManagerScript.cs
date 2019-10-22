using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private int stageProgress;
    private List<string> stageCompletionPercentage;
    private List<string> stageNames;
    List<Tuple<int, string, string>> worldStageNames;
    List<Tuple<int, string, string>> worldStageProgress;

    private DataHandler datahandler;
    private StageMapManagerImplementation stageMapImplementor;



    public void LoadStageMap(int worldLevel, List<Tuple<int, string, string>> worldStageNames, List<Tuple<int, string, string>> worldStageProgress)
    {
        datahandler = GameObject.Find("DataManager").GetComponent<DataHandler>();
      
        this.worldStageNames = worldStageNames;
        this.worldStageProgress = worldStageProgress;
        this.worldLevel = worldLevel;
        stageMapImplementor = new StageMapManagerImplementation();
        stageMapImplementor.InitializeNoOfStagesAndUserProgress(this.worldStageNames,this.worldStageProgress,this.worldLevel);
        this.stageNames = stageMapImplementor.GetStageNames();
        this.stageCount = stageMapImplementor.GetStageCount();
        this.stageProgress = stageMapImplementor.GetStageProgress();
        this.stageCompletionPercentage = stageMapImplementor.GetStageCompletionPercentage();
        DeclareStageMapButtons();
        stageSelect.SetActive(true);
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
        confirmPanel.confirmPanelAppear(stageName, worldLevel, stageLevel,stageCompletionPercentage[stageLevel-1]);

    }

    //when a stage is selected and confirmed, load next scene
    public void OnSelectStagePlayButton(int stageLevel)
    {
        //change depengind on ameplay,ideally load scene
        
        Tuple<int, int> worldAndStageLevel = new Tuple<int, int>(this.worldLevel, stageLevel);
        datahandler.SetWorldAndStageLevel(worldAndStageLevel);
        SceneManager.LoadScene("Custom Lobby");
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

    public void Share()
    {
        ShareScript sharescript = GameObject.Find("ShareHandler").GetComponent<ShareScript>();
        sharescript.Share();
    }
    //Determine which stages are available to the user.
    public void DeclareStageMapButtons()
    { 

        for (int i = 0; i < stageMapButtons.Length; i++)
        {
            StageMapButtonScript aStageButton = stageMapButtons[i].GetComponent<StageMapButtonScript>();
            if (stageMapImplementor.DeclareStageMapButton(this.stageProgress,this.stageCount,i)==1)
            {
             
                aStageButton.SetStageButtonImage(activeSprite2);
                aStageButton.SetStageName(stageNames[i]);
                stageMapButtons[i].GetComponent<Button>().interactable = true;
                aStageButton.SetStageButton(true);
            }

            else if (stageMapImplementor.DeclareStageMapButton(this.stageProgress, this.stageCount, i) == 2) 
            {
                aStageButton.SetStageButtonImage(lockedSprite2);
                aStageButton.SetStageButton(false);
            }
        }
    }
}


public class StageMapManagerImplementation
{
    List<string> stageCompletionPercentage = new List<string>();
    List<string> stageNames = new List<string>();
    int stageCount = 0;
    int stageProgress = 0;

    public int GetStageCount()
    {
        return this.stageCount;
    }

    public int GetStageProgress()
    {
        return this.stageProgress;
    }

    public List<string> GetStageNames()
    {
        return this.stageNames;
    }

    public List<string> GetStageCompletionPercentage()
    {
        return this.stageCompletionPercentage;
    }

    public void InitializeNoOfStagesAndUserProgress(List<Tuple<int, string, string>> worldStageNames,
    List<Tuple<int, string, string>> worldStageProgress,int worldLevel)

    {

        foreach (var entry in worldStageProgress)
        {
            if (entry.Item1 == worldLevel)
            {

                this.stageProgress++;
                stageCompletionPercentage.Add(entry.Item3);
            }
        }

        foreach (var entry in worldStageNames)
        {  
            if (entry.Item1 == worldLevel)
            {
                this.stageCount++;
                this.stageNames.Add(entry.Item3);

            }

        }

    }


    public int DeclareStageMapButton(int stageProgress, int stageCount, int i)
    {
        if (i < stageProgress)
        {
            return 1;
        }
        else if ((i >= stageProgress && i < stageCount))
        {
            return 2;
        }
        else
        {
            return 0;
        }
      
    }
}



  