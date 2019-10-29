using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainStageGame : MonoBehaviour
{
    private DataHandler dataHandler;
    [SerializeField]
    GameObject loading;
    [SerializeField]
    GameObject qnsText;
    [SerializeField]
    Button[] choices;
    [SerializeField]
    GameObject answerPopUp;
    [SerializeField]
    GameObject questionPopUp;
    [SerializeField]
    GameObject clearedPopUp;
    [SerializeField]
    Text bannerText;
    [SerializeField]
    Text correctWrong;
    [SerializeField]
    Sprite answerRightQns;
    [SerializeField]
    Sprite answerWrongQns;
    [SerializeField]
    Text clearedText;
    [SerializeField]
    Text difficultyText;
    DatabaseReference reference;
    private string firebaseUserID;
    private string fBUsername;
    bool done;
    int currentWorld;
    int currentStage;
    int currentDiff;
    int percentageRight;
    private double startTime, timeTaken;
    int points = 0;
    bool right = false;
    int consecutiveRights = 0;

    //Fetching
    public List<QuestionChoice> qnDiff1List = new List<QuestionChoice>();
    public List<QuestionChoice> qnDiff2List = new List<QuestionChoice>();
    public List<QuestionChoice> qnDiff3List = new List<QuestionChoice>();
    public List<QuestionChoice> qnDiff4List = new List<QuestionChoice>();
    public List<QuestionChoice> qnDiff5List = new List<QuestionChoice>();

    public List<QuestionChoice> answeredList = new List<QuestionChoice>();
    public List<string> rightList = new List<string>();
    public List<string> wrongList = new List<string>();

    public GameObject currentSelected;
    public GameObject[] patterns;
    GameObject selectedPattern;
    List<GameObject> allButtons = new List<GameObject>();
    int correctQns = 0;
    int wrongQns = 0;
    int totalQnsAnswered = 0;
    int row = 0;
    string stageName = "";
    // Start is called before the first frame update
    void Start()
    {
        dataHandler = GameObject.Find("DataManager").GetComponent<DataHandler>();     
        firebaseUserID = dataHandler.GetFirebaseUserId();
        Debug.Log(firebaseUserID);
        fBUsername = dataHandler.GetFBUserName();
        Debug.Log(fBUsername);
       
        Debug.Log("currentDiff=" + dataHandler.GetCharacterLevel());

        Tuple<int, int> worldAndStageLevel = dataHandler.GetWorldAndStageLevel();
    
        currentDiff = dataHandler.GetCharacterLevel()+1;

      
        currentStage = worldAndStageLevel.Item2;
        currentWorld = worldAndStageLevel.Item1;
        Debug.Log("currentStage=" + currentStage);
        Debug.Log("currentWorld=" + currentWorld);
        stageName = dataHandler.GetStageName();
        stageName = "World" + currentWorld + " Stage" + currentStage;
        StartCoroutine(ReadDB());
        StartCoroutine(ReadPoint());

      
        bannerText.text = stageName;
        foreach (GameObject temp in patterns)
        {
            temp.SetActive(false);
        }
        int random = UnityEngine.Random.Range(0, (patterns.Length - 1));
        Debug.Log(random);
        selectedPattern = patterns[random];
        selectedPattern.SetActive(true);
        foreach (Transform patternBtn in selectedPattern.GetComponentInChildren<Transform>())
        {
            allButtons.Add(patternBtn.gameObject);
        }
        difficultyText.text = "Current Difficulty: " + currentDiff.ToString();

    }

    IEnumerator ReadPoint()
    {
        done = false;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;


        FirebaseDatabase.DefaultInstance.GetReference("Leaderboard/" + firebaseUserID).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                int countDB = 0;

                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    foreach (var user in snapshot.Children)
                    {
                        if (user.Key == "Score")
                            points = int.Parse(user.Value.ToString());

                    }
                }
            }
        });

        yield return new WaitUntil(() => done == true);

        Debug.Log(points);
    }
    IEnumerator ReadDB()
    {
        done = false;
        if (loading != null)
            loading.SetActive(true);

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;


        FirebaseDatabase.DefaultInstance.GetReference("StageNames/World" + currentWorld + "/Stage" + currentStage + "/Difficulty").GetValueAsync().ContinueWith(task =>
        {
            //reference.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                int countDB = 0;

                DataSnapshot snapshot = task.Result;

                foreach (var questionDifficulty in snapshot.Children)
                {
                    Debug.LogFormat("Count={0}", questionDifficulty.ChildrenCount); //Node at question number
                    int difficulty = int.Parse(questionDifficulty.Key);
                    
                    //noOfCustom = (int)questionDifficulty.ChildrenCount;  //noOfQuestions
                    foreach (var actualQuestion in questionDifficulty.Children)
                    {
                        countDB = 0;
                        QuestionChoice qn = new QuestionChoice();
                        qn.setDifficulty(difficulty);
                        qn.setQns(actualQuestion.Key);
                        Debug.LogFormat("Key={0}", actualQuestion.Key); //question 1,2.....
                        foreach (var value in actualQuestion.Children)
                        {

                            Debug.LogFormat("Value={0}", value.Value.ToString()); //values inside question 1,2
                            if (countDB >= 0 && countDB <= 3)
                            {
                                //optionChoice.Add(value.Value.ToString());
                                qn.addOptionChoice(value.Value.ToString());
                            }
                            else if (countDB == 4)
                            {
                                //correctAnswers.Add(value.Value.ToString());
                                qn.setAnswer(int.Parse(value.Value.ToString()));
                                Debug.Log(value.Value.ToString());
                            }
                            countDB++;
                        }
                        if(difficulty== 1)
                            qnDiff1List.Add(qn);
                        if (difficulty == 2)
                            qnDiff2List.Add(qn);
                        if (difficulty == 3)
                            qnDiff3List.Add(qn);
                        if (difficulty == 4)
                            qnDiff4List.Add(qn);
                        if (difficulty == 5)
                            qnDiff5List.Add(qn);
                    }
                }

                done = true;

                Debug.Log(countDB);
            }
        });

        yield return new WaitUntil(() => done == true);

        if (done == true) //reading db should be done by now
        {
            startTime = Time.time;

            for (int i = 0; i < qnDiff1List.Count; i++)
            {
                Debug.Log("Diff 1 " + qnDiff1List[i].getQns());
              
            }
            for (int i = 0; i < qnDiff2List.Count; i++)
            {
                Debug.Log("Diff 2 " + qnDiff2List[i].getQns());
               
            }

        }
    }
    public void CheckEnd()
    {
        timeTaken = Time.time - startTime;

        totalQnsAnswered = correctQns + wrongQns;
        clearedPopUp.SetActive(true);
        double divide = ((double)correctQns / (double)totalQnsAnswered);
        Debug.Log(divide);
        percentageRight = (int)(divide * 100.0);
        Debug.Log(percentageRight);
        clearedText.text += "Total Questions Correct: " + correctQns + "/" + totalQnsAnswered;
        clearedText.text += "\n";
        clearedText.text += "Points earned so far: " + points;

        if (percentageRight >= 70)
        {
            clearedText.text += "\n";
            clearedText.text += "You have unlocked the next stage!";
        }
        else
        {
            clearedText.text += "\n";
            clearedText.text += "You have not done well enough to unlock the next stage\nTry harder!";
        }
        UpdateProgress();
        StoreStats();
    }
    
    public void UpdateProgress()
    {
        string world = "World" + currentWorld;
        string stage = "Stage" + currentStage;
        reference.Child("Progress").Child(firebaseUserID).Child(world).Child(stage).SetValueAsync(percentageRight);
        Debug.Log(world + " " + stage);

    }
    public void StoreStats()
    {
        string world = "World" + currentWorld + " Stage1";
        reference.Child("Leaderboard").Child(firebaseUserID).Child("Name").SetValueAsync(fBUsername);
        reference.Child("Leaderboard").Child(firebaseUserID).Child("Score").SetValueAsync(points);
        double timeTakenPer = Math.Round((timeTaken / totalQnsAnswered), 2);
        timeTaken = Math.Round(timeTaken, 2);
        reference.Child("Data").Child("Main").Child(world).Child(firebaseUserID).Child("timeTakenPer").SetValueAsync(timeTakenPer + " secs");
        reference.Child("Data").Child("Main").Child(world).Child(firebaseUserID).Child("totalTimeTaken").SetValueAsync(timeTaken + " secs");
        reference.Child("Data").Child("Main").Child(world).Child(firebaseUserID).Child("attemptedTimestamp").SetValueAsync(Firebase.Database.ServerValue.Timestamp);
        reference.Child("Data").Child("Main").Child(world).Child(firebaseUserID).Child("fbUsername").SetValueAsync(fBUsername);
        reference.Child("Data").Child("Main").Child(world).Child(firebaseUserID).Child("noRight").SetValueAsync(correctQns);
        reference.Child("Data").Child("Main").Child(world).Child(firebaseUserID).Child("noWrong").SetValueAsync(wrongQns);
        reference.Child("Data").Child("Main").Child(world).Child(firebaseUserID).Child("totalQns").SetValueAsync(totalQnsAnswered);

    }
    public void generateRdmQuestion(GameObject btn)
    {
        int random;
        if(currentDiff == 1)
        {
            while(true)
            {
                random = UnityEngine.Random.Range(0, (qnDiff1List.Count));
                if (!qnDiff1List[random].getCleared())
                    break;
            }
            Debug.Log(random);
            btn.GetComponent <QuestionChoice>().setQns(qnDiff1List[random].getQns());
            btn.GetComponent<QuestionChoice>().setDifficulty(qnDiff1List[random].getDifficulty());
            btn.GetComponent<QuestionChoice>().setOptionChoice(qnDiff1List[random].getChoices());
            btn.GetComponent<QuestionChoice>().setAnswer(qnDiff1List[random].getAnswer());
            btn.GetComponent<QuestionChoice>().setQnsNumber(random);
            Debug.Log(qnDiff1List[random].getQns());
        }
        if (currentDiff == 2)
        {
            while (true)
            {
                random = UnityEngine.Random.Range(0, (qnDiff2List.Count));
                if (!qnDiff2List[random].getCleared())
                    break;
            }
            Debug.Log(random);
            btn.GetComponent<QuestionChoice>().setQns(qnDiff2List[random].getQns());
            btn.GetComponent<QuestionChoice>().setDifficulty(qnDiff2List[random].getDifficulty());
            btn.GetComponent<QuestionChoice>().setOptionChoice(qnDiff2List[random].getChoices());
            btn.GetComponent<QuestionChoice>().setAnswer(qnDiff2List[random].getAnswer());
            btn.GetComponent<QuestionChoice>().setQnsNumber(random);
            Debug.Log(qnDiff2List[random].getQns());
        }
        if (currentDiff == 3)
        {
            while (true)
            {
                random = UnityEngine.Random.Range(0, (qnDiff3List.Count));
                if (!qnDiff2List[random].getCleared())
                    break;
            }
            Debug.Log(random);
            btn.GetComponent<QuestionChoice>().setQns(qnDiff3List[random].getQns());
            btn.GetComponent<QuestionChoice>().setDifficulty(qnDiff3List[random].getDifficulty());
            btn.GetComponent<QuestionChoice>().setOptionChoice(qnDiff3List[random].getChoices());
            btn.GetComponent<QuestionChoice>().setAnswer(qnDiff3List[random].getAnswer());
            btn.GetComponent<QuestionChoice>().setQnsNumber(random);
            Debug.Log(qnDiff3List[random].getQns());
        }
        if (currentDiff == 4)
        {
            while (true)
            {
                random = UnityEngine.Random.Range(0, (qnDiff4List.Count));
                if (!qnDiff2List[random].getCleared())
                    break;
            }
            Debug.Log(random);
            btn.GetComponent<QuestionChoice>().setQns(qnDiff4List[random].getQns());
            btn.GetComponent<QuestionChoice>().setDifficulty(qnDiff4List[random].getDifficulty());
            btn.GetComponent<QuestionChoice>().setOptionChoice(qnDiff4List[random].getChoices());
            btn.GetComponent<QuestionChoice>().setAnswer(qnDiff4List[random].getAnswer());
            btn.GetComponent<QuestionChoice>().setQnsNumber(random);
            Debug.Log(qnDiff4List[random].getQns());
        }
        if (currentDiff == 5)
        {
            while (true)
            {
                random = UnityEngine.Random.Range(0, (qnDiff5List.Count));
                if (!qnDiff2List[random].getCleared())
                    break;
            }
            Debug.Log(random);
            btn.GetComponent<QuestionChoice>().setQns(qnDiff5List[random].getQns());
            btn.GetComponent<QuestionChoice>().setDifficulty(qnDiff5List[random].getDifficulty());
            btn.GetComponent<QuestionChoice>().setOptionChoice(qnDiff5List[random].getChoices());
            btn.GetComponent<QuestionChoice>().setAnswer(qnDiff5List[random].getAnswer());
            btn.GetComponent<QuestionChoice>().setQnsNumber(random);
            Debug.Log(qnDiff5List[random].getQns());
        }
        loadQns(btn);
    }
    public void UpdateButtonCondition(int i)
    {
        int difficulty = currentSelected.GetComponent<QuestionChoice>().getDifficulty();
        int qnsNumber = currentSelected.GetComponent<QuestionChoice>().getQnsNumber();
        Debug.Log(qnsNumber);

        if (CheckAns(i))
        {
            Debug.Log("Correct");
            correctQns++;
            consecutiveRights++;
            if (consecutiveRights>=2)
            {
                currentDiff++;
                difficultyText.text = "Current Difficulty: " + currentDiff.ToString();
                consecutiveRights = 0;
            }
            if(difficulty==1)
            {
                points += 10;
                qnDiff1List[qnsNumber].setAnsweredCorrectWrong(true);
                qnDiff1List[qnsNumber].setCleared(true);
                answeredList.Add(qnDiff1List[qnsNumber]);
            }
            if (difficulty == 2)
            {
                points += 20;
                qnDiff2List[qnsNumber].setAnsweredCorrectWrong(true);
                qnDiff2List[qnsNumber].setCleared(true);
                answeredList.Add(qnDiff2List[qnsNumber]);
            }
            if (difficulty == 3)
            {
                points += 30;
                qnDiff3List[qnsNumber].setAnsweredCorrectWrong(true);
                qnDiff3List[qnsNumber].setCleared(true);
                answeredList.Add(qnDiff3List[qnsNumber]);
            }
            if (difficulty == 4)
            {
                points += 40;
                qnDiff4List[qnsNumber].setAnsweredCorrectWrong(true);
                qnDiff4List[qnsNumber].setCleared(true);
                answeredList.Add(qnDiff4List[qnsNumber]);
            }
            if (difficulty == 5)
            {
                points += 50;
                qnDiff5List[qnsNumber].setAnsweredCorrectWrong(true);
                qnDiff5List[qnsNumber].setCleared(true);
                answeredList.Add(qnDiff5List[qnsNumber]);
            }

            correctWrong.GetComponent<Text>().text = "Well done, you got the correct answer!";
            currentSelected.GetComponent<Button>().GetComponent<Image>().sprite = answerRightQns;
        }
        else
        {
            Debug.Log("Wrong");
            wrongQns++;
            consecutiveRights = 0;
            if (difficulty == 1)
            {
                qnDiff1List[qnsNumber].setAnsweredCorrectWrong(false);
                qnDiff1List[qnsNumber].setCleared(true);
                answeredList.Add(qnDiff1List[qnsNumber]);
            }
            if (difficulty == 2)
            {
                qnDiff2List[qnsNumber].setAnsweredCorrectWrong(false);
                qnDiff2List[qnsNumber].setCleared(true);
                answeredList.Add(qnDiff2List[qnsNumber]);
            }
            if (difficulty == 3)
            {
                qnDiff3List[qnsNumber].setAnsweredCorrectWrong(false);
                qnDiff3List[qnsNumber].setCleared(true);
                answeredList.Add(qnDiff3List[qnsNumber]);
            }
            if (difficulty == 4)
            {
                qnDiff4List[qnsNumber].setAnsweredCorrectWrong(false);
                qnDiff4List[qnsNumber].setCleared(true);
                answeredList.Add(qnDiff4List[qnsNumber]);
            }
            if (difficulty == 5)
            {
                qnDiff5List[qnsNumber].setAnsweredCorrectWrong(false);
                qnDiff5List[qnsNumber].setCleared(true);
                answeredList.Add(qnDiff5List[qnsNumber]);
            }
            correctWrong.GetComponent<Text>().text = "Sorry, you got the wrong answer";
            currentSelected.GetComponent<Button>().GetComponent<Image>().sprite = answerWrongQns;
        }
        if (answerPopUp != null)
            answerPopUp.SetActive(true);

        GameObject temp = currentSelected;
        currentSelected.GetComponent<Button>().interactable = false;
        foreach (GameObject a in allButtons)
        {
            a.GetComponent<Button>().interactable = false;
        }

        if (temp.GetComponent<MainStageBtns>().getPossibleRoute().Length > 0)
        {
            foreach (GameObject a in temp.GetComponent<MainStageBtns>().getPossibleRoute())
            {
                a.GetComponent<Button>().interactable = true;
            }
            for (int j = 0; j < qnDiff1List.Count; j++)
            {
                Debug.Log("qns " + qnDiff1List[j].getQns());

            }
        }
        else
        {
            CheckEnd();
        }
        /* if (initializedButtons.Count > 0)
         {
             initializedButtons[currentQnsNumber].GetComponent<Button>().interactable = false;
             if (currentQnsNumber < initializedButtons.Count - 1) //There is a next button
             {
                 initializedButtons[currentQnsNumber + 1].GetComponent<Button>().interactable = true;
                 initializedButtons[currentQnsNumber + 1].GetComponent<Button>().GetComponent<Image>().sprite = normalQns;
             }
         }
         if (answerPopUp != null)
             answerPopUp.SetActive(true);
         qnsCounterT++;
         if (qnsCounter != null)
             qnsCounter.GetComponent<Text>().text = ("Question Cleared: " + (qnsCounterT) + "/" + qnList.Count).ToString();*/
    }
    public bool CheckAns(int i)
    {
        int difficulty = currentSelected.GetComponent<QuestionChoice>().getDifficulty();
        int qnsNumber = currentSelected.GetComponent<QuestionChoice>().getQnsNumber();
        if(difficulty == 1)
        {
            if (i == qnDiff1List[qnsNumber].getAnswer())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (difficulty==2)
        {
            if (i == qnDiff2List[qnsNumber].getAnswer())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (difficulty == 3)
        {
            if (i == qnDiff3List[qnsNumber].getAnswer())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (difficulty == 4)
        {
            if (i == qnDiff4List[qnsNumber].getAnswer())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (i == qnDiff5List[qnsNumber].getAnswer())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void loadQns(GameObject btn)
    {
        //Question pop up
        currentSelected = btn;
        QuestionChoice qn = btn.GetComponent<QuestionChoice>();
        Debug.Log(btn.GetComponent<QuestionChoice>().getQns());
        questionPopUp.SetActive(true);
        //currentQnsNumber = qn.getQnsNumber();
        //bannerText.GetComponent<Text>().text = "Qns " + (qn.getQnsNumber() + 1).ToString();
        qnsText.GetComponent<Text>().text = qn.getQns();
        for (int i = 0; i < qn.getChoices().Count; i++)
        {
            choices[i].gameObject.SetActive(true);
            choices[i].GetComponentInChildren<Text>().text = qn.getChoices()[i].ToString();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
