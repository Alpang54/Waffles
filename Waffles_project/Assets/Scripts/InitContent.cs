﻿using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitContent : MonoBehaviour
{
    public GameObject extraContent;
    public Text stageName;
    public Transform contentPanel;
    public int noOfCustom;
    ArrayList arrayStageName = new ArrayList();
    public bool done=false;

    public string strJson;
    // Start is called before the first frame update

    private void Awake()
    {
        
    }

    void Start()
    {

        StartCoroutine(ReadDB());
        //Invoke("loadDB", 1);
        

        


    }
    IEnumerator ReadDB()
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
                    noOfCustom = (int)snapshot.ChildrenCount;

                    foreach (var stages in snapshot.Children)
                    {
                        Debug.LogFormat("Key={0}", stages.Key); //Node Custom Stage Name
                        arrayStageName.Add(stages.Key.ToString());
                        
                        foreach (var questionNumber in stages.Children)
                        {
                            Debug.LogFormat("Key={0}", questionNumber.Children);//Node at Question Number
                            string noOfQuestionNumberInStage = questionNumber.ChildrenCount.ToString();
                            foreach(var indiQuestion in questionNumber.Children) //Node at indiQuestion
                            {
                                foreach(var value in indiQuestion.Children) //values in indiQuestion
                                {
                                    string test = value.Value.ToString();
                                }
                            }
                        }
                    }
                    done = true;
                    //strJson =snapshot.GetRawJsonValue();
                 
                }
            });

        yield return new WaitUntil(() => done == true); 
           
        
       
        if(done==true) //reading db should be done by now
        {
            loadDB();
            //yield return new WaitForSeconds(1f);
        }
        
       
    }
    void loadDB()
    {
        for (int i = 0; i < noOfCustom; i++)
        {
             stageName.text = arrayStageName[i].ToString();
            GameObject go = Instantiate(extraContent) as GameObject;
            //go.GetComponent<>
            go.SetActive(true);
            go.transform.SetParent(contentPanel);



        }
    }
   
}
