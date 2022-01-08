using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    follow,
    attack,
    back
}

public class Enemy : MonoBehaviour
{
    public int idEnemy ;
    public string nameEnemy ;
    public string description;
    public int dameAttack = 10;
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int coinBonus = 10;
    // Start is called before the first frame update
    // void Start()
    // {
    //     currentHealth = maxHealth;
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    public void ReduceHealth(int dame){
        currentHealth = currentHealth - dame;
        if(currentHealth <=0){
            gameObject.SetActive(false);
            GameController.instance.enemiesKilled.Add(idEnemy);
            AddCoinsForUser();
        }
    }

    protected void AddCoinsForUser(){
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if(obj == null){
            return;
        }
        if(coinBonus >0){
            obj.GetComponent<PlayerController>().AddCoins(coinBonus);
        }
    }

    protected void DisplayObject(){
        if(GameController.instance.enemiesKilled.Contains(idEnemy)){
            gameObject.SetActive(false);
        }
        else{
            gameObject.SetActive(true);
        }
    }
}
