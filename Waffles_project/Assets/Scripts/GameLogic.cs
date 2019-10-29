﻿using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class GameLogic : MonoBehaviour
{
    private DataSnapshot snapshot;
	public Text QuestionField, Option1,Option2,Option3,Option4,ScoreText,PassFail;
    public int currentDiff;
    public int currentWorld;
    public int currentStage;
    public int correctAns = 0;
    public string userToken = "0nVjQmDgZtcSG0tvkLZ5saGeTdC2";
    public int currentProgress=0;
    public int score = 0, scoreCheck=0;
    public int[,] questions = new int[20,20];
    private int childcount=0;
    private DataHandler datahandler;
    void Start()
    {
        datahandler = GameObject.Find("DataManager").GetComponent<DataHandler>();
        Tuple<int, int> worldAndStageLevel=datahandler.GetWorldAndStageLevel();
        Debug.Log("currentStage="+worldAndStageLevel.Item1);
        Debug.Log("currentWorld="+worldAndStageLevel.Item2);
        Debug.Log("currentDiff="+datahandler.GetCharacterLevel());
        currentWorld = worldAndStageLevel.Item2;
        currentStage = worldAndStageLevel.Item1;
        currentDiff = datahandler.GetCharacterLevel();
        currentDiff++;
        Debug.Log("currentDiff="+currentDiff);
        Debug.Log("currentWorld="+currentWorld);
    }

    public async void GetQuestions()
    {
        string RootName = "StageNames/World"+currentWorld+"/Stage"+currentStage+"/Difficulty/"+currentDiff+"/Questions";
        
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        await FirebaseDatabase.DefaultInstance.GetReference(RootName).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                snapshot = null;
                Debug.Log("Error encountered, check input parameters");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {     // Do something with snapshot...
                this.snapshot = task.Result;
                Debug.Log("Finally fetched snapshot " + RootName);
            }
        });
        //GetSnapshotFromDatabase("StageNames/World0/Stage1/Difficulty/1/Questions/What Is Software Engineering/1");
        Debug.Log("3");
        //DataSnapshot rootSnapshot = GetSnapshot();
        
        string[] temp=new string[5];
        int temp1=0;
        if (childcount == 0) 
        {
            foreach (DataSnapshot a in snapshot.Children) 
            {
                childcount++;
            }

            Debug.Log("childcount if childcount = 0 = " + childcount);
        }

        //int temp2 = Random.Range(0,snapshot.Children.getChildrenCount());
        Debug.Log("childcount = "+childcount);
        
        int temp2=getRand(childcount);
        Debug.Log(temp2);
        int i = 0;
        foreach (DataSnapshot a in snapshot.Children)
        {
            i++;
            Debug.Log(a.Key.ToString());
            if (i == temp2)
            {
                QuestionField.text = a.Key.ToString();
                foreach (var s in a.Children)
                {
                    //IDictionary dictUsers = (IDictionary)s.Value;   
                    temp[temp1] = s.Value.ToString();
                    Debug.Log(temp[temp1]);
                    temp1++;
                }
                break;
            }
            
        }
        Option1.text=temp[0];
        Option2.text=temp[1];
        Option3.text=temp[2];
        Option4.text=temp[3];
        correctAns = int.Parse(temp[4]);

        //Debug.Log(snapshot.Value.ToString());
        //foreach(var s in rootSnapshot.Children){
        //IDictionary dictUsers = (IDictionary)s.Value;   
        //Debug.Log(s.Value.ToString());               
        //}   


    }

    public void checkAns(int ans)
    {
        if (ans == correctAns)
        {
            Debug.Log("Correct");
            score++;
        }
        else
        {
            Debug.Log("Wrong");
        }
        scoreCheck++;
        ScoreText.text="Score:"+score.ToString()+'/'+scoreCheck.ToString();
        if ((double) score / scoreCheck>((double)0.5))
        {
            PassFail.text = "Passed";
        }
        else
        {
            PassFail.text = "Failed";
        }
    }

    public async void updateProgress()
    {
        //string scoreText =score+'/'+scoreCheck;
        //ScoreText.text=score.ToString()+'/'+scoreCheck.ToString();
        //double total = (double)((double)score / scoreCheck);
        //userToken = "0nVjQmDgZtcSG0tvkLZ5saGeTd";
        Debug.Log("update called");
        userToken = "0nVjQmDgZtcSG0tvkLZ5saGeTdC2";
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        String RootName = "Progress/" + userToken +"/World"+currentWorld+"/Stage"+currentStage;
        await FirebaseDatabase.DefaultInstance.GetReference(RootName).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                snapshot = null;
                Debug.Log("Error encountered, check input parameters");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {     // Do something with snapshot...
                this.snapshot = task.Result;
                Debug.Log("Finally fetched snapshot " + RootName);
            }
        });
        
        currentProgress = Convert.ToInt32(snapshot.Value);
        Debug.Log(currentProgress);
        
        currentWorld = 1;
        
        double finishedStage = ((double) score / scoreCheck) * 100;
        if ((((double)score/scoreCheck) > (double)0.5) && (finishedStage > currentProgress))
        {
            //currentProgress = finishedStage;
            //update firebase progress/usertoken/world/stage/%value%
            
            
            Debug.Log(userToken);
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
            reference = FirebaseDatabase.DefaultInstance.RootReference;
            RootName = "Progress/" + userToken +"/World"+currentWorld+"/Stage"+currentStage;
            await FirebaseDatabase.DefaultInstance.GetReference(RootName).SetRawJsonValueAsync(finishedStage.ToString());
            
            
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
            reference = FirebaseDatabase.DefaultInstance.RootReference;
            RootName = "Leaderboard/" + userToken +"/Score";
            await FirebaseDatabase.DefaultInstance.GetReference(RootName).GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    snapshot = null;
                    Debug.Log("Error encountered, check input parameters");
                    // Handle the error...
                }
                else if (task.IsCompleted)
                {     // Do something with snapshot...
                    this.snapshot = task.Result;
                    Debug.Log("Finally fetched snapshot " + RootName);
                }
            });

            int leaderboard = Convert.ToInt32(snapshot.Value);
            
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
            reference = FirebaseDatabase.DefaultInstance.RootReference;
            RootName = "Leaderboard/" + userToken +"/Score";
            await FirebaseDatabase.DefaultInstance.GetReference(RootName).SetRawJsonValueAsync((leaderboard+finishedStage-currentProgress).ToString());
        }

        //score = scoreCheck = 0;
        resetQuestions();
    }

    private int getRand(int limit)
    {
        Debug.Log("limit = "+limit+" currentDiff = "+currentDiff);
        int i = 0,num = Random.Range(1, limit+1);
        Debug.Log(limit);
        Debug.Log(num+" rand");
        if (questions[currentDiff,limit-1] == 0)
        {
            while (questions[currentDiff,i] != 0)
            {
                if (questions[currentDiff,i] == num)
                {
                    return getRand(limit);
                }
                i++;
            }
        }
        else
        {
            resetQuestions();
            Debug.Log("reset");
        }
        questions[currentDiff,i] = num;
        return num;
    }

    private void resetQuestions()
    {
        int i = 0;
        while (questions[currentDiff,i] != 0)
        {
            questions[currentDiff,i] = 0;
            i++;
        }

        Debug.Log("reset called");
    }

    public void getHarderQuestion()
    {
        currentDiff++;
        childcount = 0;
        GetQuestions();
        childcount = 0;
        currentDiff--;
    }
}

