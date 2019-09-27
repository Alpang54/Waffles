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

public class WorldMapScript : MonoBehaviour
{
    [SerializeField]
    private Button[] buttons;
    [SerializeField]
    private string[] stageNames;
    [SerializeField]
    private static int totalWorldMapNodes;
    [SerializeField]
    private static string[] worldNames;

    async static void getWorldNamesfromDatabase()
    {
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage response = await client.GetAsync("https://cz3003-waffles.firebaseio.com/WorldNames/.json"))
            {
                using (HttpContent content = response.Content)
                {
                    string mycontent = await content.ReadAsStringAsync();
                    print(mycontent);
                    Debug.Log("Maybe");
                    if (mycontent != "null") {

                        mycontent = mycontent.Substring(1, mycontent.Length - 2);
                        string[] allWorlds = mycontent.Split(',');
                        totalWorldMapNodes = allWorlds.Length;

                        worldNames = new string[totalWorldMapNodes];
                        string[] worldKeyAndValueArray = mycontent.Split(new Char[] { ',', ':' },
                                StringSplitOptions.RemoveEmptyEntries); 

                        for(int i=0;i<worldKeyAndValueArray.Length; i=i+2)
                        {
                            worldNames[i / 2] = worldKeyAndValueArray[i + 1];}
                        for(int j = 0; j < worldNames.Length; j++)
                        {
                            Debug.Log(worldNames[j]);
                        }


                    }

                    }
                    

  

                }
            }
        }
    

    public void getWorldNames()
    {
        Debug.Log("working?");

        getWorldNamesfromDatabase();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnSubmitA()
    {
        print("start");
        print("working");
        

    }
}

