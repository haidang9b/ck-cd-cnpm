using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject OptionPanel;
    private AudioSource audioSource;    
    public Slider volumeSlider;
    public Toggle toggleMusic;
    public Toggle toggleEffectSound;
    private FaderScript fader;

    // Start is called before the first frame update
    void Start()
    {
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
        Debug.Log("has music " + PlayerPrefs.GetInt("HasMusic"));
    }

    // load data nếu chơi game cũ
    public void ContinuteGame(){
        LoadData();
        StartCoroutine(ChangeLevel());
        SceneManager.LoadScene("SceneGame");
    }
    
    // xử lý load data game cũ player đã chơi
    private void LoadData(){

    }
    // start game mới
    public void NewGame(){
        StartCoroutine(ChangeLevel());
        SceneManager.LoadScene("CutScene");
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
        float vol = PlayerPrefs.GetFloat("Volume");
        volumeSlider.value = vol;
        AudioListener.volume = vol;

        bool hasMusic = PlayerPrefs.GetInt("HasMusic", 0) == 0? false: true;
        bool hasEffectSound = PlayerPrefs.GetInt("HasEffectSound", 0) == 0? false: true;

        toggleMusic.isOn = hasMusic;
        toggleEffectSound.isOn = hasEffectSound;        
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
        StartCoroutine(ChangeLevel());
        SceneManager.LoadScene("Login");
    }
}
