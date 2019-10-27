using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldConfirmPanel : MonoBehaviour
{

    public GameObject worldMapManager;
    public Text confirmWorldNameText;

    [SerializeField] private Text worldCompletionPercentageText;

    string worldName;
    int worldLevel;
    public int worldSelected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //causes worldconfirmpanel to appear with appropriate text showing
    public void confirmPanelAppear(string worldName, int worldLevel, int worldCompletionPercentage)
    {
        Debug.Log("worldName is" + worldName);
        confirmWorldNameText.text =worldName;
        worldCompletionPercentageText.text = "World Completion Percentage: "+ worldCompletionPercentage.ToString()+ "%";
        this.gameObject.SetActive(true);
        SetWorldLevel(worldLevel);
        SetWorldName(worldName);
       
    }

    //turns confirm panel off
    public void SetInactive()
    {
        this.gameObject.SetActive(false);
    }

  
   
    public void SetWorldName(string worldName)
    {
        this.worldName = worldName;
    }
    public void SetWorldLevel(int worldLevel)
    {
        this.worldLevel = worldLevel;
    }

    //when the world is chosen, actual implementation is passed to world manager 
    public void Proceed()
    {   
        WorldMapManagerScript mapManager = worldMapManager.GetComponent<WorldMapManagerScript>();
        mapManager.OnSelectWorldProceedButton(this.worldLevel);
        this.SetInactive();

    }

}
