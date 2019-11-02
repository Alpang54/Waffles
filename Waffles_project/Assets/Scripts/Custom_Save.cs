using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * This class handles editing of custom stage and upload changes to database
 * @author Ng Kai Qian
 */

public class Custom_Save : MonoBehaviour
{
    public GameObject backButton;
    public GameObject nextButton;
    public GameObject doneButton;
    public GameObject inputStage;
    public GameObject scrollView;
    public GameObject prefRef;
    public GameObject addMoreQuestion;
    public GameObject inputQns;
    public GameObject popUpError;
    public GameObject popUpComplete;
    public GameObject popupConfirm;
    public Text error;
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
    public int noOfQuestion=Edit.noOfCustom;
    public static string custStageName;
    public ArrayList questionName = new ArrayList();
    public ArrayList correctAnswers = new ArrayList();
    public ArrayList optionChoice = new ArrayList();

   
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
        custStageName = StageNameManage.customName;
        int dbQuestion = Edit.noOfCustom;
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



                foreach (Transform options in inputs.gameObject.transform)
                {
                    if (buttonImageCount == 0)
                    {
                        if (count == 0)
                        {
                            questionName.Add(options.gameObject.GetComponent<InputField>().text);
                            buttonImageCount++;
                        }

                        else
                        {
                            optionChoice.Add(options.gameObject.GetComponent<InputField>().text);
                            buttonImageCount++;
                        }


                    }
                    else
                    {
                        if (options.gameObject.GetComponent<Button>().image.sprite == correctChecked)
                        {
                            correctAnswers.Add(correctAnswer); //option of correct answer

                        }


                    }

                }






                count++;
                correctAnswer++;

            }
        }

        DatabaseReference databaseRef;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log(custStageName);

        

        for (int i = 0; i < noOfQuestion; i++)
        {
            databaseRef.Child("CustomStage").Child(custStageName).Child("QuestionNumber").Child((i + 1).ToString()).Child("StageName").SetValueAsync(custStageName);
            databaseRef.Child("CustomStage").Child(custStageName).Child("QuestionNumber").Child((i + 1).ToString()).Child("Question").SetValueAsync(questionName[i]);
            databaseRef.Child("CustomStage").Child(custStageName).Child("QuestionNumber").Child((i + 1).ToString()).Child("Correct").SetValueAsync(correctAnswers[i]);
            databaseRef.Child("CustomStage").Child(custStageName).Child("QuestionNumber").Child((i + 1).ToString()).Child("1").SetValueAsync(optionChoice[i * 4]);
            databaseRef.Child("CustomStage").Child(custStageName).Child("QuestionNumber").Child((i + 1).ToString()).Child("2").SetValueAsync(optionChoice[i * 4 + 1]);
            databaseRef.Child("CustomStage").Child(custStageName).Child("QuestionNumber").Child((i + 1).ToString()).Child("3").SetValueAsync(optionChoice[i * 4 + 2]);
            databaseRef.Child("CustomStage").Child(custStageName).Child("QuestionNumber").Child((i + 1).ToString()).Child("4").SetValueAsync(optionChoice[i * 4 + 3]);
            
            
            
        }

        if(noOfQuestion<dbQuestion)
        {
            for(int i=dbQuestion;i>noOfQuestion;i--)
            {
                databaseRef.Child("CustomStage").Child(custStageName).Child("QuestionNumber").Child(i.ToString()).RemoveValueAsync();
            }
                    
        }

        popUpComplete.SetActive(true);

        optionChoice.Clear();
        correctAnswers.Clear();
        questionName.Clear();
    }

    public void pressPlus()
    {

        int buttonImageCount = 0;
        int correctAnswer = 0;
        bool filled = true;
        int neverTick = 0;
        int correctTick = 0;
        noOfQuestion = contentPanel2.childCount;
        if (noOfQuestion == 0)
        {
            GameObject go = Instantiate(extraQuestion);
            go.name = noOfQuestion.ToString();
            goQuestion.Add(go);
            go.SetActive(true);
            go.transform.SetParent(contentPanel2);
            go.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            foreach (Transform stageQuestion in contentPanel2.transform) //noOfQUestion
            {
                string test = stageQuestion.gameObject.name;
                correctAnswer = 0;
                int count = 0;

                foreach (Transform inputs in stageQuestion.transform) //elements inside questions
                {
                    buttonImageCount = 0;
                    test = inputs.gameObject.name;



                    foreach (Transform options in inputs.gameObject.transform)
                    {
                        if (buttonImageCount == 0)
                        {
                            test = options.gameObject.GetComponent<InputField>().text;  //text for option
                            if (string.IsNullOrEmpty(test) == true)
                            {
                                filled = false;
                            }
                            buttonImageCount++;
                        }
                        else
                        {
                            if (options.gameObject.GetComponent<Button>().image.sprite == correctChecked)
                            {

                                correctTick++;
                            }


                        }

                    }


                    count++;
                    correctAnswer++;

                }




            }

            if (correctTick != noOfQuestion)
                filled = false;



            if (filled == true)
            {
                GameObject go = Instantiate(extraQuestion);
                go.name = noOfQuestion.ToString();
                goQuestion.Add(go);
                go.SetActive(true);
                go.transform.SetParent(contentPanel2);
                go.gameObject.transform.localScale = new Vector3(1, 1, 1);

            }
            else
            {
                if (correctTick != noOfQuestion)
                {
                    error.text = ("Please Tick One Of The Options");
                    popUpError.SetActive(true);
                }
                else
                {
                    error.text = ("One Or More Of The InputField(s) Is Empty");
                    popUpError.SetActive(true);
                }

            }
        }

    }


    public void prompAndCheck()
    {
        noOfQuestion = contentPanel2.childCount;
        if (noOfQuestion == 0)
        {
            error.text = ("Please Add At Least One Question");
            popUpError.SetActive(true);

        }
        else
        {

            int buttonImageCount = 0;
            int correctAnswer = 0;
            bool filled = true;
            int neverTick = 0;
            int correctTick = 0;



            foreach (Transform stageQuestion in contentPanel2.transform) //noOfQUestion
            {
                string test = stageQuestion.gameObject.name;
                correctAnswer = 0;
                int count = 0;

                foreach (Transform inputs in stageQuestion.transform) //elements inside questions
                {
                    buttonImageCount = 0;
                    test = inputs.gameObject.name;



                    foreach (Transform options in inputs.gameObject.transform)
                    {
                        if (buttonImageCount == 0)
                        {
                            test = options.gameObject.GetComponent<InputField>().text;  //text for option
                            if (string.IsNullOrEmpty(test) == true)
                            {
                                filled = false;
                            }
                            buttonImageCount++;
                        }
                        else
                        {
                            if (options.gameObject.GetComponent<Button>().image.sprite == correctChecked)
                            {

                                correctTick++;
                            }


                        }


                    }


                }
                count++;
                correctAnswer++;






            }

            if (correctTick != noOfQuestion)
                filled = false;



            if (filled == true)
            {
                popupConfirm.SetActive(true);
            }
            else
            {
                if (correctTick != noOfQuestion)
                {
                    error.text = ("Please Tick One Of The Options");
                    popUpError.SetActive(true);
                }
                else
                {
                    error.text = ("One Or More Of The InputField(s) Is Empty");
                    popUpError.SetActive(true);
                }

            }


        }
    }
}
