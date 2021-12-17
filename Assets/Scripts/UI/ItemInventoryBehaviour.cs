using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventoryBehaviour : MonoBehaviour
{
    private string[] equipmentTypes = {"Weapon", "Armor"};
    public int buttonID;
    private Item thisItem;

    private Item GetThisItem(){
        for(int i = 0; i< GameController.instance.itemsInventory.Count; i++){
            if(buttonID == i){
                thisItem = GameController.instance.itemsInventory[i];
            }
        }
        return thisItem;
    }

    public void CloseButton(){
        GameController.instance.RemoveItemInInventory(GetThisItem());
    }

    void Update(){
        float x = transform.position.x;
        float y =transform.position.y;
        Rect bounds = new Rect(x-50, y-50,100,100); // kiem tra click trong vung can click
        if(Input.GetMouseButtonDown(0) && bounds.Contains(Input.mousePosition)){
            if(GetThisItem() != null){
                if(GetThisItem().GetType().ToString() == equipmentTypes[0] || GetThisItem().GetType().ToString() == equipmentTypes[1] ){
                    AddToEquipment(GetThisItem());
                }
                
                Debug.Log("click at inventory name = "+ gameObject.name );
            }
        }
    }

    private void AddToEquipment(Item item) {
        Debug.Log(GameController.instance.equipmentsUsing[0]);
        // if(item.GetType().ToString() =="Weapon"){
        //     Debug.Log(equipmentsUsing[0].ToString());
        //     if(GameController.instance.equipmentsUsing[0] == null){
        //         GameController.instance.RemoveItemInInventory(item);
        //         GameController.instance.setEquipment(0, item);
        //     }
        //     else{
        //         GameController.instance.RemoveItemInInventory(item);
        //         Item oldItem = GameController.instance.equipmentsUsing[0];
        //         GameController.instance.setEquipment(0, item);
        //         GameController.instance.AddItemToInventory(oldItem);
        //     }


        // }
        // else if(item.GetType().ToString() =="Armor"){
        //     if(GameController.instance.equipmentsUsing[1] == null){
        //         GameController.instance.RemoveItemInInventory(item);
        //         GameController.instance.setEquipment(1, item);
        //     }
        //     else{
        //         GameController.instance.RemoveItemInInventory(item);
        //         Item oldItem = GameController.instance.equipmentsUsing[1];
        //         GameController.instance.setEquipment(1, item);
        //         GameController.instance.AddItemToInventory(oldItem);
        //     }
        // }
    }
}
