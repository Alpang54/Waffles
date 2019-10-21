using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageConfirmPanel : MonoBehaviour
{

    public GameObject stageMapManager;
    public Text confirmStageNameText;
    public Text stageCompletion;
    string stageName;
    int stageLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //turn on stage confirm panel with appropriate text
    public void confirmPanelAppear(string stageName, int worldLevel, int stageLevel,string stageCompletionPercentage)
    {
        confirmStageNameText.text = worldLevel + "-" +stageLevel+"-"+ stageName;
        stageCompletion.text=stageCompletionPercentage+"%";
        this.gameObject.SetActive(true);
    }


    //turn off stage confirm panel
    public void SetInactive()
    {
        this.gameObject.SetActive(false);
    }

    
    public void SetStageName(string stageName)
    {
        this.stageName = stageName;
    }
    public void SetStageLevel(int stageLevel)
    {
        this.stageLevel = stageLevel;
    }

    //when the stage is selected and confirmed, pass to stage manager to proceed
    public void Play()
    { ///needs to change depending on gameplay
        StageMapManagerScript mapManager = stageMapManager.GetComponent<StageMapManagerScript>();
        this.SetInactive();
        mapManager.OnSelectStagePlayButton(this.stageLevel);

    }
}
