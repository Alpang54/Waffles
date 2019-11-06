using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;



/** DBHandler manages the Stage map on the application side regarding GameObjects
* @author Ang Jie Kai Alvis
**/
public class DBHandler : MonoBehaviour
{
    private DataSnapshot snapshot;


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    public DataSnapshot GetSnapshot()
    {
        return this.snapshot;
    }


    /** Reads a jsonParse Tree from the database
    * @params RootName is the root name of the json parse tree
    **/
    public async Task ReadfromFirebase(string RootName)
    {
        await GetSnapshotFromDatabase(RootName);
        Debug.Log("Snapshot should have loaded");

    }


    /** Error checking to determine if there is such a parse tree specified
    * @params RootName is the root name of the json parse tree
    **/
    private async Task GetSnapshotFromDatabase(string RootName)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        await FirebaseDatabase.DefaultInstance.GetReference(RootName).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                snapshot = null;
                Debug.Log("Error encountered, check input parameters");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {     // Do something with snapshot...
                this.snapshot = task.Result;
                Debug.Log("Finally fetched snapshot" + RootName);

            }
        });
    }
}


