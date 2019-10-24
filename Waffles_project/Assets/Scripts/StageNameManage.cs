using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageNameManage : MonoBehaviour
{
    public Button clickedDelete;
    public Button clickedEdit;
    public GameObject prefabRef;
    public GameObject popUpDelete;
    public Transform contentPanel2;
    public Text stageName;
    private int noOfCustom=0;
    public int delete = 0;
    static public string customName;
    private DataHandler dataHandler;
    public ArrayList arrayStageName = new ArrayList();
    public ArrayList pushKey = new ArrayList();
    public bool done = false;
    public GameObject popUpComplete;
    public static string stgName;
    

    public  void onDeleteClick()
    {

        
        dataHandler = GameObject.Find("DataManager").GetComponent<DataHandler>();
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
       
            // dataHandler = GameObject.Find("DataManager").GetComponent<DataHandler>();



            reference.Child("CustomStage").Child(stgName).RemoveValueAsync();
            reference.Child("UserCustom").Child(dataHandler.GetFirebaseUserId()).Child(stgName).RemoveValueAsync();
            reference.Child("Data").Child("Custom").Child(stgName).RemoveValueAsync();


            Destroy(prefabRef);
           popUpComplete.SetActive(true);
        

       


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
   
    public void DeleteCheck()
    {

        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
        Debug.Log(currentSelected.transform.parent.name);
        stgName = currentSelected.transform.parent.name;
        popUpDelete.SetActive(true);
    }

}