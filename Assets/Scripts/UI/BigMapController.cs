using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMapController : MonoBehaviour
{
    public GameObject cameraBigMap;
    public GameObject toolPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CameraBigMapControl();
    }
    private void CameraBigMapControl(){
        if(Input.GetKeyDown(KeyCode.M)){
            if(GameController.instance.isPaused){
                Resume();
            }
            else{
                Pause();
            }
        }
    }

    private void Resume() {
        cameraBigMap.gameObject.SetActive(false);
        toolPanel.gameObject.SetActive(true);
        Time.timeScale = 1.0f;
        GameController.instance.isPaused = false;
    }
    public void Pause() {
        cameraBigMap.gameObject.SetActive(true);
        toolPanel.gameObject.SetActive(false);
        Time.timeScale = 0;
        GameController.instance.isPaused = true;
    }
}
