using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{


    private bool isLoggedIn;
    private string firebaseUserId;
    private Tuple<int, int> worldAndStageLevel;
    private int characterlevel;
    private string facebookUsername;
    private string stageName;
    // Start is called before the first frame update
    void Start()
    {
        this.isLoggedIn = false;
        DontDestroyOnLoad(this.gameObject);

    }

    public void SetFBUserName(string user)
    {
        facebookUsername = user;
    }

    public string GetFBUserName()
    {
        return facebookUsername;
    }
    public bool GetIsLoggedIn()
    {
        return this.isLoggedIn;
    }

    public string GetFirebaseUserId()
    {
        return this.firebaseUserId;
    }

    public Tuple<int, int> GetWorldAndStageLevel()
    {
        return this.worldAndStageLevel;
    }

    public int GetCharacterLevel()
    {
        return (PlayerPrefs.GetInt("CharacterSelected") - 1);
    }


    public void SetIsLoggedIn(bool isLoggedIn)
    {
        this.isLoggedIn = isLoggedIn;

    }

    public void SetFireBaseUserId(string firebaseUserId)
    {
        this.firebaseUserId = firebaseUserId;

    }

    public void SetWorldAndStageLevel(Tuple<int, int> worldAndStageLevel)
    {
        this.worldAndStageLevel = worldAndStageLevel;
    }
    public void SetStageName(string s)
    {

    }
    public string GetStageName()
    {
        return stageName;
    }

    public void SetCharacterLevel()
    {
        //set the character level
        characterlevel = PlayerPrefs.GetInt("CharacterSelected");
    }
}
