﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSel : MonoBehaviour
{
    //figure out what is being selected
    private int selectedCharacterIndes;

    [Header("list of characters")]
    //able to change in the inspector
    [SerializeField] private List<CharacterSelectObject> characterList = new List<CharacterSelectObject>();

    // Start is called before the first frame update
    //this is to reference in the unity 
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image characterimage;
    

    private void Start()
    {
        UpdateCharacterSelectionUI();
    }

    public void LeftArrow()
    {
        selectedCharacterIndes--;
        if(selectedCharacterIndes < 0)
        {
            selectedCharacterIndes = characterList.Count - 1;
        }

        UpdateCharacterSelectionUI();
    }

    public void RightArrow()
    {
        selectedCharacterIndes++;
        if (selectedCharacterIndes == characterList.Count)
        {
            selectedCharacterIndes = 0;
        }

        UpdateCharacterSelectionUI();
    }

    public void ConfirmCharSel()
    {
        //This is to set what the player has choosen for the diffculties of the game
        PlayerPrefs.SetInt("CharacterSelected" , selectedCharacterIndes);
       //Test to get the value on what the level is in the console
       Debug.Log(PlayerPrefs.GetInt("CharacterSelected")); 
    }


    private void UpdateCharacterSelectionUI()
    {
        //change the viusal, description, image
        //change image
        characterimage.sprite =characterList[selectedCharacterIndes].splash;       
        //change description
        description.text = characterList[selectedCharacterIndes].description;
    }

    [System.Serializable]
    public class CharacterSelectObject
    {
        //what does the character list contain
        //change the character
        public Sprite splash;
        //description is the starting level
        public string description;
    }
}