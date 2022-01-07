using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    public InputField UsernameTextField;
    public InputField PasswordTextField;
    public Text ErrorText;
    // Start is called before the first frame update
    void Start()
    {
        ErrorText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickLogin(){
        string username = UsernameTextField.text;
        string password = PasswordTextField.text;

        if(username.Length == 0 || password.Length == 0){
            ErrorText.text = "Please enter your username and password.";
            return;
        }
        StartCoroutine(Login(username, password));
    }

    // gửi request login và lấy token
    IEnumerator Login(string user, string pass){
        LoginDTO login = new LoginDTO{username = user, password = pass};
        string logindataJsonString = JsonConvert.SerializeObject(login);
        var www = new UnityWebRequest (ConstantServer.URL_LOGIN, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(logindataJsonString);
        www.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.error != null)
        {
            ErrorText.text = "Username or password incorrect.";
            // Debug.Log("Error: " + www.error);
        }
        else
        {
            // ErrorText.text = "Login OK";
            // Dictionary<string,string> hdrs = www.GetResponseHeaders();
            // string s = "";
            // foreach (KeyValuePair<string,string> hdr in hdrs) {
            //     s += hdr.Key + " => " + hdr.Value + "\n";
            // }
            // Debug.Log(s);
            Debug.Log("token "+ www.downloadHandler.text);
            DBManager.TOKEN = www.downloadHandler.text;
            DBManager.USERNAME = user;
            PlayerPrefs.SetString("token",  www.downloadHandler.text);
            // while (PlayerPrefs.GetString("token" ) == "") {
            //     DBManager.TOKEN = www.downloadHandler.text;
            //     DBManager.USERNAME = user;
            //     PlayerPrefs.SetString("token",  www.downloadHandler.text);
            // }
            // Debug.Log("token is mmmmmm "+ www.GetResponseHeader("token"));
            PlayerPrefs.Save();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        }
    }

    private IEnumerator waiting(){
        yield return new WaitForSeconds(2);
    }



    
}
