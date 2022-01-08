using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Newtonsoft.Json;

public enum EquipmentType{
    Weapon,
    Armor
}

public class GameController : MonoBehaviour
{
    // áp dụng singleton để thao tác khi gọi tới object này
    public static GameController instance;

    public GameObject player;
    public bool isPaused;
    // thanh máu + mana
    public Slider HealthSlider;
    public Slider ManaSlider;

    // dnah sách các item có trong túi đồ + số lượng đi kèm của nó
    public List<Item> itemsInventory = new List<Item>();
    public List<int> itemNumbers = new List<int>();

    // có 2 thứ: giáp + vũ khí, mang vào sẽ + dame và giáp
    public List<Item> equipments = new List<Item>();
    // slot ô chứa đồ
    public GameObject[] slots;
    // slot các item giáp và vũ khí đang mang
    public GameObject[] slotsEquipment;
    private AudioSource audioSource;
    private bool hasEffectSound;
    // âm thanh nhặt đồ
    public AudioClip pickupAudio;
    private Item weaponCurrent = null;
    private Item armorCurrent = null;

    // set devilFruit
    public HashSet<int> devilFruit = new HashSet<int>();
    public HashSet<int> enemiesKilled = new HashSet<int>();

    // skill learned
    public List<Skill> skillPlayer = new List<Skill>();

    // all item
    public List<Item> itemsInGame;
    public Text cointCurrentText;
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

    
    // hiển thị item trong inventory
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
                // Debug.Log("Type items: "+ itemsInventory[i].GetType());
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
        // cần check sound + effect
        float vol = PlayerPrefs.GetFloat("Volume");
        AudioListener.volume = vol;
        GetInventoryItems();
        DisplayItems();
        GetEnemiesKilled();

    }

    private void InitData(){
    //   load data khi đầu game vào
    }

    public void UpdateCoinText(){
        int coinCurrent = player.GetComponent<PlayerController>().GetCoins();
        cointCurrentText.text = coinCurrent + "";
    }

    //  gắn giáp + vũ khí cho nhân vật
    public void AddItemToEquipment(Item item){
        if(item.GetType().ToString() == "Weapon"){
            if(weaponCurrent == null){
                weaponCurrent = item;
            }
            else{
                AddItemToInventory(weaponCurrent);
                weaponCurrent = item;
            }
        }
        if(item.GetType().ToString() == "Armor"){
            if(armorCurrent == null){
                armorCurrent = item;

            }
            else{
                AddItemToInventory(armorCurrent);
                armorCurrent = item;
            }
        }
        DisplayItemsEquipment();
    }

    //  hiển thị các equipments
    private void DisplayItemsEquipment(){
        if(weaponCurrent == null){
            slotsEquipment[0].transform.GetChild(1).GetComponent<Image>().color = new Color(1,1,1,0.4f);

            slotsEquipment[0].transform.GetChild(2).GetComponent<Image>().color = new Color(1,1,1,0);
            slotsEquipment[0].transform.GetChild(2).GetComponent<Image>().sprite = null;
            slotsEquipment[0].transform.GetChild(2).gameObject.SetActive(false);
        }
        else{
            slotsEquipment[0].transform.GetChild(1).GetComponent<Image>().color = new Color(1,1,1,0);

            slotsEquipment[0].transform.GetChild(2).GetComponent<Image>().color = new Color(1,1,1,1);
            slotsEquipment[0].transform.GetChild(2).GetComponent<Image>().sprite = weaponCurrent.itemSpite;
            slotsEquipment[0].transform.GetChild(2).gameObject.SetActive(true);
        }

        if(armorCurrent == null){
            slotsEquipment[1].transform.GetChild(1).GetComponent<Image>().color = new Color(1,1,1,0.4f);

            slotsEquipment[1].transform.GetChild(2).GetComponent<Image>().color = new Color(1,1,1,0);
            slotsEquipment[1].transform.GetChild(2).GetComponent<Image>().sprite = null;
            slotsEquipment[1].transform.GetChild(2).gameObject.SetActive(false);
        }
        else{
            slotsEquipment[1].transform.GetChild(1).GetComponent<Image>().color = new Color(1,1,1,0);

            slotsEquipment[1].transform.GetChild(2).GetComponent<Image>().color = new Color(1,1,1,1);
            slotsEquipment[1].transform.GetChild(2).GetComponent<Image>().sprite = armorCurrent.itemSpite; 
            slotsEquipment[1].transform.GetChild(2).gameObject.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        DisplayManaAndHealth();
        UpdateCoinText();
    }

    // xử lý thêm item khi nhặt đồ
    public void AddItemToInventory(Item _item){
        if(hasEffectSound){
            audioSource.clip = pickupAudio;
            audioSource.Play();
        }
        if(itemsInventory.Count < slots.Length){
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
        }
        else{
            Debug.LogWarning("Khong the add");
        }
        
        DisplayItems();
    }

    // remove item khỏi inventory
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

    //  hiển thị chính xác các slider máu + mana
    public void DisplayManaAndHealth(){
        ManaSlider.maxValue = player.GetComponent<PlayerController>().GetMaxMana();
        HealthSlider.maxValue = player.GetComponent<PlayerController>().GetMaxHealth();
        ManaSlider.value = player.GetComponent<PlayerController>().GetCurrentMana();
        HealthSlider.value = player.GetComponent<PlayerController>().GetCurrentHealth();
    }

    // 0 = weapon, 1 = armor
    public void setEquipment(int index, Item data) {
        if( index >=0 && index <2){
            equipments[index] = data;
        }
    }
    // get data vũ khí và giáp
    public Weapon GetCurrentWeapon() {
        return (Weapon)weaponCurrent;
    }

    public Armor GetCurrentArmor() {
        return (Armor)armorCurrent;
    }

    public void RemoveArmor(){
        armorCurrent = null;
        DisplayItemsEquipment();
    }
    public void RemoveWeapon(){
        weaponCurrent = null;
        DisplayItemsEquipment();
    }

    public void ClearEquipment(){
        if(armorCurrent != null){
            AddItemToInventory(armorCurrent);
            armorCurrent = null;
            DisplayItemsEquipment();
        }
        if(weaponCurrent != null){
            AddItemToInventory(weaponCurrent);
            weaponCurrent = null;
            DisplayItemsEquipment();
        }
    }
    private async void GetEnemiesKilled(){
        string urlRequest = ConstantServer.URL_ACCOUNT + "/" + DBManager.USERNAME + "/killed";
        using var www = UnityWebRequest.Get(urlRequest);
        www.SetRequestHeader("Authorization",DBManager.TOKEN);
        var operation = www.SendWebRequest();
        while(operation.isDone == false){
            await Task.Yield();
        }
        
        if(www.error != null){
            Debug.Log("Error : " + www.error);
        }
        else{
            // Debug.Log("Result EnemyKilledDTO "+ www.downloadHandler.text);
            List<EnemyKilledDTO> result = new List<EnemyKilledDTO>();
            result = JsonConvert.DeserializeObject<List<EnemyKilledDTO>>(www.downloadHandler.text);
            // Debug.Log(www.downloadHandler.text);
            foreach(EnemyKilledDTO e in result){
                enemiesKilled.Add(e.idEnemy);
            }
        }
    }

    private async void GetInventoryItems(){
        string urlRequest = ConstantServer.URL_INVENTORY_USER +  DBManager.USERNAME;
        using var www = UnityWebRequest.Get(urlRequest);
        www.SetRequestHeader("Authorization",DBManager.TOKEN);
        var operation = www.SendWebRequest();
        while(operation.isDone == false){
            await Task.Yield();
        }
        
        if(www.error != null){
                Debug.Log("Error : " + www.error);
        }
        else{
            // Debug.Log("Result "+ www.downloadHandler.text);
            List<InventoryUserDTO> inventories = new List<InventoryUserDTO>();
            inventories = JsonConvert.DeserializeObject<List<InventoryUserDTO>>(www.downloadHandler.text);

            foreach(InventoryUserDTO iu in inventories){
                // Debug.Log("name : " + iu.idItem);
                Item exist = itemsInGame.Find(x => x.idItem == iu.idItem);
                if(exist){
                    for(int j = 0; j< iu.quantity ; j++){
                        AddItemToInventory(exist);
                    }
                }
            }
            DisplayItems();

        }
    }

}
