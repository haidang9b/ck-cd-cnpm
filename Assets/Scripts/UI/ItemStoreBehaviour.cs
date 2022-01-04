using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemStoreBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int IDButton{ get; set;}
    public Item thisItem{ get; set;}
    private GameObject player;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(thisItem != null){
            BoughtItem();
            // Debug.Log("Click item " + thisItem.itemName);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(thisItem != null){
            Debug.Log("Enter " + thisItem.itemName + " slot");
          
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if(thisItem != null){
            Debug.Log("Exit " + thisItem.itemName + " slot");

        }
    }
    private void BoughtItem(){
        int coinsCurrent = player.GetComponent<PlayerController>().GetCoins();
        if(coinsCurrent >= thisItem.price){
            player.GetComponent<PlayerController>().reduceCoin(thisItem.price);
            GameController.instance.AddItemToInventory(thisItem);
            Debug.Log("Co the mua");
        }
        else{
            Debug.Log("Khong the mua");
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
