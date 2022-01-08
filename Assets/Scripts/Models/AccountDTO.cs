using System.Collections.Generic;

[System.Serializable]
public class AccountDTO
{
    public string username { get; set;}
    public string password { get;set;}
    public int coin { get; set; }
    public int currentHealth {get;set;}
    public int currentMana {get; set;}
    public float x {get; set; }
    public float y {get; set; }
    public int idLevel {get; set; }
}

[System.Serializable]
public class SettingDTO
{
    public string username { get; set;}
    public int volume { get; set;}
    public bool hasEffect { get; set;}
    public bool hasMusic { get; set;}

}

[System.Serializable]
public class Killed{

}
[System.Serializable]
public class WeaponDTO{
    public string id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int price { get; set; }
    public int dame { get; set; }
}

[System.Serializable]
public class ArmorDTO{
    public string id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int price { get; set; }
    public int dame { get; set; }
}

[System.Serializable]
public class HpDTO{
    public string id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int price { get; set; }
    public int rateHP { get; set; }
}

[System.Serializable]
public class MpDTO{
    public string id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int price { get; set; }
    public int rateMP { get; set; }
}

[System.Serializable]
public class InventoryUserDTO {
    public string username { get; set; }
    public string idItem { get; set; }
    public int quantity {get;set;}
}

[System.Serializable]

public class LevelDTO
{
    public int idLevel { get; set; }
    public int dameAttack {get;set;}
    public int defense { get; set; }
    public int maxMana { get; set; }
    public int maxHealth {get;set;}
    public int fee {get; set; }
}

[System.Serializable]
public class DevilFruitDTO{
    public int id{get;set;}
}

[System.Serializable]
public class DevilFruitUsenameDTO{
    public int id{get;set;}
    public string username { get; set; }
}

[System.Serializable]
public class SaveUserDTO
{
    public string username { get; set;}
    public int coin { get; set; }
    public int currentHealth {get;set;}
    public int currentMana {get; set;}
    public float x {get; set; }
    public float y {get; set; }
    public int idLevel {get; set; }
    public List<InventoryUserDTO> inventory {get;set;}
    public SettingDTO setting { get; set; }
    public List<DevilFruitDTO>  devilFruit {get;set;}

    public List<SkillIDDTO> skill {get;set;}
    public List<EnemyIDDTO> enemy {get;set;}
}


[System.Serializable]
public class SkillUserDTO{
    public string idSkill { get; set; }
    public string username {get;set;}
}

[System.Serializable]
public class SkillIDDTO {
    public string id { get;set;}
}
[System.Serializable]
public class LoginDTO
{
    public string username { get; set;}
    public string password { get;set;}
}

[System.Serializable]
public class EnemyIDDTO{
    public int id { get; set;}
}

[System.Serializable]
public class EnemyKilledDTO{
    public int id { get; set;}
    public string username {get;set;}
    public int idEnemy {get;set;}
}