using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBackgroundController : MonoBehaviour
{
    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        // xử lý music cho background, nhạc nền 
        _audioSource = GetComponent<AudioSource>();
        bool hasMusic = PlayerPrefs.GetInt("HasMusic", 0) == 0? false: true;
        if(hasMusic){
            if(_audioSource.isPlaying == false){
                _audioSource.Play();
            }
        }
        else{
            if(_audioSource.isPlaying == true){
                _audioSource.Pause();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
