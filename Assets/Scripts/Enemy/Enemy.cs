using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int idEnemy { get; set;}
    public string nameEnemy { get; set;}
    public string description { get; set;}
    public int dameAttack { get; set;}
    public int maxHealth { get; set;}
    public int currentHealth { get; set;}
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
