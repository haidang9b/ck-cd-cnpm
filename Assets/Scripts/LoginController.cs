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

    private bool inPageLogin = true;
    public Text ErrorText;
    public Text LoginText;
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

        if(inPageLogin){
            string username = UsernameTextField.text;
            string password = PasswordTextField.text;

            if(username.Length == 0 || password.Length == 0){
                ErrorText.text = "Please enter your username and password.";
                return;
            }
            StartCoroutine(Login(username, password));
        }
        else{
            LoginText.text = "LOGIN";
            UsernameTextField.text = "";
            PasswordTextField.text = "";
            ErrorText.text = "";
            inPageLogin = true;
        }
    }

    public void ClickRegister(){
        if(inPageLogin){
            LoginText.text = "Register";
            UsernameTextField.text = "";
            PasswordTextField.text = "";
            ErrorText.text = "";
            inPageLogin = false;
        }
        else{
            string username = UsernameTextField.text;
            string password = PasswordTextField.text;
            if(username.Length == 0 || password.Length == 0){
                ErrorText.text = "Please enter your username and password.";
                return;
            }
            StartCoroutine(Register(username, password));
        }
    }

    IEnumerator Register(string user, string pass){
        LoginDTO login = new LoginDTO{username = user, password = pass};
        string logindataJsonString = JsonConvert.SerializeObject(login);
        var www = new UnityWebRequest (ConstantServer.URL_REGISTER, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(logindataJsonString);
        www.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();

        if (www.error != null)
        {
            ErrorText.text = "This username already exists";
        }
        else
        {
            ErrorText.text = "Register successfully";
        }
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
        }
        else
        {
            DBManager.TOKEN = www.downloadHandler.text;
            DBManager.USERNAME = user;
            PlayerPrefs.SetString("token",  www.downloadHandler.text);
            PlayerPrefs.Save();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        }
    }

    private IEnumerator waiting(){
        yield return new WaitForSeconds(2);
    }
}
