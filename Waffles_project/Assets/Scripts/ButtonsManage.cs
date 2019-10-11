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

    public void pressNext()
    {
        backButton.SetActive(!backButton.active);
        nextButton.SetActive(!nextButton.active);
        doneButton.SetActive(!doneButton.active);
        inputStage.SetActive(!inputStage.active);
        scrollView.SetActive(!scrollView.active);
    }
    public void pressBack()
    {
        backButton.SetActive(!backButton.active);
        nextButton.SetActive(!nextButton.active);
        doneButton.SetActive(!doneButton.active);
        inputStage.SetActive(!inputStage.active);
        scrollView.SetActive(!scrollView.active);
    }
   

    public void correctOptionCheck(Button btn)
    {
        correct = Int32.Parse(btn.name);
       // btn.image.sprite = correctChecked;
        
        if(correct==1)
        { 
            option2.image.sprite = normalChecked;
         
            option3.image.sprite = normalChecked;
           
            option4.image.sprite = normalChecked;

            option1.image.sprite = correctChecked;
        }
        else if(correct==2)
        {
            option1.image.sprite = normalChecked;

            option3.image.sprite = normalChecked;

            option4.image.sprite = normalChecked;

            option2.image.sprite = correctChecked;
        }
        else if(correct==3)
        {
            option2.image.sprite = normalChecked;

            option1.image.sprite = normalChecked;

            option4.image.sprite = normalChecked;

            option3.image.sprite = correctChecked;
        }
        else if(correct==4)
        {
            option2.image.sprite = normalChecked;

            option3.image.sprite = normalChecked;

            option1.image.sprite = normalChecked;

            option4.image.sprite = correctChecked;
        }
        

        if(option1.image.sprite==correctChecked)
        {
            newCorrect = 1000;
        }
    }


    public void pressDone()
    {

        if (option1.image.sprite == correctChecked)
        {
            CorrectAnswer = stageAnswer1.text;
        }
        else if(option2.image.sprite == correctChecked)
        {
            CorrectAnswer = stageAnswer2.text;
        }
        else if(option3.image.sprite == correctChecked)
        {
            CorrectAnswer = stageAnswer2.text;
        }
        else
        {
            CorrectAnswer = stageAnswer2.text;
        }
        DatabaseReference databaseRef;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://cz3003-waffles.firebaseio.com/");
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
      
      
        databaseRef.Child("CustomStage").Child(stageName.text).Child("Question").SetValueAsync(stageQuestion.text);
        databaseRef.Child("CustomStage").Child(stageName.text).Child("1").SetValueAsync(stageAnswer1.text);
        databaseRef.Child("CustomStage").Child(stageName.text).Child("2").SetValueAsync(stageAnswer2.text);
        databaseRef.Child("CustomStage").Child(stageName.text).Child("3").SetValueAsync(stageAnswer3.text);
        databaseRef.Child("CustomStage").Child(stageName.text).Child("4").SetValueAsync(stageAnswer4.text);
        databaseRef.Child("CustomStage").Child(stageName.text).Child("Correct").SetValueAsync(CorrectAnswer);


       


    }
}
