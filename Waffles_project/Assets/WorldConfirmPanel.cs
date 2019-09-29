using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldConfirmPanel : MonoBehaviour
{

    public GameObject worldMapManager;
    public Text confirmWorldNameText;

    string worldName;
    int worldLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //causes worldconfirmpanel to appear with appropriate text showing
    public void confirmPanelAppear(string worldName, int worldLevel)
    {   
        confirmWorldNameText.text = worldLevel + "-" + worldName;
        this.gameObject.SetActive(true);
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
        this.SetInactive();
        mapManager.OnSelectWorldProceedButton(this.worldLevel);

    }

}
