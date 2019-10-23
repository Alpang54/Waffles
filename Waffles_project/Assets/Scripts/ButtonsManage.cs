using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonsManage : MonoBehaviour
{
    public GameObject backButton;
    public GameObject nextButton;
    public GameObject doneButton;
    public GameObject inputStage;
    public GameObject scrollView;
    public GameObject prefRef;
    public GameObject addMoreQuestion;
    public GameObject inputQns;
    public Transform contentPanel2;
    public Button option1;
    public Button option2;
    public Button option3;
    public Button option4;
    public Sprite correctChecked;
    public Sprite normalChecked;
    public InputField stageName;
    public InputField stageQuestion;
    public InputField stageAnswer1;
    public InputField stageAnswer2;
    public InputField stageAnswer3;
    public InputField stageAnswer4;
    public int correct;
    public int newCorrect;
    public String CorrectAnswer;
    public GameObject extraQuestion;
    public List<GameObject> goQuestion;
    public int noOfQuestion;
    public ArrayList questionName = new ArrayList();
    public ArrayList correctAnswers = new ArrayList();
    public ArrayList optionChoice = new ArrayList();
    private DataHandler dataHandler;
    public void pressNext()
    {
        backButton.SetActive(!backButton.active);
        nextButton.SetActive(!nextButton.active);
        doneButton.SetActive(!doneButton.active);
        inputStage.SetActive(!inputStage.active);
        scrollView.SetActive(!scrollView.active);
        addMoreQuestion.SetActive(!addMoreQuestion.active);
    }
    public void pressBack()
    {
        backButton.SetActive(!backButton.active);
        nextButton.SetActive(!nextButton.active);
        doneButton.SetActive(!doneButton.active);
        inputStage.SetActive(!inputStage.active);
        scrollView.SetActive(!scrollView.active);
        addMoreQuestion.SetActive(!addMoreQuestion.active);
    }


    public void correctOptionCheck(Button btn)
    {
        correct = Int32.Parse(btn.name);
        // btn.image.sprite = correctChecked;

        if (correct == 1)
        {
            option2.image.sprite = normalChecked;

            option3.image.sprite = normalChecked;

            option4.image.sprite = normalChecked;

            option1.image.sprite = correctChecked;
        }
        else if (correct == 2)
        {
            option1.image.sprite = normalChecked;

            option3.image.sprite = normalChecked;

            option4.image.sprite = normalChecked;

            option2.image.sprite = correctChecked;
        }
        else if (correct == 3)
        {
            option2.image.sprite = normalChecked;

            option1.image.sprite = normalChecked;

            option4.image.sprite = normalChecked;

            option3.image.sprite = correctChecked;
        }
        else if (correct == 4)
        {
            option2.image.sprite = normalChecked;

            option3.image.sprite = normalChecked;

            option1.image.sprite = normalChecked;

            option4.image.sprite = correctChecked;
        }
        //string test = goQuestion[0].name;
        // string test=goQuestion[0].GetComponent<GameObject>().GetComponent<InputField>().text;
        //string test= GameObject.Find("1").GetComponent<InputField>().text;

        
    }

    public void pressDone()
    {
        dataHandler = GameObject.Find("DataManager").GetComponent<DataHandler>();
        string uID = dataHandler.GetFirebaseUserId();
        int buttonImageCount = 0;
        int correctAnswer = 0;
        noOfQuestion = contentPanel2.childCount;
        foreach (Transform stageQuestion in contentPanel2.transform) //noOfQUestion
        {
            string test = stageQuestion.gameObject.name;
            correctAnswer = 0;
            int count = 0;
            
            foreach (Transform inputs in stageQuestion.transform) //elements inside questions
            {
                buttonImageCount = 0;
                test = inputs.gameObject.name;
                
                    if (count == 0)
                    {
                        test = inputs.gameObject.GetComponent<InputField>().text;
                        questionName.Add(inputs.gameObject.GetComponent<InputField>().text);// input question
                        
                    }
                    else
                    {

                        foreach(Transform options in inputs.gameObject.transform)
                         {
                            if(buttonImageCount==0)
                               {
                                 test = options.gameObject.GetComponent<InputField>().text;  //text for option
                                optionChoice.Add(options.gameObject.GetComponent<InputField>().text);
                                 buttonImageCount++;
                                }
                            else
                              {
                                 if(options.gameObject.GetComponent<Button>().image.sprite==correctChecked)
                                    {
                                        correctAnswers.Add(correctAnswer); //option of correct answer
                                     }

                        }
                           
                         }


                    }
                    count++;
                correctAnswer++;
               
            }
        }
        
        DatabaseReference databaseRef;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://cz3003-waffles.firebaseio.com/");
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;


       
        databaseRef.Child("Data").Child("Custom").Child(stageName.text).SetValueAsync(stageName.text);
        databaseRef.Child("UserCustom").Child(dataHandler.GetFirebaseUserId()).Push().SetValueAsync(stageName.text);
        for (int i=0;i<noOfQuestion;i++)
        {
            databaseRef.Child("CustomStage").Child(stageName.text).Child("QuestionNumber").Child((i+1).ToString()).Child("StageName").SetValueAsync(stageName.text);
            databaseRef.Child("CustomStage").Child(stageName.text).Child("QuestionNumber").Child((i+1).ToString()).Child("Question").SetValueAsync(questionName[i]);
            databaseRef.Child("CustomStage").Child(stageName.text).Child("QuestionNumber").Child((i+1).ToString()).Child("Correct").SetValueAsync(correctAnswers[i]);
            databaseRef.Child("CustomStage").Child(stageName.text).Child("QuestionNumber").Child((i+1).ToString()).Child("1").SetValueAsync(optionChoice[i*4]);
            databaseRef.Child("CustomStage").Child(stageName.text).Child("QuestionNumber").Child((i+1).ToString()).Child("2").SetValueAsync(optionChoice[i*4+1]);
            databaseRef.Child("CustomStage").Child(stageName.text).Child("QuestionNumber").Child((i+1).ToString()).Child("3").SetValueAsync(optionChoice[i*4+2]);
            databaseRef.Child("CustomStage").Child(stageName.text).Child("QuestionNumber").Child((i+1).ToString()).Child("4").SetValueAsync(optionChoice[i*4+3]);
            
        }
        

 
    }

    public void pressPlus()
    {
        
        noOfQuestion++;
        GameObject go = Instantiate(extraQuestion) ;
        go.name = noOfQuestion.ToString();
        goQuestion.Add(go);
        go.SetActive(true);
        go.transform.SetParent(contentPanel2);
        go.gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public void pressDelete()
    {
        Destroy(prefRef);
    }
}
