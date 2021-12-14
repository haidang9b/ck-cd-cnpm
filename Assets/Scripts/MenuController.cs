using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject OptionPanel;

    // Start is called before the first frame update
    void Start()
    {
        // OpenMain();
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
    public void ContinuteGame(){

    }
    public void NewGame(){

    }

    public void OpenOptions(){
        MainPanel.SetActive(false);
        OptionPanel.SetActive(true);
    }

    public void OpenMain(){
        MainPanel.SetActive(true);
        OptionPanel.SetActive(false);
    }
}
