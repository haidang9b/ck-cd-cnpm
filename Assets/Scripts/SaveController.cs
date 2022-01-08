using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
    public GameObject panelMain;
    public GameObject panelTool;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPanel(){
        panelMain.SetActive(true);
        panelTool.SetActive(false);
        Time.timeScale = 0.0f;
        GameController.instance.isPaused = true;
    }

    public void ClosePanel(){
        panelMain.SetActive(false);
        panelTool.SetActive(true);
        Time.timeScale = 1.0f;
        GameController.instance.isPaused = false;
    }

    public void SaveAndExit(){
        // save data in here
        Time.timeScale = 1.0f;
        var data = PreprocessingData();
        SaveGame(data);
    }

    private async void SaveGame(SaveUserDTO data){
        var urlRequest = ConstantServer.URL_GAMES + "/" + DBManager.USERNAME;
        string saveDataJsonString = JsonConvert.SerializeObject(data);
        var www = new UnityWebRequest (urlRequest, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(saveDataJsonString);
        www.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Authorization",DBManager.TOKEN);

        var operation = www.SendWebRequest();
        while(operation.isDone == false){
            await Task.Yield();
        }
        if (www.error != null)
        {
            Debug.Log("Error SaveGame: " + www.error);
        }
        else
        {
            Debug.Log("Save Game: OK ");
        }
    }

    private SaveUserDTO PreprocessingData(){
        // back đồ ra khỏi equipment
        GameController.instance.ClearEquipment();
        // tạo ra object setting;
        SettingDTO st = new SettingDTO();
        var hasMusicSt = PlayerPrefs.GetInt("HasMusic", 0) == 0 ? false : true;
        var hasEffectSoundSt = PlayerPrefs.GetInt("HasEffectSound", 0) == 0 ? false : true;
        float volumeSt = PlayerPrefs.GetFloat("Volume")*100;
        st.username = DBManager.USERNAME;
        st.hasEffect = hasEffectSoundSt;
        st.hasMusic = hasMusicSt;
        st.volume = (int)volumeSt;
        // lấy vị trí của player
        Vector3 userPosition = player.transform.position;
        SaveUserDTO saveData = new SaveUserDTO();
        saveData.username = DBManager.USERNAME;
        saveData.coin = player.GetComponent<PlayerController>().GetCoins();
        saveData.currentHealth = player.GetComponent<PlayerController>().GetCurrentHealth();
        saveData.currentMana = player.GetComponent<PlayerController>().GetCurrentMana();
        saveData.x = userPosition.x;
        saveData.y = userPosition.y;
        saveData.idLevel = player.GetComponent<PlayerController>().getCurrentLevel().idLevel;
        saveData.setting = st;

        // lay tat ca skills da hoc cua user
        List<Skill> userSkills = player.GetComponent<PlayerController>().GetSkillsPlayer();
        List<SkillIDDTO> skillIDDTOs = new List<SkillIDDTO>();
        foreach(var s in userSkills){
            skillIDDTOs.Add(new SkillIDDTO{id = s.id});
        }

        List<InventoryUserDTO> inventoryUsers = new List<InventoryUserDTO>();
        for(int i = 0 ; i<GameController.instance.itemsInventory.Count;i++ ){
            InventoryUserDTO tmp = new InventoryUserDTO();
            tmp.username = DBManager.USERNAME;
            tmp.idItem = GameController.instance.itemsInventory[i].idItem;
            tmp.quantity = GameController.instance.itemNumbers[i];
            inventoryUsers.Add(tmp);
        }
        saveData.inventory = inventoryUsers;
        List<DevilFruitDTO> dvfs = new List<DevilFruitDTO>();
        foreach(var i in GameController.instance.devilFruit){
            dvfs.Add(new DevilFruitDTO{id = i});
        }
        saveData.devilFruit = dvfs;
        saveData.skill = skillIDDTOs;

        List<EnemyIDDTO> es = new List<EnemyIDDTO>();
        foreach(var j in GameController.instance.enemiesKilled){
            es.Add( new EnemyIDDTO{id = j});
        }
        saveData.enemy = es;
        return saveData;
    }
    public void DontSave(){
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }
}
