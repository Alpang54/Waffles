using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Facebook.Unity;
using UnityEngine.UI;

[Serializable]
public class FacebookScript
{
    private string accessToken;
    private string userID;
    public FacebookScript() {
      
    }



    public void FacebookLogin()
    {
        var permissions = new List<string>()
        {"public_profile","email","user_friends" };
        FB.LogInWithReadPermissions(permissions,AuthCallback);
    }

    public void FacebookLogout()
    {
        FB.LogOut();
    }

    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {

            // AccessToken class will have session details
            this.setAccessToken(Facebook.Unity.AccessToken.CurrentAccessToken);
            // Print current access token's User ID
            this.setUserID(Facebook.Unity.AccessToken.CurrentAccessToken.UserId);
            Debug.Log(this.accessToken);
            Debug.Log(Facebook.Unity.AccessToken.CurrentAccessToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in Facebook.Unity.AccessToken.CurrentAccessToken.Permissions)
            {
                Debug.Log(perm);
            }
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    public void setAccessToken(Facebook.Unity.AccessToken accessToken)
    {
        this.accessToken = ""+ accessToken;
    }
    public string getAccessToken() {
        return this.accessToken;
    }

    public void setUserID(string userID)
    {
        this.userID = userID;
    }
    public string getuserID()
    {
        return this.userID;
    }

}
