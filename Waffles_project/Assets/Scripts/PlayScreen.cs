using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScreen : MonoBehaviour
{
	public string loadLevel;
	public GameObject playWindow;
	public GameObject nodeWindow;
	public GameObject scoreWindow;
	public int endFlag=0;
	//public GameObject nextRow;
	
	public void openPlayWindow(int end=0){
		playWindow.SetActive (true);
		nodeWindow.SetActive (false);
		endFlag = end;
	}
	
	public void closePlayWindow(){
		if (endFlag == 1)
		{
			endFlag = 0;
			openScoreWindow();
		}
		playWindow.SetActive (false);
		nodeWindow.SetActive (true);
		
	}

	public void openScoreWindow()
	{
		playWindow.SetActive (false);
		nodeWindow.SetActive (false);
		scoreWindow.SetActive(true);
	}

	/*public void closeScoreWindow()//change scene instead
	{
		scoreWindow.SetActive(false);
	}*/
	
}
