using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInventoryBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string[] equipmentTypes = {"Weapon", "Armor"};
    public int buttonID;
    private Item thisItem;

    public Tooltips tooltip;
    Vector2 positionDisplay;
    
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetThisItem();
        if(thisItem != null){
            
            Debug.Log("Enter " + thisItem.itemName + " slot");
            tooltip.ShowTooltip();
            tooltip.UpdateTooltip(GetToolTipDescription(thisItem));
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GameObject.Find("Canvas").transform as RectTransform, Input.mousePosition, null, out positionDisplay);
            tooltip.SetPosition(positionDisplay);
        }
    }
    
    private string GetToolTipDescription(Item _item){
        if(_item == null){
            return "";
        }
        else{
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("<color=black><size=36><b>Name:</b></size></color> <color=orange><size=36><b>{0}</b></size></color>\n", _item.itemName);

            if(_item.GetType().ToString() == "Weapon"){
                Weapon weapon = (Weapon)_item;
                stringBuilder.AppendFormat("<color=black><size=36><b>Dame:</b></size></color> <color=red><size=36><b>{0}</b></size></color>\n", weapon.dame);
            }
            else if(_item.GetType().ToString() == "Armor"){
                Armor armor = (Armor)_item;
                stringBuilder.AppendFormat("<color=black><size=36><b>HP:</b></size></color> <color=red><size=36><b>{0}</b></size></color>\n", armor.HP);
                stringBuilder.AppendFormat("<color=black><size=36><b>Defense:</b></size></color> <color=gray><size=36><b>{0}</b></size></color>\n", armor.HP);
            }
            else if(_item.GetType().ToString() == "HP"){
                HP hp = (HP)_item;
                stringBuilder.AppendFormat("<color=black><size=36><b>+ HP:</b></size></color> <color=red><size=36><b>{0}</b></size></color>\n", hp.rateHP);
            }
            else if(_item.GetType().ToString() == "MP"){
                MP mp = (MP)_item;
                stringBuilder.AppendFormat("<color=black><size=36><b>+ MP:</b></size></color> <color=blue><size=36><b>{0}</b></size></color>\n", mp.rateMP);
            }
            
            stringBuilder.AppendFormat("<color=black><size=36><b>Sell Price:</b></size></color> <color=yellow><size=36><b>{0}</b></size></color>\n", _item.price);
            stringBuilder.AppendFormat("<color=black><size=36><b>Description:</b></size></color> <color=gray><size=36><b>{0}</b></size></color>\n", _item.itemDescription);
            return stringBuilder.ToString();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(thisItem != null){
            Debug.Log("Exit " + thisItem.itemName + " slot");
            tooltip.HideTooltip();
            tooltip.UpdateTooltip("");
        }
    }
}
