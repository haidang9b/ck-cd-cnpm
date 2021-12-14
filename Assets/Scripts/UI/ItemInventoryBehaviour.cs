using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventoryBehaviour : MonoBehaviour
{
    
    public int buttonID;
    private Item thisItem;

    private Item GetThisItem(){
        for(int i = 0; i< GameController.instance.items.Count; i++){
            if(buttonID == i){
                thisItem = GameController.instance.items[i];
            }
        }
        return thisItem;
    }

    public void CloseButton(){
        GameController.instance.RemoveItem(GetThisItem());
    }
}
