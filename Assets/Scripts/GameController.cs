using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum EquipmentType{
    Weapon,
    Armor
}

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameObject player;
    public bool isPaused;
    public Slider HealthSlider;
    public Slider ManaSlider;
    public List<Item> itemsInventory = new List<Item>();
    public List<int> itemNumbers = new List<int>();

    public List<Item> equipmentsUsing = new List<Item>();
    public GameObject[] slots;
    public GameObject[] slotsEquipment;
    private AudioSource audioSource;
    private bool hasEffectSound;
    public AudioClip pickupAudio;
    // Start is called before the first frame update
    void Awake(){

        if(instance == null){
            instance = this;
        }
        else{
            if( instance != this){
                Destroy(gameObject);
            }
        }
        
    }
    private void DisplayItems(){
        for(int i = 0; i< itemsInventory.Count;i++){
            slots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,1);
            slots[i].transform.GetChild(0).GetComponent<Image>().sprite = itemsInventory[i].itemSpite;

            slots[i].transform.GetChild(1).GetComponent<Text>().color = new Color(1,1,1,1);
            slots[i].transform.GetChild(1).GetComponent<Text>().text = itemNumbers[i].ToString();
            slots[i].transform.GetChild(1).gameObject.SetActive(true);

            slots[i].transform.GetChild(2).gameObject.SetActive(true);
        }

        for(int i = 0; i< slots.Length; i++ ){
            if(i < itemsInventory.Count){
                Debug.Log("Type items: "+ itemsInventory[i].GetType());
                slots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,1);
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = itemsInventory[i].itemSpite;

                slots[i].transform.GetChild(1).GetComponent<Text>().color = new Color(1,1,1,1);
                slots[i].transform.GetChild(1).GetComponent<Text>().text = itemNumbers[i].ToString();

                slots[i].transform.GetChild(2).gameObject.SetActive(true);
            }
            else{
                slots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,0);
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;

                slots[i].transform.GetChild(1).GetComponent<Text>().color = new Color(1,1,1,0);
                slots[i].transform.GetChild(1).GetComponent<Text>().text = null;

                slots[i].transform.GetChild(2).gameObject.SetActive(false);
            }
            
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        InitData();
        hasEffectSound = PlayerPrefs.GetInt("HasEffectSound", 0) == 1 ? true : false;
        float vol = PlayerPrefs.GetFloat("Volume");
        AudioListener.volume = vol;
    }

    private void InitData(){

    }

    // Update is called once per frame
    void Update()
    {
        DisplayManaAndHealth();
    }
    public void AddItemToInventory(Item _item){
        if(hasEffectSound){
            audioSource.clip = pickupAudio;
            audioSource.Play();
        }
        if(itemsInventory.Contains(_item) == false){
            itemsInventory.Add(_item);
            itemNumbers.Add(1);
        }
        else{
            for(int i = 0; i< itemsInventory.Count; i++){
                if(_item == itemsInventory[i]){
                    itemNumbers[i]++;
                }
            }
        }
        DisplayItems();
    }

    public void RemoveItemInInventory(Item _item) {
        if(itemsInventory.Contains(_item)){
            for(int i = 0; i< itemsInventory.Count; i++){
                if(_item == itemsInventory[i]){
                    itemNumbers[i]--;
                    if(itemNumbers[i] == 0){
                        itemsInventory.Remove(itemsInventory[i]);
                        itemNumbers.Remove(itemNumbers[i]);
                    }
                }
            }
            
        }
        else{
            Debug.Log("No in here");
        }
        DisplayItems();
        
    }

    public void DisplayManaAndHealth(){
        ManaSlider.maxValue = player.GetComponent<PlayerController>().GetMaxMana();
        HealthSlider.maxValue = player.GetComponent<PlayerController>().GetMaxHealth();
        ManaSlider.value = player.GetComponent<PlayerController>().GetCurrentMana();
        HealthSlider.value = player.GetComponent<PlayerController>().GetCurrentHealth();
    }

    // 0 = weapon, 1 = armor
    public void setEquipment(int index, Item data) {
        if( index >=0 && index <2){
            equipmentsUsing[index] = data;
        }
    }

    // public void DisplayEquipment(){
    //     for(int i = 0 ; i< slotsEquipment.Length; i++){
    //         if(equipmentsUsing[i] != null){
    //             // set suggest active = false
    //             slotsEquipment[i].transform.GetChild(1).GetComponent<Image>().color = new Color(1,1,1,0);
    //             slotsEquipment[i].transform.GetChild(1).gameObject.SetActive(false);

    //             // show image equipment
    //             slotsEquipment[i].transform.GetChild(2).GetComponent<Image>().color = new Color(1,1,1,1);
    //             slotsEquipment[i].transform.GetChild(2).GetComponent<Image>().sprite = equipmentsUsing[i].itemSpite;
    //             slotsEquipment[i].transform.GetChild(2).gameObject.SetActive(true);
    //         }
    //         else{
    //             slotsEquipment[i].transform.GetChild(1).GetComponent<Image>().color = new Color(1,1,1,1);
    //             slotsEquipment[i].transform.GetChild(1).gameObject.SetActive(true);

    //             slotsEquipment[i].transform.GetChild(2).GetComponent<Image>().color = new Color(1,1,1,0);
    //             slotsEquipment[i].transform.GetChild(2).GetComponent<Image>().sprite = null;
    //             slotsEquipment[i].transform.GetChild(2).gameObject.SetActive(false);
    //         }
            
    //     }
    // }
}
