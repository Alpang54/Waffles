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
    GameObject popUp;
    private void Awake()
    {
        
    }

    void Start()
    {

        dataHandler = GameObject.Find("DataManager").GetComponent<DataHandler>();

        if (currentScene == "Lobby")
            StartCoroutine(ReadDBLobby());
        else
        {

            StartCoroutine(ReadDBEdit());
            //Invoke("loadDB", 1);
           
        }



    }
    IEnumerator ReadDBLobby()
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
    void loadDBLobby()
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
        loading.SetActive(false);

        foreach (Transform child in contentPanel)
        {

            if ((child.gameObject.name.ToString().ToLower().Contains(ifs.text.ToString().ToLower())))
            {
                child.gameObject.SetActive(true);
                count++;
            }
            else
                child.gameObject.SetActive(false);

        }
        if (count <= 0)
        {
            loading.SetActive(true);
            loading.GetComponent<Text>().text = "No matching stage found";
        }
    }
    public void GetGameObjectName(GameObject textUpdate)
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
        Debug.Log(currentSelected.transform.parent.name);
        popUp.SetActive(true);
        StartCoroutine(FetchPastProgress(currentSelected.transform.parent.name,textUpdate));

    }
    public static DateTime? ConvertUnixTimeStamp(string unixTimeStamp)
    {
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        epoch = epoch.AddMilliseconds(Convert.ToDouble(unixTimeStamp));// your case results to 4/5/2013 8:48:34 AM
        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");
        DateTime dt = System.TimeZoneInfo.ConvertTimeFromUtc(epoch, tzi);
        return dt;
    }
    IEnumerator FetchPastProgress(string stageName,GameObject update)
    {
        done = false;
        loading.SetActive(true);
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        string data = "";
        update.GetComponent<Text>().text = "";
        FirebaseDatabase.DefaultInstance.GetReference("Data/Custom/" + stageName + "/" + dataHandler.GetFirebaseUserId()).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                done = true;
                int countDB = 0;
                int noOfCustom;
                DataSnapshot snapshot = task.Result;
                foreach (var stats in snapshot.Children)
                {
                    if (stats.Key == "attemptedTimestamp")
                        data += "Timestamp attempted: " + ConvertUnixTimeStamp(stats.Value.ToString()) + "\n";
                    if (stats.Key == "qnsCount")
                        data += "Number of attempted qns: " + stats.Value.ToString() + "\n";
                    if (stats.Key == "noRight")
                        data += "Number of correct qns: " + stats.Value.ToString() + "\n";
                    if (stats.Key == "noWrong")
                        data += "Number of wrong qns: " + stats.Value.ToString() + "\n";
                    if (stats.Key == "timeTakenPer")
                        data += "Average time taken for each qns: " + stats.Value.ToString() + "\n";
                    Debug.Log(stats.Value.ToString());

                }
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

    IEnumerator ReadDBEdit()
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
    void loadDBEdit()
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
