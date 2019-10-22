using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DBHandler : MonoBehaviour
{
    private DataSnapshot snapshot;
	public Text QuestionField, Option1,Option2,Option3,Option4,ScoreText;
    public int currentDiff = 1;
    public int currentWorld = 0;
    public int currentStage = 1;
    public int correctAns = 0;
    public string userToken = "0nVjQmDgZtcSG0tvkLZ5saGeTd";
    public int currentProgress=0;
    public int score = 0, scoreCheck=0;
    public int[,] questions = new int[20,20];
    private int childcount=0;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        //QuestionField.text = " ";
        //SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        //load userToken
    }

    public DataSnapshot GetSnapshot()
    {
        return this.snapshot;
    }

    public async Task ReadfromFirebase(string RootName)
    {
        await GetSnapshotFromDatabase(RootName);
        Debug.Log("Snapshot should have loaded");
       
    }

    private async Task GetSnapshotFromDatabase(string RootName)
    {
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
                Debug.Log("Finally fetched snapshot" + RootName);
                Debug.Log(task.Result); 
                Debug.Log("1");
                //this.snapshot = snapshot.Children;
                //Debug.Log(snapshot.Key);
                //Debug.Log(snapshot.Children);
                //var values = snapshot.Children as IDictionary<string, object>;
                //Debug.Log(values);
                //string ss = snapshot.Value.ToString();
                Debug.Log( snapshot.Value.ToString());
                /*foreach ( DataSnapshot user in snapshot.Children){
                    IDictionary dictUser = (IDictionary)user.Value;
                    Debug.Log ("" + dictUser["1"] + " - " + dictUser["2"]);
                }
                /*foreach (var v in values)
                {
                    Debug.Log(v.Key + ":" + v.Value); // category:livingroom, code:126 ...
                }*/
                Debug.Log("2"); 
                 

            }
        });
    }
	
	public async void GetQuestions()
    {
        //string RootName = "StageNames/World0/Stage1/Difficulty/1/Questions/What Is Software Engineering";
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
                Debug.Log("Finally fetched snapshot" + RootName);
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

            Debug.Log("childcount = " + childcount);
        }

        //int temp2 = Random.Range(0,snapshot.Children.getChildrenCount());
        Debug.Log(childcount);
        
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
        ScoreText.text=score.ToString()+'/'+scoreCheck.ToString();
    }

    public void updateProgress(int finishedStage=101)
    {
        //string scoreText =score+'/'+scoreCheck;
        //ScoreText.text=score.ToString()+'/'+scoreCheck.ToString();
        //double total = (double)((double)score / scoreCheck);
        userToken = "0nVjQmDgZtcSG0tvkLZ5saGeTd";
        if ((((double)score/scoreCheck) > (double)0.5) && (finishedStage > currentProgress))
        {
            currentProgress = finishedStage;
            //update firebase progress/usertoken/world/stage/%value%
            
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            Debug.Log(userToken);
            string RootName = "StageProgress/" + userToken +"/World"+currentWorld+"/Stage"+currentStage;
            //string json = JsonUtility.ToJson(finishedStage);
            Debug.Log(RootName);
            FirebaseDatabase.DefaultInstance.GetReference(RootName).SetRawJsonValueAsync(finishedStage.ToString());
            //Debug.Log(json);
            /*var database = firebase.database();
            firebase.database().ref('StageProgress/' + userToken+"/World"+currentWorld+"/Stage"+currentStage).set({
                finishedStage
            });*/

        }

        //score = scoreCheck = 0;
        resetQuestions();
    }

    private int getRand(int limit)
    {
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
    
    public void setUserToken(string token)
    {
        userToken = token;
    }

    public void setCurrentDiff(int diff)
    {
        currentDiff = diff;
    }
    
    public void setCurrentWorld(int world)
    {
        currentWorld = world;
    }
    
    public void setCurrentStage(int stage)
    {
        currentStage = stage;
    }

    public void setCurrentProgress(int prog)
    {
        currentProgress = prog;
    }
}


