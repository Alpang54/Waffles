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

    private Login LoginManagerScript;


    void Awake()
    {
        LoginManagerScript = GameObject.Find("LoginManager").GetComponent<Login>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }


    public void MenuLoginLogout()
    {
        LoginManagerScript = GameObject.Find("LoginManager").GetComponent<Login>();
        bool isLoggedIn = LoginManagerScript.GetLoggedIn();
        if (isLoggedIn)
        {
            UpdateLoginOutText(isLoggedIn);
            LoginManagerScript.FacebookLogout();
        }
        else
        {
            UpdateLoginOutText(isLoggedIn);
            LoginManagerScript.FacebookLogin();

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
