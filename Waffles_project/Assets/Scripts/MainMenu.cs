using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Button loginOutbtn;
    // Start is called before the first frame update
    void Start()
    {
        if(FB.IsLoggedIn)
        {
            loginOutbtn.GetComponentInChildren<Text>().text = "Logout";
        }
        else
        {
            loginOutbtn.GetComponentInChildren<Text>().text = "Login";
        }

    }

    public void MenuLoginLogout()
    {
        if (this.gameObject.GetComponent<Login>() != null)
        {

            //Changes text of login/logout button (Shows login even on user cancel)
            if (!FB.IsLoggedIn)
            {
                this.gameObject.GetComponent<Login>().FacebookLogin();
                //Without this it will just say logout even if user cancels the login
                if (this.gameObject.GetComponent<Login>().loginProgress == 1)
                {
                    loginOutbtn.GetComponentInChildren<Text>().text = "Logout";
                    Debug.Log("Re logged in");
                }

            }
            else
            {
                this.gameObject.GetComponent<Login>().FacebookLogout();
                loginOutbtn.GetComponentInChildren<Text>().text = "Logoin";
                Debug.Log("Logging out");

            }

        }
       
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
