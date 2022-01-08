using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstantServer
{
    // public static string URL_SERVER = "http://localhost:8080";
    public static string URL_SERVER = "https://mighty-shelf-67808.herokuapp.com";
    public static string URL_LOGIN = URL_SERVER + "/login";
    public static string URL_REGISTER = URL_SERVER+ "/accounts/register";
    public static string URL_ACCOUNT = URL_SERVER + "/accounts";
    public static string URL_SETTING = URL_SERVER + "/settings";
    public static string URL_SKILLS = URL_SERVER + "/skills";
    public static string URL_LEVELS = URL_SERVER + "/levels";
    public static string URL_INVENTORY_USER = URL_SERVER + "/inventories/users/";
    
    public static string URL_GAMES = URL_SERVER + "/games";

    public static string URL_NEW_GAME = URL_SERVER + "/games/new";
}
