using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilFruits : MonoBehaviour
{
    public int id;
    public GameObject pickUpEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.instance.devilFruit.Contains(id)){
            gameObject.SetActive(false);
        }
        else{
            gameObject.SetActive(true);
        }
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            GameObject clone = Instantiate(pickUpEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(clone, 1.0f);
            GameController.instance.devilFruit.Add(id);
        }
    }
    
}
