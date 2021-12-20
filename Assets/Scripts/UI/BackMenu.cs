using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackMenu : MonoBehaviour
{
    private FaderScript fader;
    // Start is called before the first frame update
    public void BackToMenu(){
        
        StartCoroutine(ChangeLevel());
        SceneManager.LoadScene("Menu");
    }

    private IEnumerator ChangeLevel(){
        fader = GetComponent<FaderScript>();
        float begin = fader.BeginFade(1);
        yield return new WaitForSeconds(2f);
    }
}
