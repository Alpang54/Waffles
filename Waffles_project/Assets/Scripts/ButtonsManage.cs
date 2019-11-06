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
 * This class will handle all buttons clicks for Create Custom Scene and do their functions inside as well as logic testing
 * @author Ng Kai Qian
 */

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
    public GameObject popUpError;
    public GameObject popUpComplete;
    public GameObject popupConfirm;
    public Text error;
    public Text completeText;
    public GameObject stgWordCount;
    public GameObject stgMaxCount;
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
    public ArrayList dbStgName = new ArrayList();
    private DataHandler dataHandler;
    private bool done = false;
    private int dbStg;


    /**If User press next button*/
    public void pressNext()
    {
        if(string.IsNullOrEmpty(inputStage.GetComponent<InputField>().text.ToString()))  //Check if any stage name is entered
        {
            error.text = ("Please Enter Stage Name");
            popUpError.SetActive(true);
        }
        else  //If it's not empty or null
        {

            StartCoroutine(ReadDup()); //Start Coroutine to read all stage names inside db
            
            
        }
        
    }

    /**Allows the user decides to choose another custom stage name again*/
    public void pressBack() 
    {
        backButton.SetActive(!backButton.active);
        nextButton.SetActive(!nextButton.active);
        doneButton.SetActive(!doneButton.active);
        inputStage.SetActive(!inputStage.active);
        scrollView.SetActive(!scrollView.active);
        stgMaxCount.SetActive(!stgMaxCount.active);
        stgWordCount.SetActive(!stgWordCount.active);
        addMoreQuestion.SetActive(!addMoreQuestion.active);
    }

    /**Checks which options is being checked by user*/
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



    }

    /** Upload the custom stage data to DB */
    public void pressDone()  
    {
        dataHandler = GameObject.Find("DataManager").GetComponent<DataHandler>();
        string uID = dataHandler.GetFirebaseUserId();
        int buttonImageCount = 0;
        int correctAnswer = 0;
        int correcttick=0;
       

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
                        if(count==0)
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



            databaseRef.Child("Data").Child("Custom").Child(stageName.text).SetValueAsync(stageName.text);
            databaseRef.Child("UserCustom").Child(dataHandler.GetFirebaseUserId()).Child(stageName.text).SetValueAsync(stageName.text);
            for (int i = 0; i < noOfQuestion; i++)
            {
                databaseRef.Child("CustomStage").Child(stageName.text).Child("QuestionNumber").Child((i + 1).ToString()).Child("StageName").SetValueAsync(stageName.text);
                databaseRef.Child("CustomStage").Child(stageName.text).Child("QuestionNumber").Child((i + 1).ToString()).Child("Question").SetValueAsync(questionName[i]);
                databaseRef.Child("CustomStage").Child(stageName.text).Child("QuestionNumber").Child((i + 1).ToString()).Child("Correct").SetValueAsync(correctAnswers[i]);
                databaseRef.Child("CustomStage").Child(stageName.text).Child("QuestionNumber").Child((i + 1).ToString()).Child("1").SetValueAsync(optionChoice[i * 4]);
                databaseRef.Child("CustomStage").Child(stageName.text).Child("QuestionNumber").Child((i + 1).ToString()).Child("2").SetValueAsync(optionChoice[i * 4 + 1]);
                databaseRef.Child("CustomStage").Child(stageName.text).Child("QuestionNumber").Child((i + 1).ToString()).Child("3").SetValueAsync(optionChoice[i * 4 + 2]);
                databaseRef.Child("CustomStage").Child(stageName.text).Child("QuestionNumber").Child((i + 1).ToString()).Child("4").SetValueAsync(optionChoice[i * 4 + 3]);

            }

        //  completeText.text = stageName.text;
        completeText.text = "Custom Stage "+stageName.text.ToString()+" Created!";
        popUpComplete.SetActive(true);
        
       





    }

    /** Instantiate new questions if user decide to add more questions */
    public void pressPlus()  
    {
        int buttonImageCount = 0;
        int correctAnswer = 0;
        bool filled = true;
        int neverTick = 0;
        int correctTick = 0;
        noOfQuestion = contentPanel2.childCount;
        if(noOfQuestion==0)
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
            
    

            if(filled==true)
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
                if(correctTick!=noOfQuestion)
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


    /** Delete questions */
    public void pressDelete() 
    {
        noOfQuestion--;
        Destroy(prefRef);
    }


    /**Error checking for input values */
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


    /**Read all custom stage name inside DB*/
    IEnumerator ReadDup() 
    {
        done = false;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        


        FirebaseDatabase.DefaultInstance.GetReference("CustomStage").GetValueAsync().ContinueWith(task =>
        {
            //reference.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {


                DataSnapshot snapshot = task.Result;
                dbStg = (int)snapshot.ChildrenCount; 

                foreach (var stages in snapshot.Children)
                {
                    Debug.LogFormat("Key={0}", stages.Key); //Node Custom Stage Name
                    dbStgName.Add(stages.Key.ToString());


                }
                done = true;
                //strJson =snapshot.GetRawJsonValue();

            }
        });

        yield return new WaitUntil(() => done == true);



        if (done == true) //reading db should be done by now
        {
            bool found = false;
            for (int i=0;i<dbStg;i++)
            {
                if(String.Equals(dbStgName[i],stageName.text)) //Check if there's existing stage name in db
                {
                    found = true;
                    break;
                }
            }
            if(found==true)//If there's existing sane stage name pop up error
            {
                error.text = ("Stage Name Exists! Please Choose Another Stage Name"); 
                popUpError.SetActive(true); 
            }
            else  //Proceed
            {
                backButton.SetActive(!backButton.active);
                nextButton.SetActive(!nextButton.active);
                doneButton.SetActive(!doneButton.active);
                stgMaxCount.SetActive(!stgMaxCount.active);
                stgWordCount.SetActive(!stgWordCount.active);
                inputStage.SetActive(!inputStage.active);
                scrollView.SetActive(!scrollView.active);
                addMoreQuestion.SetActive(!addMoreQuestion.active);
                //yield return new WaitForSeconds(1f);
            }

        }


    }


    /**count the number of characters inside the stage name*/
    public void Text_Changed(string newText)
    {
        // backButton.SetActive(true);
        int textCount = newText.Length;
        stgWordCount.GetComponent<Text>().text=textCount.ToString();
        
    }

}
