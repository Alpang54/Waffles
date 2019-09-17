using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseScript
{
    private Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    private string credentials;

    public void FireBaseLogin(string accessToken)
    {
        Firebase.Auth.Credential credential = Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
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
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
        
    }

    public string getCredentials()
    {
        return this.credentials;
    }

    public void setCredentials(Firebase.Auth.Credential credential)
    {
        this.credentials = ""+credential;

    }
}
