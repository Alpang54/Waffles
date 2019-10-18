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
    [SerializeField] public string userID;
    private string accessTokenForFirebase;
    [SerializeField] public static Credential credentials;


    public bool loggedIn = false;

    void Awake()
    {
        //Only init if this page is login
        if (!FB.IsInitialized)
            FB.Init(SetInit, OnHideUnity);
        DontDestroyOnLoad(this.gameObject);
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


    public bool GetLoggedIn()
    {
        return this.loggedIn;
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



    public string GetUserID()
    {
        return this.userID;

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
            loggedIn = true;
        }
    }
  

    private void FBAuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            this.accessToken = AccessToken.CurrentAccessToken;
            credentials = FacebookAuthProvider.GetCredential(this.accessToken.TokenString);
            loggedIn = true;
            FirebaseLogin();
            Debug.Log("In FBAUTHCALLBACK");
            Debug.Log("userid is:"+this.accessToken.UserId);


        }
        else
        {
            loggedIn = false;
            Debug.Log("User cancelled login");
        }
        //Only in main menu this will work
        if(this.gameObject.GetComponent<MainMenu>()!=null)
        {
            //Reflect correct text on button to show login/logout
            this.gameObject.GetComponent<MainMenu>().UpdateLoginOutText(loggedIn);
        }
    }
    public void FacebookLogout()
    {
        if(FB.IsLoggedIn)
        {
            FB.LogOut();
            loggedIn = false;
        }
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
        });
        Debug.Log("Login done");
      
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