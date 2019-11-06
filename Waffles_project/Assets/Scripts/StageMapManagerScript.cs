using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



/** StageMapManagerScript manages the Stage map on the application side regarding GameObjects
* @author Ang Jie Kai Alvis
**/
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


    /** Initializes the stage using values obtained from the world map.
* @params worldlevel is the world selected, worldStageNames is a list of tuples containing a world, a stage, and the stage name, worldStageProgress is a list of tuples containing a world, a stage, and the user progress for that stage
**/
    public void LoadStageMap(int worldLevel, List<Tuple<int, int, string>> worldStageNames, List<Tuple<int, int, string>> worldStageProgress)
    {
        datahandler = GameObject.Find("DataManager").GetComponent<DataHandler>();

        //default page number is 1
        this.pageNumber = 1;
        this.worldLevel = worldLevel;

        stageMapImplementor = new StageMapManagerImplementation();

        //Get stage names and progress from the parameters
        this.stageNames = stageMapImplementor.InitializeStageNames(worldStageNames, worldLevel);
        this.stageProgress = stageMapImplementor.InitializeStageProgress(worldStageProgress, worldLevel);

        //If User progress on last attempted stage is more than 70%, we allow him to proceed to next stage
        DetermineIfUserStageProgressShouldBeUpdated();
        UpdateLeftRightButtons();



        //If The number of stages in this world is 0, declare 0 stages.
        if (stageNames[0].Item1 == 0)
        {
            DeclareStageMapButtons(stageProgress.Count, 0, this.pageNumber);
        }
        else
        {
            DeclareStageMapButtons(stageProgress.Count, stageNames.Count, this.pageNumber);
        }
        
        stageSelect.SetActive(true);

    }


    /**To determine if the last attempted stage of has more than 70% completion, if so, we allow user to proceed to next stage
     **/
    private void DetermineIfUserStageProgressShouldBeUpdated()
    {
        int userStageProgress = this.stageProgress.Count;
        int temporaryStageCompletionPercentage = Int32.Parse(this.stageProgress[userStageProgress - 1].Item2);

        if (temporaryStageCompletionPercentage >= 70 && userStageProgress < this.stageNames.Count) 
        {
            Tuple<int, string> aRecordofStageProgress = new Tuple<int, string>(userStageProgress + 1, "0");
            this.stageProgress.Add(aRecordofStageProgress);
            DeclareStageMapButtons(this.stageProgress.Count, this.stageNames.Count, this.pageNumber);
        }

    }


    /**turn stage map on
     * */
    public void SetActive()
    {
        stageSelect.SetActive(true);
    }
    /** turn stage map off
     **/
    public void SetInactive()
    {
        stageSelect.SetActive(false);
    }

    /**when a stage is selected, show confirm panel
     * @params stageLevel is the stage level of the stage selected
     * */
    public void OnSelectStageButton(int stageLevel)
    {
        string stageName = this.stageNames[stageLevel - 1].Item2;
        StageConfirmPanel confirmPanel = stageConfirmPanel.GetComponent<StageConfirmPanel>();
        confirmPanel.confirmPanelAppear(stageName, worldLevel, stageLevel, this.stageProgress[stageLevel - 1].Item2);



    }

    /**when a stage is selected and confirmed, load next scene
     * @params stagelevel is the stage level of the stage selected
     * */
    public void OnSelectStagePlayButton(int stageLevel)
    {
        //change depengind on ameplay,ideally load scene

        Tuple<int, int> worldAndStageLevel = new Tuple<int, int>(this.worldLevel, stageLevel);

        datahandler.SetWorldAndStageLevel(worldAndStageLevel);
        SceneManager.LoadScene("Main Stage");
    }


    /**Shares a screenshot of the current screen to whichever platform the user chooses
     **/
    public void Share()
    {
        ShareScript sharescript = GameObject.Find("ShareHandler").GetComponent<ShareScript>();
        sharescript.Share();
    }

    /**Determine which stages are available to the user.
     * @params stageProgress is the progress of the user of a stage, stageCount is the total number of stages available, pageNumber is the current page number of the stage map
     * */
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

    /**When user clicks the next page button, increment this page number and update the page selection buttons
     * */
    public void onNextStageMapButton()
    {
        this.pageNumber++;
        UpdateLeftRightButtons();
    }

    /**When user clicks the previous page button, decrement this page number and update the page selection buttons
     * */
    public void onPreviousStageMapButton()
    {
        this.pageNumber--;
        UpdateLeftRightButtons();
    }

    /**Check if which pages user can flips to and re-declare stage buttons
     **/
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

    /** Reloads the current scene to go back to the world map
     * */
    public void BackButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}

/** StageMapManagerImplementaion implements the Stage map logic
* @author Ang Jie Kai Alvis
**/
public class StageMapManagerImplementation
    {
        List<string> stageCompletionPercentage = new List<string>();
        List<string> stageNames = new List<string>();
        int stageCount;
        int stageProgress;

    /** Returns the no of stages available
   * */
    public int GetStageCount()
        {
            return this.stageCount;
        }
    /** Returns the user progress
   * */
    public int GetStageProgress()
        {
            return this.stageProgress;
        }
    /** Returns the nane of a stage
   * */
    public List<string> GetStageNames()
        {
            return this.stageNames;
        }
    /** Returns a list of the completion percentage of the stages
   * */
    public List<string> GetStageCompletionPercentage()
        {
            return this.stageCompletionPercentage;
        }

        /**Returns the stage Progress
         * @worldStageProgress is a list of tuple containing the world, the stage, and the progress the user has on that stage. worldLevel is the world number selected previously
         * */
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

            //if there is no progress at all for this stage, set progress to 1 and give the percentage as 0;
            if (stageProgress.Count == 0)
            {
            Tuple<int, string> aRecordofStageProgress = new Tuple<int, string>(1, "0");
            stageProgress.Add(aRecordofStageProgress);

            }

            return stageProgress;
        }

    /** Returns a list of stage  and its Names from the worldStageNames
     * @params worldStageNames is a list of tuple of a world, a stage, and its names, worldlevel is the world number selected
     * */
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

    /** Computes the stage number of a button
  * @params pageNumber is the page number of the stages available in a world, no OfStagePerPage is the number of stages in a page, i is the i'th button of the stage map
  * */
    public int ComputeStageNumber(int pageNumber, double noOfStagePerPage, int i)
        {
        if (pageNumber < 1)
        {
            pageNumber = 1;
        }
        if (noOfStagePerPage < 1)
        {
           noOfStagePerPage = 9.0;
        }
        if (i < 0)
        {
            i = 0;
        }
        int stageNumberText = (int)((pageNumber - 1) * noOfStagePerPage + i + 1);
            return stageNumberText;
        }

    /** To decide if we should declare the button or not.
  * @params stageProgress is the stage where the user has progressed until, stageCount is the total number of available stages, pagenumber is the page number of the stage map, i is the i'th button of the stage map
  * */
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





  