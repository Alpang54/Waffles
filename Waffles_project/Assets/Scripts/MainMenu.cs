using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using UnityEngine.SceneManagement;
/**
*Menu Manager to show pop up if user is not logged in, since the user must be logged in for the firebase to get correct token
* @author Mok Wei Min
**/
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

    /**
    *Operates from button press, it loads the next scene if the user is logged in, if not, prompt pop up to login
    * @param nextScene scene name that button press is linked to
    **/
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
    /**
    *Operates from button press, it activates the gameobject if the user is logged in, if not, prompt pop up to login, Not being used at the moment
    * @param toActivate gameobject to activate if user is logged in
    **/
    public void CheckLogin(GameObject toActivate)
    {
        if (datahandler.GetIsLoggedIn())
        {
            toActivate.SetActive(true);
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
