using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameObject player;
    public bool isPaused;
    public Slider HealthSlider;
    public Slider ManaSlider;
    public List<Item> items = new List<Item>();
    public List<int> itemNumbers = new List<int>();
    public GameObject[] slots;
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
        DontDestroyOnLoad(gameObject);
    }
    private void DisplayItems(){
        for(int i = 0; i< items.Count;i++){
            slots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,1);
            slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].itemSpite;

            slots[i].transform.GetChild(1).GetComponent<Text>().color = new Color(1,1,1,1);
            slots[i].transform.GetChild(1).GetComponent<Text>().text = itemNumbers[i].ToString();
            slots[i].transform.GetChild(1).gameObject.SetActive(true);

            slots[i].transform.GetChild(2).gameObject.SetActive(true);
        }

        for(int i = 0; i< slots.Length; i++ ){
            if(i < items.Count){
                slots[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1,1,1,1);
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].itemSpite;

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
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        DisplayManaAndHealth();
    }
    public void AddItem(Item _item){
        if(items.Contains(_item) == false){
            items.Add(_item);
            itemNumbers.Add(1);
        }
        else{
            for(int i = 0; i< items.Count; i++){
                if(_item == items[i]){
                    itemNumbers[i]++;
                }
            }
        }
        DisplayItems();
    }

    public void RemoveItem(Item _item) {
        if(items.Contains(_item)){
            for(int i = 0; i< items.Count; i++){
                if(_item == items[i]){
                    itemNumbers[i]--;
                    if(itemNumbers[i] == 0){
                        items.Remove(items[i]);
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
}
