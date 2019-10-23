using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomGame : MonoBehaviour
{
    /* 
     public Transform contentPanel;
     public GameObject extraContent;
     public Text stageName;
     public Sprite correctChecked;
      public GameObject plus;
     public GameObject save;
     public ArrayList questionName = new ArrayList();
     public ArrayList correctAnswers = new ArrayList();
     public ArrayList optionChoice = new ArrayList();*/
    public List<QuestionChoice> qnList = new List<QuestionChoice>();
    public static string CuststageName;
    public bool done = false;
    public static int noOfCustom;


    [SerializeField]
    GameObject panel;
    [SerializeField]
    GameObject questionBtn;

    [SerializeField]
    GameObject qnsText;
    [SerializeField]
    Button[] choices;
    [SerializeField]
    GameObject questionPopUp;
    [SerializeField]
    GameObject answerPopUp;
    [SerializeField]
    GameObject clearedPopUp;

    [SerializeField]
    Sprite answerRightQns;
    [SerializeField]
    Sprite answerWrongQns;
    [SerializeField]
    Sprite lockedQns;
    [SerializeField]
    Sprite normalQns;
    [SerializeField]
    Text stageName;
    [SerializeField]
    Text correctWrong;
    [SerializeField]
    Text bannerText;
    [SerializeField]
    Text clearedText;

    [SerializeField]
    GameObject qnsCounter;
    int qnsCounterT = 0;

    int correctQns;
    int wrongQns;

    [SerializeField]
    GameObject loading;

    List<GameObject> initializedButtons = new List<GameObject>();
    int currentQnsNumber = 0;


    // Start is called before the first frame update
    void Start()
    {
        CuststageName = StageNameManage.customName;
        //Debug.Log(stageName);
        stageName.text = CuststageName;
        StartCoroutine(ReadDB());
        questionPopUp.SetActive(false);
        answerPopUp.SetActive(false);
    }

    // Update is called once per frame



    IEnumerator ReadDB()
    {
        done = false;
        if (loading != null)
            loading.SetActive(true);
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;


        FirebaseDatabase.DefaultInstance.GetReference("CustomStage/" + CuststageName + "/").GetValueAsync().ContinueWith(task =>
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

                foreach (var questionNumber in snapshot.Children)
                {
                    Debug.LogFormat("Key={0}", questionNumber.Key); //Node at question number

                    noOfCustom = (int)questionNumber.ChildrenCount;  //noOfQuestions
                    foreach (var actualQuestionNumber in questionNumber.Children)
                    {
                        countDB = 0;
                        QuestionChoice qn = new QuestionChoice();

                        Debug.LogFormat("Key={0}", actualQuestionNumber.Key); //question 1,2.....
                        foreach (var value in actualQuestionNumber.Children)
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
                            else if (countDB == 5)
                            {
                                //questionName.Add(value.Value.ToString());
                                qn.setQns(value.Value.ToString());
                            }
                            countDB++;
                        }
                        qnList.Add(qn);

                    }
                }

                done = true;

                Debug.Log(countDB);
            }
        });

        yield return new WaitUntil(() => done == true);

        if (done == true) //reading db should be done by now
        {
            if (loading != null)
                loading.SetActive(false);
            Debug.Log(qnList.Count);
            for (int i = 0; i < qnList.Count; i++)
            {
                Debug.Log("QNS " + i.ToString() + " " + qnList[i].getQns());

                for (int j = 0; j < qnList[i].getChoices().Count; j++)
                {
                    Debug.Log("Choices " + j.ToString() + " " + qnList[i].getChoices()[j].ToString());
                }

            }
            for (int i = 0; i < qnList.Count; i++)
            {
                GameObject newButton = Instantiate(questionBtn) as GameObject;
                newButton.GetComponent<Button>().GetComponentInChildren<Text>().text = "Qns " + (i + 1).ToString();
                newButton.gameObject.name = i.ToString();
                newButton.GetComponent<Button>().GetComponent<Image>().sprite = lockedQns;
                newButton.GetComponent<Button>().interactable = false;
                newButton.GetComponent<QuestionChoice>().setOptionChoice(qnList[i].getChoices());
                qnList[i].setQnsNumber(i);
                newButton.GetComponent<QuestionChoice>().setQnsNumber(i); //qns start from 0
                newButton.GetComponent<QuestionChoice>().setAnswer(qnList[i].getAnswer());
                newButton.GetComponent<QuestionChoice>().setQns(qnList[i].getQns());
                newButton.gameObject.transform.SetParent(panel.gameObject.transform);
                newButton.gameObject.transform.localScale = new Vector3(1, 1, 1);
                initializedButtons.Add(newButton);
            }
            initializedButtons[currentQnsNumber].GetComponent<Button>().GetComponent<Image>().sprite = normalQns;
            initializedButtons[currentQnsNumber].GetComponent<Button>().interactable = true;
            qnsCounter.GetComponent<Text>().text = ("Question Cleared: " + (qnsCounterT) + "/" + qnList.Count).ToString();

            for (int i = 0; i < qnList.Count; i++)
            {
                Debug.Log("ASKJAKSJKAS " + qnList[i].getQns());
                Debug.Log("ASKJAKSJKAS " + qnList[i].getQnsNumber());

            }
            //loadDB();
            //yield return new WaitForSeconds(1f);
        }
    }
    public void CheckAns(int i)
    {
        Debug.Log(i + " " + qnList[currentQnsNumber].getAnswer());
        Debug.Log("BA " + qnList[currentQnsNumber].getQnsNumber());
        answerPopUp.SetActive(true);
        //currentQnsNumber starts from 1 on, array start from 0
        if (i == qnList[currentQnsNumber].getAnswer())
        {
            Debug.Log("Correct");
            correctQns++;
            qnList[currentQnsNumber].setAnsweredCorrectWrong(true);
            correctWrong.GetComponent<Text>().text = "Well done, you got the correct answer!";
            initializedButtons[currentQnsNumber].GetComponent<Button>().GetComponent<Image>().sprite = answerRightQns;
        }
        else
        {
            Debug.Log("Wrong");
            wrongQns++;
            qnList[currentQnsNumber].setAnsweredCorrectWrong(false);
            correctWrong.GetComponent<Text>().text = "Sorry, you got the wrong answer";
            initializedButtons[currentQnsNumber].GetComponent<Button>().GetComponent<Image>().sprite = answerWrongQns;
        }
        initializedButtons[currentQnsNumber].GetComponent<Button>().interactable = false;
        if (currentQnsNumber < initializedButtons.Count - 1) //There is a next button
        {
            initializedButtons[currentQnsNumber + 1].GetComponent<Button>().interactable = true;
            initializedButtons[currentQnsNumber + 1].GetComponent<Button>().GetComponent<Image>().sprite = normalQns;
        }
        qnsCounterT++;
        qnsCounter.GetComponent<Text>().text = ("Question Cleared: " + (qnsCounterT) + "/" + qnList.Count).ToString();
    }
    public void CheckEnd()
    {
        if (qnsCounterT == qnList.Count)
        {
            Debug.Log("Cleared");
            clearedPopUp.SetActive(true);
            clearedText.text += "Total Questions Correct: " + correctQns + "/" + qnList.Count;
            clearedText.text += "\n";
            if (correctQns > 0)
            {
                clearedText.text += "\nQuestions answered correctly: ";
                for (int i = 0; i < qnList.Count; i++)
                {
                    if (qnList[i].getAnsweredCorrecrWrong())
                    {
                        clearedText.text += ((qnList[i].getQnsNumber() + 1).ToString() + ",");
                    }
                }
            }
            if (wrongQns > 0)
            {
                clearedText.text += "\n";
                clearedText.text += "Questions answered wrongly: ";
                for (int i = 0; i < qnList.Count; i++)
                {
                    if (!qnList[i].getAnsweredCorrecrWrong())
                    {
                        clearedText.text += ((qnList[i].getQnsNumber() + 1).ToString() + ",");
                    }
                }
                clearedText.text += "\n";
            }
        }
    }
    public void loadQns()
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
        QuestionChoice qn = currentSelected.GetComponent<QuestionChoice>();
        questionPopUp.SetActive(true);
        currentQnsNumber = qn.getQnsNumber();
        bannerText.GetComponent<Text>().text = "Qns " + (qn.getQnsNumber() + 1).ToString();
        //qnsCounter.GetComponent<Text>().text = ("Current Question " + (currentQnsNumber + 1) + "/" + qnList.Count).ToString();
        qnsText.GetComponent<Text>().text = qn.getQns();
        for (int i = 0; i < qn.getChoices().Count; i++)
        {
            choices[i].gameObject.SetActive(true);
            choices[i].GetComponentInChildren<Text>().text = qn.getChoices()[i].ToString();
        }
        // Debug.Log(qn.getAnswer());

    }
    void loadDB()
    {
        /*
        int buttonImageCount = 0;
        int correctAnswer = 0;
        int counting = 0;
        for (int i = 0; i < noOfCustom; i++)
        {
            //stageName.text = arrayStageName[i].ToString();
            GameObject go = Instantiate(extraContent) as GameObject;
            //go.GetComponent<>
            go.name = (i + 1).ToString();

            go.SetActive(true);
            go.transform.SetParent(contentPanel);

        }

        foreach (Transform stageQuestion in contentPanel.transform) //noOfQUestion
        {
            string test = stageQuestion.gameObject.name;
            correctAnswer = 0;
            int count = 0;
            int count2 = 0;

            foreach (Transform inputs in stageQuestion.transform) //elements inside questions
            {
                buttonImageCount = 0;
                test = inputs.gameObject.name;

                if (count == 0)
                {
                    //test = inputs.gameObject.GetComponent<InputField>().text;
                    inputs.gameObject.GetComponent<InputField>().text = questionName[counting].ToString();// input question

                }
                else
                {
                    
                    foreach (Transform options in inputs.gameObject.transform)
                    {
                        if (buttonImageCount == 0)
                        {
                            options.gameObject.GetComponent<InputField>().text = optionChoice[counting*4+count2].ToString();  //text for option
                            
                            buttonImageCount++;
                            count2++;
                        }
                        else
                        {
                            if(count2==Int32.Parse(correctAnswers[counting].ToString()))
                            {
                                options.gameObject.GetComponent<Button>().image.sprite = correctChecked;
                            }
                            
                            

                        }

                    }
                    

                }
                
                count++;
                correctAnswer++;

            }
            counting++;
        }

        plus.SetActive(true);
        save.SetActive(true);*/
    }



}
