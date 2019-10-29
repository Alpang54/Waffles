using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStageBtns : MonoBehaviour
{
    [SerializeField]
    GameObject[] possibleRoute;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public GameObject[] getPossibleRoute()
    {
        return possibleRoute;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
