using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldConfirmPanel : MonoBehaviour
{
    public GameObject WorldSelect;
    public GameObject StageSelect;
    public GameObject StageConfirmPanel;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Cancel()
    {
        this.gameObject.SetActive(false);
    }

    public void Proceed()
    {
        this.gameObject.SetActive(false);
        WorldSelect.SetActive(false);
        StageSelect.SetActive(true);
        Debug.Log("haha");
        
        Debug.Log("hahah");
        
    }
}
