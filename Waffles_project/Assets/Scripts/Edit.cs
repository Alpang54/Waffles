using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class populates the data retrieve from the DB when the user selects a stage
 * @author Ng Kai Qian
 */
public class Edit : MonoBehaviour
{
    public bool done = false;
    public Transform contentPanel;
    public GameObject extraContent;
    public Text stageName;
    public static int noOfCustom;
    public static string CuststageName;
    public Sprite correctChecked;
    public ArrayList questionName = new ArrayList();
    public ArrayList correctAnswers = new ArrayList();
    public ArrayList optionChoice = new ArrayList();
    public GameObject plus;
    public GameObject save;


    // Start is called before the first frame update
    void Start()
    {
        CuststageName = StageNameManage.customName;
        Debug.Log(stageName);

        StartCoroutine(ReadDB()); //Starts corountine to read db
    }

    // Update is called once per frame
    


    IEnumerator ReadDB() /**Read db for selected custom stage name for editing*/
    {
        done = false;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;


        FirebaseDatabase.DefaultInstance.GetReference("CustomStage/"+CuststageName+"/").GetValueAsync().ContinueWith(task =>
        {
            //reference.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                int  countDB = 0;

                DataSnapshot snapshot = task.Result;
                
               

                foreach (var questionNumber in snapshot.Children)
                {
                    Debug.LogFormat("Key={0}", questionNumber.Key); //Node at question number
                    noOfCustom = (int)questionNumber.ChildrenCount;  //noOfQuestions
                    foreach (var actualQuestionNumber in questionNumber.Children)
                    {
                        countDB = 0;
                        Debug.LogFormat("Key={0}", actualQuestionNumber.Key); //question 1,2.....
                        foreach(var value in actualQuestionNumber.Children)
                        {
                            Debug.LogFormat("Value={0}", value.Value.ToString()); //values inside question 1,2
                            if(countDB>=0&&countDB<=3)
                                optionChoice.Add(value.Value.ToString());
                            else if(countDB==4)
                                correctAnswers.Add(value.Value.ToString());
                            else if(countDB==5)
                                questionName.Add(value.Value.ToString());
                            countDB++;
                        }
                    }

                    
                }
                
                done = true;
                
                
            }
        });

        yield return new WaitUntil(() => done == true);



        if (done == true) //reading db should be done by now
        {
            loadDB();
            //yield return new WaitForSeconds(1f);
        }


    }

    void loadDB()  /*Instantiate the number of questions based on DB and load the values to display to user*/
    {
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
            go.transform.localScale = new Vector3(1, 1, 1);

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



                foreach (Transform options in inputs.gameObject.transform)
                {
                    if (buttonImageCount == 0)
                    {
                        if (count == 0)
                        {
                            options.gameObject.GetComponent<InputField>().text = questionName[counting].ToString();// input question
                            //count2++;
                            buttonImageCount++;
                        }

                        else
                        {
                            options.gameObject.GetComponent<InputField>().text = optionChoice[counting * 4 + count2].ToString();  //text for option

                            buttonImageCount++;
                            count2++;
                        }


                    }
                    else
                    {
                        if (count2 == Int32.Parse(correctAnswers[counting].ToString()))
                        {
                            options.gameObject.GetComponent<Button>().image.sprite = correctChecked;
                        }


                    }

                }




             
                count++;
                correctAnswer++;

            }
            counting++;
        }

        plus.SetActive(true);
        save.SetActive(true);
    }


   
}
