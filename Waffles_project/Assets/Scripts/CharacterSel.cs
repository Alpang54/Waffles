using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/**
 * This class is used for the character select page 
 * and sent the user set difficulty 
 * Component CharacterManager
 * @author Fang ShuHui
 * */
public class CharacterSel : MonoBehaviour
{
     
    /*
      This variable is to get the character(question diffcultiy) from the user input
    */
    private int selectedCharacterIndes;

    [Header("list of characters")]
    [SerializeField] private List<CharacterSelectObject> characterList = new List<CharacterSelectObject>();

    /**
     *  this is to reference in the unity 
     */

    [Header("UI References")]
    [SerializeField] private Text description;
    [SerializeField] private Image characterimage;
    [SerializeField] private Text level;

    /**
     * When the scene/page is loaded it will run this function
     */


    private void Start()
    {
        //hide the text
        GameObject.Find("Level").active = false;
        //call function to change the output 
        UpdateCharacterSelectionUI();
    }

    /**
     * When the user click on the left button this function will be activated
     * It will transverse the list by going left
     */
    public void LeftArrow()
    {
        selectedCharacterIndes--;
        if(selectedCharacterIndes < 0)
        {
            selectedCharacterIndes = characterList.Count - 1;
        }

        UpdateCharacterSelectionUI();
    }

    /**
     * When the user click on the right button this function will be activated
     * It will transverse the list by going right
     */
    public void RightArrow()
    {
        selectedCharacterIndes++;
        //if the number is equal to the max character created
        if (selectedCharacterIndes == characterList.Count)
        {
            //reset to 0
            selectedCharacterIndes = 0;
        }

        UpdateCharacterSelectionUI();
    }

    /**
     * This function is for sending the question difficulty when the user click on the 
     * confirm (green button) 
     */

    public void ConfirmCharSel()
    {
        
        //This is to set what the player has choosen for the diffculties of the game
        PlayerPrefs.SetInt("CharacterSelected" , int.Parse(level.text));
       //Test to get the value on what the level is in the console
       Debug.Log(PlayerPrefs.GetInt("CharacterSelected")); 
    }


    /**
     *  This function is used to change the output(e.g image, description)
     */
    private void UpdateCharacterSelectionUI()
    {

        //change the viusal, description, image
        //change image
        characterimage.sprite =characterList[selectedCharacterIndes].splash;       
        //change description
        description.text = characterList[selectedCharacterIndes].description;
        //change level 
        level.text = characterList[selectedCharacterIndes].level;
    }
    /**
     * This function is to set what the list contain 
     */
  
    [System.Serializable]
    public class CharacterSelectObject
    {

        //what does the character list contain
        //Character image
        public Sprite splash;
        //description is the starting level
        public string description;
        //character level
        public string level;

    }
}
