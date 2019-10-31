using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 *Simple way to tag the buttons that this current button is linked to as a route for the main stage gameplay
 * @author Mok Wei Min
**/
public class MainStageBtns : MonoBehaviour
{
    [SerializeField]
    GameObject[] possibleRoute;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    /**
    *@return the gameobject array that was tagged to the button as the possible routes
    **/
    public GameObject[] getPossibleRoute()
    {
        return possibleRoute;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
