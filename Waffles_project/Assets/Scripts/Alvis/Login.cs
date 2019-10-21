using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Proyecto26;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Networking;
using Facebook.Unity;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    private Facebook.Unity.AccessToken accessToken;
    [SerializeField] private string userID;
    private string accessTokenForFirebase;
    [SerializeField] private static Credential credentials;


    [SerializeField]
    Button loginOutbtn;

    private bool loggedIn = false;
    private DataHandler datahandler;


// Start of Default Code
    void Awake()
    { 
        //Only init if this page is login
        if (!FB.IsInitialized)
            FB.Init(SetInit, OnHideUnity);
        datahandler = GameObject.Find("DataManager").GetComponent<DataHandler>();
        this.loggedIn = datahandler.GetIsLoggedIn();
        if (this.loggedIn)
        {
            loginOutbtn.GetComponentInChildren<Text>().text = "Logout";
        }
    }
 
 
    private void SetInit()
    {
        if (FB.IsInitialized)
        {
            Debug.Log("Fb init is done");
            // Signal an app activation App Event
            FB.ActivateApp();
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }


    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    //End Of Default Code

    public void OnLoginLogoutButtonClick()
    {
        if (!loggedIn)
        {
            FacebookLogin();
        }
        else
        {
            FacebookLogout();
        }
    }

    public void FacebookLogin()
    {
        if (!FB.IsLoggedIn)
        {
            var perms = new List<string>() { "public_profile", "email" };
            FB.LogInWithReadPermissions(perms, FBAuthCallback);
        }
        else
        {
            this.loggedIn = true;
            loginOutbtn.GetComponentInChildren<Text>().text = "Logout";
            Debug.Log("Fb is logged in already");
        }
    }
  

    private void FBAuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            this.loggedIn = true;
            this.accessToken = AccessToken.CurrentAccessToken;
            credentials = FacebookAuthProvider.GetCredential(this.accessToken.TokenString);
            datahandler.SetIsLoggedIn(true);
            loginOutbtn.GetComponentInChildren<Text>().text = "Logout";
            FirebaseLogin();
            Debug.Log("In FBAUTHCALLBACK");
            Debug.Log("this.accesstoken.userid is:" + this.accessToken.UserId);

        }
        else
        {
            this.loggedIn = false;
            loginOutbtn.GetComponentInChildren<Text>().text = "Login";
            Debug.Log("User cancelled login");
            datahandler.SetIsLoggedIn(false);
        }
    }

    public void FacebookLogout()
    {
         FB.LogOut();
         this.loggedIn = false;
        loginOutbtn.GetComponentInChildren<Text>().text = "Login";
        datahandler.SetIsLoggedIn(false);
    }



    private void FirebaseLogin()
    {
           Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
           auth.SignInWithCredentialAsync(credentials).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }
           
            Firebase.Auth.FirebaseUser newUser = task.Result;
               userID = newUser.UserId;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
            newUser.DisplayName, newUser.UserId);
               datahandler.SetFireBaseUserId(newUser.UserId);
        });
        Debug.Log("Login done");
      
    }



    public bool GetLoggedIn()
    {
        return this.loggedIn;
    }

    public string GetUserID()
    {
        return this.userID;

    }


}





    /*
    async static void GetFromDatabase()

    {
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage response = await client.GetAsync("https://cz3003-waffles.firebaseio.com/.json"))
            
                using (HttpContent content = response.Content)
                {
                    string mycontent = await content.ReadAsStringAsync();
                    print(mycontent);
                }
            }
        }

    }




    async static void PostToDatabase(User user)
    {
        string json = JsonUtility.ToJson(user);

        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage response = await client.PutAsync("https://cz3003-waffles.firebaseio.com/users.json", stringContent))
            {
                using (HttpContent content = response.Content)
                {
                    string mycontent = await content.ReadAsStringAsync();
                }
            }
        }

    }


    bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}*/