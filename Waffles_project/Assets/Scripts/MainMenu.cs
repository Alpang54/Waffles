using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{


    [SerializeField]
    GameObject loginPopUp;

    private DataHandler datahandler;
    // Start is called before the first frame update
    void Start()
    {
        datahandler = GameObject.Find("DataManager").GetComponent<DataHandler>();

    }

    
    public void CheckLogin(string nextScene)
    {
        if(datahandler.GetIsLoggedIn())
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            loginPopUp.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
