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
    // Start is called before the first frame update
    void Start()
    {
        this.isLoggedIn = false;
        DontDestroyOnLoad(this.gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public bool GetIsLoggedIn()
    {
        return this.isLoggedIn;
    }

    public string GetFirebaseUserId()
    {
        return this.firebaseUserId;
    }

    public Tuple<int,int> GetWorldAndStageLevel()
    {
        return this.worldAndStageLevel;
    }

    public int GetCharacterLevel()
    {
        return this.characterlevel;
    }


    public void SetIsLoggedIn(bool isLoggedIn)
    {
        this.isLoggedIn=isLoggedIn;

    }

    public void SetFireBaseUserId(string firebaseUserId)
    {
        this.firebaseUserId = firebaseUserId;

    }

    public void SetWorldAndStageLevel(Tuple<int,int> worldAndStageLevel)
    {
        this.worldAndStageLevel = worldAndStageLevel;
    }

    public void SetCharacterLevel()
    {
        //set the character level
        this.characterlevel = PlayerPrefs.GetInt("CharacterSelected");
    }
}
