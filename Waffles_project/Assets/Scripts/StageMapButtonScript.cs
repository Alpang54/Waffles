using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMapButtonScript : MonoBehaviour
{
   
    private string stageName;
    public int stageLevel;
    public Text stageButtonText;
    private Image stageButtonImage;
    private Button stageButton;

    public GameObject stageMapManager;


    // Start is called before the first frame update
    void Start()
    { 
 
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetStageLevel(int stageLevel)
    {
        this.stageLevel = stageLevel;
        stageButtonText.text = "" + stageLevel;
    }
 
    public void SetStageName(string stageName)
    {   
        this.stageName = stageName;
    }
    public void SetStageButtonImage(Sprite activeOrInactiveSprite)
    {
        stageButtonImage = GetComponent<Image>();
        stageButtonImage.sprite = activeOrInactiveSprite;

    }
    public void SetStageButton(Boolean enabledOrNot)
    {
        stageButton = GetComponent<Button>();
        stageButton.enabled = enabledOrNot;
    }

    //when a stage is selected, pass control to stage manager to proceed
    public void OnSelectStage()
    {
        StageMapManagerScript stageManager = stageMapManager.GetComponent<StageMapManagerScript>();
        stageManager.OnSelectStageButton(this.stageLevel,this.stageName);
        

    }


    
}
