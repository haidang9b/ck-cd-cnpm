using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoController : MonoBehaviour
{
    private GameObject player;
    public Text txtDame;
    public Text txtDefense;
    public Text txtMaxHP;
    public Text txtMaxMP;
    public GameObject toolPanel;
    public GameObject infomationPanel;

    public GameObject[] areaSkills;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        InformationControl();
    }

    private void InformationControl(){
        if(Input.GetKeyDown(KeyCode.I)){
            if(GameController.instance.isPaused){
                Resume();
            }
            else{
                Pause();
            }
        }
    }

    private void Resume(){
        infomationPanel.SetActive(false);
        toolPanel.SetActive(true);
        Time.timeScale = 1.0f;
        GameController.instance.isPaused = false;
    }
    public void Pause(){
        infomationPanel.SetActive(true);
        toolPanel.SetActive(false);
        Time.timeScale = 0;
        GameController.instance.isPaused = true;

        int dameAttack = player.GetComponent<PlayerController>().GetDame();
        int defense = player.GetComponent<PlayerController>().GetDefense();
        int maxHP = player.GetComponent<PlayerController>().GetMaxHealth();
        int maxMP = player.GetComponent<PlayerController>().GetMaxMana();

        txtDame.text = dameAttack + "";
        txtDefense.text = defense + "";
        txtMaxHP.text = maxHP + "";
        txtMaxMP.text = maxMP + "";

        DisplaySkill();
    }

    private void DisplaySkill(){
        List<Skill> skills = player.GetComponent<PlayerController>().GetSkillsPlayer();
        foreach(var item in areaSkills){
            item.transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,0.25f);
        }


        foreach(Skill skill in skills){
            foreach(var item in areaSkills){
                if(skill.name == item.name){
                    Debug.Log("Đã nhận " + skill.name);
                    item.transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,1);
                }
            }
        }
    }
}
