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

    int pageNumber;

    const double noOfStagePerPage = 9.0;

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

    [SerializeField] private GameObject LeftButton;
    [SerializeField] private GameObject RightButton;



    public void LoadStageMap(int worldLevel, List<Tuple<int, string, string>> worldStageNames, List<Tuple<int, string, string>> worldStageProgress)
    {
        Debug.Log("world level is" + worldLevel);
        datahandler = GameObject.Find("DataManager").GetComponent<DataHandler>();
        this.pageNumber = 1;
        this.worldStageNames = worldStageNames;
        this.worldStageProgress = worldStageProgress;
        this.worldLevel = worldLevel;
        stageMapImplementor = new StageMapManagerImplementation();
        stageMapImplementor.InitializeNoOfStagesAndUserProgress(this.worldStageNames,this.worldStageProgress,this.worldLevel);
        this.stageNames = stageMapImplementor.GetStageNames();
        this.stageCount = stageMapImplementor.GetStageCount();
        this.stageProgress = stageMapImplementor.GetStageProgress();
        this.stageCompletionPercentage = stageMapImplementor.GetStageCompletionPercentage();
        DeclareStageMapButtons(this.stageProgress,this.stageCount,this.pageNumber);
        DetermineIfUserStageProgressShouldBeUpdated();
        stageSelect.SetActive(true);

    }


    private void DetermineIfUserStageProgressShouldBeUpdated()
    {   if (this.stageCompletionPercentage.Count >= stageProgress) // if current stage has completion percentage
        {

            //check if should declare new stages
            String stageCompletionPercentage = this.stageCompletionPercentage[stageProgress - 1];

            if (Int32.Parse(stageCompletionPercentage) >= 70 && this.stageProgress < this.stageCount)
            {
                this.stageProgress++;
                this.stageCompletionPercentage.Add("0");
                DeclareStageMapButtons(this.stageProgress, stageCount, this.pageNumber);
            }

        }
        else
        {
            this.stageCompletionPercentage.Add("0");
        }
        
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
        if (stageLevel == 1 && this.worldLevel == 1 && this.stageProgress==1)
        {
            confirmPanel.confirmPanelAppear(stageName, worldLevel, stageLevel, "0");
        }
        else
        {
            confirmPanel.confirmPanelAppear(stageName, worldLevel, stageLevel, stageCompletionPercentage[stageLevel - 1]);
            Debug.Log("stageCompletionPercentage = "+stageCompletionPercentage[stageLevel - 1]);
        }

    }

    //when a stage is selected and confirmed, load next scene
    public void OnSelectStagePlayButton(int stageLevel)
    {
        //change depengind on ameplay,ideally load scene
        
        Tuple<int, int> worldAndStageLevel = new Tuple<int, int>(this.worldLevel, stageLevel);

        datahandler.SetWorldAndStageLevel(worldAndStageLevel);
        Debug.Log("The result is:" +datahandler.GetWorldAndStageLevel());
        SceneManager.LoadScene("Main Stage");
    }



    public void Share()
    {
        ShareScript sharescript = GameObject.Find("ShareHandler").GetComponent<ShareScript>();
        sharescript.Share();
    }
    //Determine which stages are available to the user.
    public void DeclareStageMapButtons(int stageProgress, int stageCount,int pageNumber)
    { 

        for (int i = 0; i < stageMapButtons.Length; i++)
        {
            StageMapButtonScript aStageButton = stageMapButtons[i].GetComponent<StageMapButtonScript>();

            int result = stageMapImplementor.DeclareStageMapButton(stageProgress, stageCount,pageNumber, i);

            if (result==1)
            {
         
                int stageLevelForAButton = stageMapImplementor.computeStageNumber(pageNumber, noOfStagePerPage, i);
             
                stageMapButtons[i].GetComponent<Button>().interactable = true;
                aStageButton.SetStageButtonImage(activeSprite2);
                aStageButton.SetStageName(stageNames[i]);
                aStageButton.SetStageLevel(stageLevelForAButton);
                aStageButton.SetStageButton(true);
            }

            else if (result == 2) 
            {
                aStageButton.SetStageButtonImage(lockedSprite2);
                aStageButton.SetStageButton(false);
            }
        }
    }
    public void onNextStageMapButton()
    {
        double noOfAcceptablePages = this.stageCount / noOfStagePerPage;


        if (noOfAcceptablePages > this.pageNumber)
        {
            this.pageNumber++;
            DeclareStageMapButtons(this.stageProgress, this.stageCount, this.pageNumber);
        }
        UpdateLeftRightButtons();

    }

    public void onPreviousStageMapButton()
    {
        if (this.pageNumber <= 1)
        { }
        else
        {
            this.pageNumber--;
            DeclareStageMapButtons(this.stageProgress, this.stageCount, this.pageNumber);
        }
        UpdateLeftRightButtons();
    }
    private void UpdateLeftRightButtons()
    {
        double noOfAcceptablePages = this.stageCount / noOfStagePerPage;
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
        else
        {
            LeftButton.SetActive(true);
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
        if (this.stageProgress <= 0)
        {
            this.stageProgress = 1;
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

    

    public int computeStageNumber(int pageNumber, double noOfStagePerPage, int i)
    {
        int stageNumberText = (int)((pageNumber - 1) * noOfStagePerPage + i + 1);
        Debug.Log(stageNumberText);
        return stageNumberText;
    }

    public int DeclareStageMapButton(int stageProgress, int stageCount,int pageNumber, int i)
    {
     
        i = i + 9 * (pageNumber - 1) + 1;
        if (i <= stageProgress)
        {
            return 1;
        }
        else if ((i > stageProgress && i <= stageCount))
        {
            return 2;
        }
        else
        {
            return 0;
        }
      
    }
}



  