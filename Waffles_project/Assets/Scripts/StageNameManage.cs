using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageNameManage : MonoBehaviour
{
    public Button clickedDelete;
    public Button clickedEdit;
    public GameObject prefabRef;
    public Text stageName;
    static public string customName;

    public void onDeleteClick()
    {
        string test = stageName.text.ToString();


        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("CustomStage").Child(test).RemoveValueAsync();
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