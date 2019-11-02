using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * This class will instiantiate and load the number of custom stage and display to user depending on the db values
 * @author Ng Kai Qian
 */

public class InitContent : MonoBehaviour
{
    public GameObject extraContent;
    public GameObject addButton;
    public Text stageName;
    public Transform contentPanel;
    public int noOfCustom;
    
    ArrayList arrayStageName = new ArrayList();
    public bool done=false;
    
    public string strJson;
    // Start is called before the first frame update
    private DataHandler dataHandler;

    [SerializeField]
    string currentScene;
    [SerializeField]
    GameObject loading;
    [SerializeField]
    GameObject text;
    private void Awake()
    {
        
    }

    void Start()
    {

        dataHandler = GameObject.Find("DataManager").GetComponent<DataHandler>();

        if (currentScene == "Lobby")
            StartCoroutine(ReadDBLobby()); //start coroutine to read DB for Custom Lobby Scene
        else
        {

            StartCoroutine(ReadDBEdit()); //Start coroutine to read DB for Manage Custom Scene
            //Invoke("loadDB", 1);
           
        }



    }
    IEnumerator ReadDBLobby() //Read DB for Custom Lobby
    {
        done = false;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        loading.SetActive(true);


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


                }
                done = true;
                //strJson =snapshot.GetRawJsonValue();

            }
        });

        yield return new WaitUntil(() => done == true);



        if (done == true) //reading db should be done by now
        {
            loadDBLobby();
            loading.SetActive(false);
            //yield return new WaitForSeconds(1f);
        }


    }
    void loadDBLobby()  //Instantiate number of custom stage based on DB and load the stage name
    {
        for (int i = 0; i < noOfCustom; i++)
        {
            stageName.text = arrayStageName[i].ToString();
            GameObject go = Instantiate(extraContent) as GameObject;
            //go.GetComponent<>
            go.SetActive(true);
            go.name = arrayStageName[i].ToString();
            go.transform.SetParent(contentPanel);
            go.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        // addButton.SetActive(true);
    }
    public void OnSearch(InputField ifs)
    {
        int count = 0;
        text.SetActive(false);

        foreach (Transform child in contentPanel)
        {

            if (Search(child.gameObject.name, ifs.text.ToString()))
            {
                child.gameObject.SetActive(true);
                count++;
            }
            else
                child.gameObject.SetActive(false);

        }
        if (count <= 0)
        {
            //loading.SetActive(true);
            text.SetActive(true);
            text.GetComponent<Text>().text = "No matching stage found";
        }
    }
    public bool Search(string a, string b)
    {
        if (a.ToLower().Contains(b.ToLower()))
            return true;
        else
            return false;
    }
    public void GetGameObjectName(GameObject textUpdate)
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
        Debug.Log(currentSelected.transform.parent.name);
        StartCoroutine(FetchPastProgress(currentSelected.transform.parent.name,textUpdate));

    }
    public static DateTime? ConvertUnixTimeStamp(long unixTimeStamp)
    {
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        epoch = epoch.AddMilliseconds(unixTimeStamp);// your case results to 4/5/2013 8:48:34 AM
        TimeZoneInfo tzi = TimeZoneInfo.Local;
        
        DateTime dt = System.TimeZoneInfo.ConvertTimeBySystemTimeZoneId(epoch, tzi.Id);
        return dt;
    }
    IEnumerator FetchPastProgress(string stageName,GameObject update)
    {
        done = false;
        loading.SetActive(true);
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        string data = "";
        Debug.Log(dataHandler.GetFirebaseUserId());
        update.GetComponent<Text>().text = "";
        FirebaseDatabase.DefaultInstance.GetReference("Data/Custom/" + stageName + "/" + dataHandler.GetFirebaseUserId()).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                int countDB = 0;
                int noOfCustom;
                DataSnapshot snapshot = task.Result;
                Debug.LogFormat("Count={0}", snapshot.ChildrenCount); //Node at question number

                foreach (var stats in snapshot.Children)
                {
                    Debug.LogFormat("Value={0}", stats.Value.ToString()); //values inside question 1,2

                    if (countDB==0)
                    {
                        long milliseconds;
                        long.TryParse(stats.Value.ToString(), out milliseconds);
                        Debug.Log(ConvertUnixTimeStamp(milliseconds));
                        data += "Timestamp attempted: " + ConvertUnixTimeStamp(milliseconds) + "\n";
                    }
                       
                    if (countDB ==4)
                        data += "Number of attempted qns: " + stats.Value.ToString() + "\n";
                    if (countDB ==2)
                        data += "Number of correct qns: " + stats.Value.ToString() + "\n";
                    if (countDB ==3)
                        data += "Number of wrong qns: " + stats.Value.ToString() + "\n";
                    if (countDB ==7)
                        data += "Average time taken for each qns: " + stats.Value.ToString() + "\n";
                    countDB++;
                }
                done = true;

            }
        });
        yield return new WaitUntil(() => done == true);

        if(done)
        {
            loading.SetActive(false);
            if (data != "")
                update.GetComponent<Text>().text = data;
            else
                update.GetComponent<Text>().text = "No previous progress found!";

        }
    }

    IEnumerator ReadDBEdit() //Read DB for manage custom based on user id
    {
        done = false;
        loading.SetActive(true);

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

            FirebaseDatabase.DefaultInstance.GetReference("UserCustom/"+dataHandler.GetFirebaseUserId()).GetValueAsync().ContinueWith(task =>
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
                        arrayStageName.Add(stages.Value.ToString());
                        
                        
                    }
                    done = true;
                    //strJson =snapshot.GetRawJsonValue();
                 
                }
            });

        yield return new WaitUntil(() => done == true); 
           
        
       
        if(done==true) //reading db should be done by now
        {
            loadDBEdit();
            loading.SetActive(false);

            //yield return new WaitForSeconds(1f);
        }


    }
    void loadDBEdit() //Instantiate number of custom stage based on DB and load the stage name
    {
        for (int i = 0; i < noOfCustom; i++)
        {
             stageName.text = arrayStageName[i].ToString();
            GameObject go = Instantiate(extraContent) as GameObject;
            go.name=(arrayStageName[i].ToString());
            //go.GetComponent<>
            go.transform.SetParent(contentPanel);
            go.gameObject.transform.localScale = new Vector3(1, 1, 1);
            go.SetActive(true);

           
            

        }
        addButton.SetActive(true);
    }

    

}
