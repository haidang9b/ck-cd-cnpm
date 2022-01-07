using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCUpgradeController : MonoBehaviour, Interactable
{
    public Button buttonUpgradeLevel;
    public Text levelCurrentText;
    public Text levelNextText;
    public Text totalCostNextText;
    // label chỉ số hiện tại


    public Text currentDameText;
    public Text currentMaxHealthText;
    public Text currentMaxManaText;
    public Text currentDefenseText;
    // end label chỉ số hiện tại


    // label next level;
    public Text nextDameText;
    public Text nextMaxHealthText;
    public Text nextMaxManaText;
    public Text nextDefenseText;

    // end label next level;

    public GameObject panelMain;
    public GameObject panelNPCUpgrade;
    void Start(){
        CloseDialogPanel();
    }
    public void Interact()
    {
        panelNPCUpgrade.SetActive(true);
        Debug.Log("Interacting with NPC upgrade");
    }

    public void CloseDialogPanel(){
        panelNPCUpgrade.SetActive(false);
    }

    public void OpenUpgrade(){
        CloseDialogPanel();
        panelMain.SetActive(true);
        LoadLabel();
    }

    public void CloseUpgradePanel(){
        panelMain.SetActive(false);
        panelNPCUpgrade.SetActive(false);
    }
    
    private void LoadLabel(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null){
            Level currentLevel = player.GetComponent<PlayerController>().getCurrentLevel();
            
            Level nextLevel = player.GetComponent<PlayerController>().getNextLevel();
            // set label cho chỉ số hiện tại
            currentDameText.text = currentLevel.dameAttack + "";
            currentDefenseText.text = currentLevel.defense + "";
            currentMaxManaText.text = currentLevel.maxMana + "";
            currentMaxHealthText.text = currentLevel.maxHealth + "";
            levelCurrentText.text = "Level "+currentLevel.idLevel;
            

            // set label cho chỉ số cấp tiếp theo
            if(nextLevel != null ){
                nextDameText.text = nextLevel.dameAttack + "";
                nextDefenseText.text = nextLevel.defense + "";
                nextMaxManaText.text = nextLevel.maxMana + "";
                nextMaxHealthText.text = nextLevel.maxHealth + "";
                totalCostNextText.text = "Total cost: " + nextLevel.fee;
                levelNextText.text = "Level "+nextLevel.idLevel;
                // disable if tiền không đủ, enable nếu tiền đủ
                buttonUpgradeLevel.interactable= player.GetComponent<PlayerController>().GetCoins() >= nextLevel.fee;
            }
            else{
                nextDameText.text = "";
                nextDefenseText.text = "";
                nextMaxManaText.text = "";
                nextMaxHealthText.text = "";
                totalCostNextText.text = "Max level";
                levelNextText.text = "Max Level ";
                buttonUpgradeLevel.interactable= false;
            }
            
        }
    }

    public void UpgradeLevel(){
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null){
            Level nextLevel = player.GetComponent<PlayerController>().getNextLevel();
            player.GetComponent<PlayerController>().UpgradeLevel();
            player.GetComponent<PlayerController>().reduceCoin(nextLevel.fee);
        }
        LoadLabel();
    }
}
