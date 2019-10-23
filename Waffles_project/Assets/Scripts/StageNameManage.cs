using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageNameManage : MonoBehaviour
{
    public Button clickedDelete;
    public Button clickedEdit;
    public GameObject prefabRef;
    public Text stageName;
    private int noOfCustom=0;
    static public string customName;
    private DataHandler dataHandler;
    public ArrayList arrayStageName = new ArrayList();
    public ArrayList pushKey = new ArrayList();
    public bool done = false;
  


    public void onDeleteClick()
    {
        dataHandler = GameObject.Find("DataManager").GetComponent<DataHandler>();
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        string test = stageName.text.ToString();
        // dataHandler = GameObject.Find("DataManager").GetComponent<DataHandler>();
       


        reference.Child("CustomStage").Child(stageName.text.ToString()).RemoveValueAsync();
        reference.Child("UserCustom").Child(dataHandler.GetFirebaseUserId()).Child(stageName.text.ToString()).RemoveValueAsync();
        reference.Child("Data").Child("Custom").Child(stageName.text.ToString()).RemoveValueAsync();


        Destroy(prefabRef);
        
       


    }



    public void onEditClick()
    {
        customName = stageName.text.ToString();
        SceneManager.LoadScene("Edit_Custom");
    }
    public void onPlayClick()
    {
        customName = stageName.text.ToString();
        SceneManager.LoadScene("Custom Stage");
    }
    void loadDB()
    {
        string test;
    }

    
}