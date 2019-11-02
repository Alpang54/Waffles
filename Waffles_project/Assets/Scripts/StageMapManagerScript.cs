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

    private int worldLevel;
    private List<Tuple<int, string>> stageProgress;
    private List<Tuple<int, string>> stageNames;

    private DataHandler datahandler;
    private StageMapManagerImplementation stageMapImplementor;

    [SerializeField] private GameObject LeftButton;
    [SerializeField] private GameObject RightButton;



    public void LoadStageMap(int worldLevel, List<Tuple<int, int, string>> worldStageNames, List<Tuple<int, int, string>> worldStageProgress)
    {
        datahandler = GameObject.Find("DataManager").GetComponent<DataHandler>();

        this.pageNumber = 1;
        this.worldLevel = worldLevel;

        stageMapImplementor = new StageMapManagerImplementation();


        this.stageNames = stageMapImplementor.InitializeStageNames(worldStageNames, worldLevel);
        this.stageProgress = stageMapImplementor.InitializeStageProgress(worldStageProgress, worldLevel);

        
        DetermineIfUserStageProgressShouldBeUpdated();
        UpdateLeftRightButtons();
        if (stageNames[0].Item1 == 0)
        {
            DeclareStageMapButtons(stageProgress.Count, 0, this.pageNumber);
        }
        else
        {
            DeclareStageMapButtons(stageProgress.Count, stageNames.Count, this.pageNumber);
        }
        
        stageSelect.SetActive(true);

        for (int i =0;i<stageNames.Count;i++)
        {
            Debug.Log(stageNames[i]);
        }

        for (int i = 0; i < stageProgress.Count; i++)
        {
            Debug.Log(stageProgress[i]);
        }
    }


    private void DetermineIfUserStageProgressShouldBeUpdated()
    {
        int userStageProgress = this.stageProgress.Count;
        int temporaryStageCompletionPercentage = Int32.Parse(this.stageProgress[userStageProgress - 1].Item2);

        if (temporaryStageCompletionPercentage >= 70 && userStageProgress < this.stageNames.Count) // if current stage has completion percentage
        {
            Tuple<int, string> aRecordofStageProgress = new Tuple<int, string>(userStageProgress + 1, "0");
            this.stageProgress.Add(aRecordofStageProgress);
            DeclareStageMapButtons(this.stageProgress.Count, this.stageNames.Count, this.pageNumber);
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
    public void OnSelectStageButton(int stageLevel)
    {
        string stageName = this.stageNames[stageLevel - 1].Item2;
        StageConfirmPanel confirmPanel = stageConfirmPanel.GetComponent<StageConfirmPanel>();
        confirmPanel.confirmPanelAppear(stageName, worldLevel, stageLevel, this.stageProgress[stageLevel - 1].Item2);



    }

    //when a stage is selected and confirmed, load next scene
    public void OnSelectStagePlayButton(int stageLevel)
    {
        //change depengind on ameplay,ideally load scene

        Tuple<int, int> worldAndStageLevel = new Tuple<int, int>(this.worldLevel, stageLevel);

        datahandler.SetWorldAndStageLevel(worldAndStageLevel);
        SceneManager.LoadScene("Main Stage");
    }



    public void Share()
    {
        ShareScript sharescript = GameObject.Find("ShareHandler").GetComponent<ShareScript>();
        sharescript.Share();
    }

    //Determine which stages are available to the user.
    public void DeclareStageMapButtons(int stageProgress, int stageCount, int pageNumber)
    {

        for (int i = 0; i < stageMapButtons.Length; i++)
        {
            StageMapButtonScript aStageButton = stageMapButtons[i].GetComponent<StageMapButtonScript>();
            int result = stageMapImplementor.DeclareStageMapButton(stageProgress, stageCount, pageNumber, i);
            Debug.Log(result);
            if (result == 1)
            {
                aStageButton.SetStageButton(true);
                int stageLevelForAButton = stageMapImplementor.ComputeStageNumber(pageNumber, noOfStagePerPage, i);

                stageMapButtons[i].GetComponent<Button>().interactable = true;
                aStageButton.SetStageButtonImage(activeSprite2);
                aStageButton.SetStageLevel(stageLevelForAButton);
                aStageButton.SetStageText(stageLevelForAButton.ToString());


            }
            else if (result == 2)
            {
                aStageButton.SetStageButton(false);
                aStageButton.SetStageButtonImage(lockedSprite2);
                aStageButton.SetStageText("");

            }

            else
            {
                aStageButton.SetStageButton(true);
                stageMapButtons[i].GetComponent<Button>().interactable = false;
                aStageButton.SetStageButtonImage(lockedSprite2);
                aStageButton.SetStageText("");

            }
        }
    }
    public void onNextStageMapButton()
    {
        this.pageNumber++;
        UpdateLeftRightButtons();
    }

    public void onPreviousStageMapButton()
    {
        this.pageNumber--;
        UpdateLeftRightButtons();
    }
    private void UpdateLeftRightButtons()
    {
        double noOfAcceptablePages = this.stageNames.Count / noOfStagePerPage;

        if (noOfAcceptablePages > this.pageNumber)
        {
            RightButton.SetActive(true);

        }
        else
        {
            RightButton.SetActive(false);
        }

        if (this.pageNumber <= 1)
        {
            LeftButton.SetActive(false);
        }
        else
        {
            LeftButton.SetActive(true);
        }
        DeclareStageMapButtons(this.stageProgress.Count, this.stageNames.Count, this.pageNumber);
    }

    public void BackButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}

    public class StageMapManagerImplementation
    {
        List<string> stageCompletionPercentage = new List<string>();
        List<string> stageNames = new List<string>();
        int stageCount;
        int stageProgress;

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

        public List<Tuple<int, string>> InitializeStageProgress(List<Tuple<int, int, string>> worldStageProgress, int worldLevel)
        {

            List<Tuple<int, string>> stageProgress = new List<Tuple<int, string>>();
            foreach (var entry in worldStageProgress)
            {
                if (entry.Item1 == worldLevel)
                {
                    Tuple<int, string> aRecordOfStageProgress = new Tuple<int, string>(entry.Item2, entry.Item3);
                    stageProgress.Add(aRecordOfStageProgress);
                }

            }
            //Reorder the stages in ascending order
            int loop = 1;
            int terminate = stageProgress.Count;
            while (true)
            {
                if (loop >= terminate)
                {
                    break;
                }
                else if (stageProgress[loop - 1].Item1 > stageProgress[loop].Item1)
                {
                    Tuple<int, string> temp = stageProgress[loop];
                    stageProgress[loop] = stageProgress[loop - 1];
                    stageProgress[loop - 1] = temp;
                    loop = 1;
                }
                else { loop++; }
            }

            if (stageProgress.Count == 0)
            {
            Tuple<int, string> aRecordofStageProgress = new Tuple<int, string>(1, "0");
            stageProgress.Add(aRecordofStageProgress);

            }

            return stageProgress;
        }


        public List<Tuple<int, string>> InitializeStageNames(List<Tuple<int, int, string>> worldStageNames, int worldLevel)
        {

            List<Tuple<int, string>> stageNames = new List<Tuple<int, string>>();
            //First, get the stages for the world selected
            foreach (var entry in worldStageNames)
            {
                if (entry.Item1 == worldLevel)
                {
                    Tuple<int, string> aRecordofStageNames = new Tuple<int, string>(entry.Item2, entry.Item3);
                    stageNames.Add(aRecordofStageNames);
                }
            }

            //Reorder the stages in ascending order
            int loop = 1;
            int terminate = stageNames.Count;
            while (true)
            {
                if (loop >= terminate)
                {
                    break;
                }
                else if (stageNames[loop - 1].Item1 > stageNames[loop].Item1)
                {
                    Tuple<int, string> temp = stageNames[loop];
                    stageNames[loop] = stageNames[loop - 1];
                    stageNames[loop - 1] = temp;
                    loop = 1;
                }
                else { loop++; }
            }

            return stageNames;


        }

        public int ComputeStageNumber(int pageNumber, double noOfStagePerPage, int i)
        {
            int stageNumberText = (int)((pageNumber - 1) * noOfStagePerPage + i + 1);
            return stageNumberText;
        }

        public int DeclareStageMapButton(int stageProgress, int stageCount, int pageNumber, int i)
        {

            i = i + 9 * (pageNumber - 1) + 1;

            if (i <= stageProgress&& i<=stageCount)
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





  