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
    public Text stageName;
    public Transform contentPanel;
    public int noOfCustom;
    ArrayList arrayStageName = new ArrayList();

    public string strJson;
    // Start is called before the first frame update

    private void Awake()
    {
        
    }

    void Start()
    {

        readDB();
        Invoke("loadDB", 2);


        


    }
    void readDB()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;







        FirebaseDatabase.DefaultInstance.GetReference("CustomStage").GetValueAsync().ContinueWith(task => {
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
                    Debug.LogFormat("Key={0}", stages.Key);
                    arrayStageName.Add(stages.Key.ToString());
                    foreach (var value in stages.Children)
                    {
                        Debug.LogFormat("Key={0}", value.Key);
                        string test = value.Value.ToString();
                    }
                }

                //strJson =snapshot.GetRawJsonValue();

            }
        });

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
    // Update is called once per frame
    void Update()
    {
        
    }
}
