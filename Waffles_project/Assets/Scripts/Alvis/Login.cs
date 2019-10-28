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


    private Firebase.Auth.FirebaseAuth auth;

    [SerializeField]
    Button loginOutbtn;

    private bool loggedIn = false;
    private DataHandler datahandler;


    // Start of Default Code
    void Awake()
    {
        datahandler = GameObject.Find("DataManager").GetComponent<DataHandler>();
        //Only init if this page is login
        if (!FB.IsInitialized)
            FB.Init(SetInit, OnHideUnity);
        Debug.Log("Awake");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

    }

    private void Start()
    {
        //Update the text whenever reload scene (Ex. switching scenes since init wont be called multiple times)
        if (FB.IsLoggedIn)
        {
            loginOutbtn.GetComponentInChildren<Text>().text = "Logout";
            SetInit();
        }
        else
            loginOutbtn.GetComponentInChildren<Text>().text = "Login";

    }

    private void SetInit()
    {
        if (FB.IsInitialized)
        {
            Debug.Log("Fb init is done");
            // Signal an app activation App Event
            FB.ActivateApp();
            //Check if they already logged in or not before, to update button text
            if (FB.IsLoggedIn)
            {
                this.loggedIn = true;
                loginOutbtn.GetComponentInChildren<Text>().text = "Logout";
                datahandler.SetIsLoggedIn(true);
                FB.API("me?fields=name", Facebook.Unity.HttpMethod.GET, GetFacebookData);
                this.accessToken = AccessToken.CurrentAccessToken;
                credentials = FacebookAuthProvider.GetCredential(this.accessToken.TokenString);
                FirebaseLogin();

            }
            else
            {
                this.loggedIn = false;
                loginOutbtn.GetComponentInChildren<Text>().text = "Login";
                datahandler.SetIsLoggedIn(false);
            }
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }
    void GetFacebookData(Facebook.Unity.IGraphResult result)
    {
        string fbName = result.ResultDictionary["name"].ToString();

        datahandler.SetFBUserName(fbName);
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
        if (!FB.IsLoggedIn)
        {
            Debug.Log("FacebookLogin");
            FacebookLogin();
        }
        else
        {
            Debug.Log("FacebookLogout");
            FacebookLogout();
        }
    }

    public void FacebookLogin()
    {
        Debug.Log("FacebookLogin");
        if (!FB.IsLoggedIn)
        {
            var perms = new List<string>() { "public_profile", "email" };
            FB.LogInWithReadPermissions(perms, FBAuthCallback);
        }
        else
        {
            this.loggedIn = true;
            datahandler.SetIsLoggedIn(true);
            loginOutbtn.GetComponentInChildren<Text>().text = "Logout";
            Debug.Log("Fb is logged in already");
        }
    }


    private void FBAuthCallback(ILoginResult result)
    {
        Debug.Log("FBAuthCallback");
        if (FB.IsLoggedIn)
        {
            this.loggedIn = true;
            this.accessToken = AccessToken.CurrentAccessToken;
            credentials = FacebookAuthProvider.GetCredential(this.accessToken.TokenString);
            print(credentials);
            datahandler.SetIsLoggedIn(true);
            Debug.Log("In FBAUTHCALLBACK"); Debug.Log("In FBAUTHCALLBACK");
            Debug.Log("In FBAUTHCALLBACK");
            loginOutbtn.GetComponentInChildren<Text>().text = "Logout";
            FirebaseLogin();
            Debug.Log("In FBAUTHCALLBACK");
            Debug.Log("this.accesstoken.userid is:" + this.accessToken.UserId);
            FB.API("me?fields=name", Facebook.Unity.HttpMethod.GET, GetFacebookData);
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
        this.loggedIn = false;
        loginOutbtn.GetComponentInChildren<Text>().text = "Login";
        datahandler.SetIsLoggedIn(false);
        FB.LogOut();

    }



    private void FirebaseLogin()
    {
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
        Debug.Log(datahandler.GetFirebaseUserId());
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
