using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Threading.Tasks;

public class MenuController : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject OptionPanel;
    public GameObject GuidePanel;
    private AudioSource audioSource;    
    public Slider volumeSlider;
    public Toggle toggleMusic;
    public Toggle toggleEffectSound;
    private FaderScript fader;

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(GetSetting());
        GetSettings();
        LoadVolume();
        audioSource = GetComponent<AudioSource>();
        // OpenMain();
    }

    // Update is called once per frame
    void Update()
    {
        float vol = PlayerPrefs.GetFloat("Volume");
        AudioListener.volume = vol;

        bool hasMusic = PlayerPrefs.GetInt("HasMusic", 0) == 0 ? false : true;
        if(hasMusic){
            if(audioSource.isPlaying == false){
                audioSource.Play();
            }
        }
        else{
            if(audioSource.isPlaying){
                audioSource.Pause();
            }
        }
    }

    // load data nếu chơi game cũ
    public void ContinuteGame(){
        PlayerPrefs.Save();
        LoadData();
        StartCoroutine(ChangeLevel());
        SceneManager.LoadScene("SceneGame");
    }
    
    // xử lý load data game cũ player đã chơi
    private void LoadData(){
        
    }
    

    //  get request - lấy cài đặt

    private async void GetSettings(){
        string username = DBManager.USERNAME;
        string urlRequest = ConstantServer.URL_SETTING+"/"+username;
        using var www = UnityWebRequest.Get(urlRequest);
        www.SetRequestHeader("Authorization",DBManager.TOKEN);
        var operation = www.SendWebRequest();
        while(operation.isDone == false){
            await Task.Yield();
        }
        
        if(www.error != null){
                // Debug.Log("Error : " + www.error);
        }
        else{
            // Debug.Log("Result "+ www.downloadHandler.text);
            SettingDTO setting = new SettingDTO();
            setting = JsonConvert.DeserializeObject<SettingDTO>(www.downloadHandler.text);

            volumeSlider.value = setting.volume/100.0f;
            AudioListener.volume = setting.volume/100.0f;
            toggleMusic.isOn = setting.hasMusic;
            toggleEffectSound.isOn = setting.hasEffect;  
            PlayerPrefs.SetFloat("Volume", setting.volume/100.0f);
            PlayerPrefs.SetInt("HasMusic", setting.hasMusic ? 1 : 0);
            PlayerPrefs.SetInt("HasEffectSound", setting.hasEffect ? 1 : 0);
        }

    }
    public void OpenGuide(){
        MainPanel.SetActive(false);
        GuidePanel.SetActive(true);
    }
    public void CloseGuide(){
        Debug.Log("click");
        MainPanel.SetActive(true);
        GuidePanel.SetActive(false);
    }

    // start game mới
    public void NewGame(){
        ResetGameForUser();
        StartCoroutine(ChangeLevel());
        SceneManager.LoadScene("CutScene");
    }

    private async void ResetGameForUser(){
        var urlRequest = ConstantServer.URL_NEW_GAME + "/" + DBManager.USERNAME;
        var www = new UnityWebRequest (urlRequest, "POST");
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Authorization",DBManager.TOKEN);
        var operation = www.SendWebRequest();
        while(operation.isDone == false){
            await Task.Yield();
        }
        if (www.error != null)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            Debug.Log("Rest to new Game:OK");
        }
    }

    public void OpenOptions(){
        MainPanel.SetActive(false);
        OptionPanel.SetActive(true);
    }

    public void OpenMain(){
        MainPanel.SetActive(true);
        OptionPanel.SetActive(false);
    }

    // load âm thanh, hiệu ứng
    void LoadVolume(){
        
    }

    public void VolumeChanged(){
        float vol = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", vol);
    }

    public void SetToggleMusic(){
        PlayerPrefs.SetInt("HasMusic", toggleMusic.isOn ? 1 : 0);
    }

    public void SetToggleEffectSound(){
        PlayerPrefs.SetInt("HasEffectSound", toggleEffectSound.isOn ? 1 : 0);
    }
    private IEnumerator ChangeLevel(){
        fader = GetComponent<FaderScript>();
        float begin = fader.BeginFade(1);
        yield return new WaitForSeconds(2f);
    }
    public void Logout(){
        // clear data
        DBManager.USERNAME = "";
        DBManager.TOKEN = "";
        StartCoroutine(ChangeLevel());
        SceneManager.LoadScene("Login");
    }
}
