using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionController : MonoBehaviour
{
    public GameObject panelMain;
    public GameObject toolPanel;
    public Text contentText;

    public GameObject[] slots;
    // Start is called before the first frame update
    void Start()
    {
        displayDevilFruits();
    }

    // Update is called once per frame
    void Update()
    {
        MissionControl();
        
    }

    private void MissionControl(){
        if(Input.GetKeyDown(KeyCode.M)){
            if(GameController.instance.isPaused){
                Resume();
            }
            else{
                Pause();
            }
        }
    }

    public void Resume(){
        panelMain.gameObject.SetActive(false);
        toolPanel.gameObject.SetActive(true);
        Time.timeScale = 1.0f;
        GameController.instance.isPaused = false;
    }

    public void Pause(){
        displayDevilFruits();
        panelMain.gameObject.SetActive(true);
        toolPanel.gameObject.SetActive(false);
        Time.timeScale = 0;
        GameController.instance.isPaused = true;
    }
    private void displayDevilFruits(){
        for(int i = 0; i < slots.Length; i ++ ){
            slots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,0.5f);
        }

        if(slots.Length >= GameController.instance.devilFruit.Count){
            for(int i = 0; i < GameController.instance.devilFruit.Count; i++){
                slots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,1);
            }
        }
        contentText.text = "You have collected "+GameController.instance.devilFruit.Count+" devil fruits";
    }
}
