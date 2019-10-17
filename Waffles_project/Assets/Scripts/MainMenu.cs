using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Button loginOutbtn;
    [SerializeField]
    GameObject loginPopUp;
    void Awake()
    {
       
    }
    // Start is called before the first frame update
    void Start()
    {

        if (!FB.IsLoggedIn)
        {
            this.gameObject.GetComponent<Login>().FacebookLogin();
        }
    }


    public void MenuLoginLogout()
    {
        if (this.gameObject.GetComponent<Login>() != null)
        {
            if (!FB.IsLoggedIn)
            {
                this.gameObject.GetComponent<Login>().FacebookLogin();
            }
            else
            {
                this.gameObject.GetComponent<Login>().FacebookLogout();
                UpdateLoginOutText(false);
                Debug.Log("Logged out");

            }

        }
       
    }
    public void UpdateLoginOutText(bool loggedIn)
    {
        if (loggedIn)
        {
            loginOutbtn.GetComponentInChildren<Text>().text = "Logout";
            Debug.Log("Re logged in");
        }
        else
        {
            loginOutbtn.GetComponentInChildren<Text>().text = "Login";

        }
    }
    
    public void CheckLogin(string nextScene)
    {
        if(FB.IsLoggedIn)
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            if (loginPopUp != null)
                loginPopUp.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
