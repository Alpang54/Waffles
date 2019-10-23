using Firebase;
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
    private void Awake()
    {
        
    }

    void Start()
    {
        

        if (currentScene == "Lobby")
            StartCoroutine(ReadDBLobby());
        else
        {
            dataHandler = GameObject.Find("DataManager").GetComponent<DataHandler>();

            StartCoroutine(ReadDBEdit());
            //Invoke("loadDB", 1);
            string test = dataHandler.GetFirebaseUserId();
            Debug.Log(test);
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
            //go.GetComponent<>
            go.SetActive(true);
            go.transform.SetParent(contentPanel);
            go.gameObject.transform.localScale = new Vector3(1, 1, 1);



        }
        addButton.SetActive(true);
    }
   
}
