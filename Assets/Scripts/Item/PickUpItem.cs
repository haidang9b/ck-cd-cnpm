using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public GameObject pickUpEffect;
    public Item itemData;
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            if(GameController.instance.itemsInventory.Count < GameController.instance.slots.Length){
                 GameObject clone = Instantiate(pickUpEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
                Destroy(clone, 1.0f);
                GameController.instance.AddItemToInventory(itemData);
            }
            else{
                Debug.Log("Kh the add item, inventory is full");
            }
           
        }
    }
}
