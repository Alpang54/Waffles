using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Proyecto26;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUser : MonoBehaviour
{
    private static readonly string databaseURL = "https://cz3003-waffles.firebaseio.com/";
    public InputField emailAddress;
    public InputField password;
    public InputField reEnterPassword;
    public Text status;
    public bool passwordMatch;
    private static readonly HttpClient client = new HttpClient();

    public void OnSubmit()
    {
        status.text = "Authenticating User";
        print("start");
        print(emailAddress.text);
        print(password.text);
        print(reEnterPassword.text);
        if (password.text == reEnterPassword.text && IsValidEmail(emailAddress.text))
        {
            print("working");
            PostToDatabase();
            status.text = "Authenticated User";
        }

        else
        {
            status.text = "Invalid email or password";
        }

    }

    public void PostToDatabase() {

        print("sent");

        User user = new User(emailAddress.text,password.text);

        RestClient.Put(databaseURL +".json", user);





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
