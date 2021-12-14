using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryMenu;
    // Start is called before the first frame update
    void Start()
    {
        inventoryMenu.gameObject.SetActive(true);
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

    private void Resume(){
        inventoryMenu.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        GameController.instance.isPaused = false;
    }

    private void Pause(){
        inventoryMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
        GameController.instance.isPaused = true;
    }
}
