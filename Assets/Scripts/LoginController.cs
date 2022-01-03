using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        if(username == "admin" && password == "123456"){
            ErrorText.text = "Login OK";

        }
        else{
            ErrorText.text = "Username and password is incorrect.";
            
        }
    }
}
