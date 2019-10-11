using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitContent : MonoBehaviour
{
    public GameObject extraContent;
    public Text stageName;
    public Transform contentPanel;
    // Start is called before the first frame update
    void Start()
    {
        DatabaseReference databaseRef;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance.GetReference("CustomStage").GetValueAsync().ContinueWith(task => {
          if (task.IsFaulted)
          {
              // Handle the error...
          }
          else if (task.IsCompleted)
          {
              DataSnapshot snapshot = task.Result;
                
              // Do something with snapshot...
          }
      });
        for (int i=0;i<12;i++)
        {
            stageName.text = "Test" + i.ToString();
            GameObject go = Instantiate(extraContent) as GameObject;
            go.SetActive(true);
            go.transform.SetParent(contentPanel);
            
            
      
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
