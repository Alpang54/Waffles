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

public class Login : MonoBehaviour
{
    private static readonly string databaseURL = "https://cz3003-waffles.firebaseio.com/";
    public InputField emailAddress;
    public InputField password;
    private static readonly HttpClient client = new HttpClient();
    


    void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                {
                    FB.ActivateApp();
                }
                else
                {
                    Debug.Log("Did not initialize");
                }
            },
            isGameShown =>
            {
                if (!isGameShown)
                {
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1;
                }
            }

            );


        }
        else
        {
            FB.ActivateApp();
        }
    }
    public void OnSubmit()
    {
        print("start");
        print(emailAddress.text);
        print(password.text);
        print("working");
        SignIn(emailAddress.text, password.text);

    }

    private void SignIn(string email, string password)
    {
        FirebaseScript fireScript = new FirebaseScript();
        FacebookScript fbScript = new FacebookScript();
        fbScript.FacebookLogin();
        string FBAccesstoken =fbScript.getAccessToken();
        //since unity does not support full testing with facebook
        fireScript.FireBaseLogin("EAAKnDH9pBCIBAEdDHjfwJAgkjNaxCrGum8dEZBWhZCiWWp7i37W3t" +
            "dD2hkZBxf1VXlUZBr4aYCZB4INqmcnxt2kj9PmuKjdsd72606jJPpa9DrRLDXqHniz7xkENXsMOS6mAzeoaEFcHYXjAmmT7UANOLHoCVU5WSus8y7zEZBzKiiJ55TQFgE");

    }


    async static void GetFromDatabase()

    {
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage response = await client.GetAsync("https://cz3003-waffles.firebaseio.com/.json"))
            {
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
            using (HttpResponseMessage response = await client.PutAsync("https://cz3003-waffles.firebaseio.com/.json", stringContent))
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
}