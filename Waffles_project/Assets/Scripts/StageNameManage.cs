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
    private ArrayList arrayStageName = new ArrayList();
    private ArrayList pushKey = new ArrayList();
    public bool done = false;

   

    public void onDeleteClick()
    {
        string test = stageName.text.ToString();
        dataHandler = GameObject.Find("DataManager").GetComponent<DataHandler>();

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("CustomStage").Child(stageName.text.ToString()).RemoveValueAsync();



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

    
}