using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject toolPanel;
    public GameObject inventoryMenu;
    public GameObject equipmentMenu;
    // Start is called before the first frame update
    void Start()
    {
        
        // inventoryMenu.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        InventoryControl();
    }

    private void InventoryControl(){
        if(Input.GetKeyDown(KeyCode.B)){
            if(GameController.instance.isPaused){
                Resume();
            }
            else{
                Pause();
            }
        }
    }

    public void Resume(){
        equipmentMenu.gameObject.SetActive(false);
        inventoryMenu.gameObject.SetActive(false);
        toolPanel.gameObject.SetActive(true);
        Time.timeScale = 1.0f;
        GameController.instance.isPaused = false;
    }

    public void Pause(){
        equipmentMenu.gameObject.SetActive(true);
        inventoryMenu.gameObject.SetActive(true);
        toolPanel.gameObject.SetActive(false);
        Time.timeScale = 0;
        GameController.instance.isPaused = true;
    }
}
