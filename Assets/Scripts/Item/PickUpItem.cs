using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public GameObject pickUpEffect;
    public Item itemData;
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            Instantiate(pickUpEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            
            GameController.instance.AddItem(itemData);
        }
    }
}
