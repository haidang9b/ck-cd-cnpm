using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public GameObject pickUpEffect;
    public Item itemData;
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            GameObject clone = Instantiate(pickUpEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(clone, 1.0f);
            GameController.instance.AddItem(itemData);
        }
    }
}
