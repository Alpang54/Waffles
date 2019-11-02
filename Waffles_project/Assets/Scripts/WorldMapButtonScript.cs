using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Proyecto26;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Networking;
using Facebook.Unity;
using System;
using System.Threading.Tasks;

public class WorldMapButtonScript : MonoBehaviour
{

    private int worldLevel;
   [SerializeField] private Text worldButtonText;
    private Image worldButtonImage;
    private Button worldButton;
    public GameObject worldMapManager;


 void Start()
    {
       
        

    }


    public void SetWorldLevel(int worldLevel)
    {
        this.worldLevel = worldLevel;
        
    }
  
    public void SetWorldButtonImage(Sprite activeOrInactiveSprite)
    {
        worldButtonImage = GetComponent<Image>();
        worldButtonImage.sprite = activeOrInactiveSprite;

    }
    public void SetWorldButton(Boolean enabledOrNot)
    {
        worldButton = GetComponent<Button>();
        worldButton.enabled = enabledOrNot;
    }

    public void SetText(string text)
    {
        this.worldButtonText.text = text;
    }

    public void OnSelectWorld()
    {

        WorldMapManagerScript mapManager = worldMapManager.GetComponent<WorldMapManagerScript>();
        
        mapManager.OnSelectWorldButton(this.worldLevel);

    }
}

