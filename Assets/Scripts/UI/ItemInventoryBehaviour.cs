using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInventoryBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
    // handler hover in area item
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
    // handler exit in area item
    public void OnPointerExit(PointerEventData eventData)
    {
        if(thisItem != null){
            Debug.Log("Exit " + thisItem.itemName + " slot");
            tooltip.HideTooltip();
            tooltip.UpdateTooltip("");
        }
    }
    
    // xử lý click item
    public void OnPointerClick(PointerEventData eventData)
    {
        // kiểm tra chuột trái click 2 lần
        if(eventData.button == PointerEventData.InputButton.Left  && eventData.clickCount == 2){
            GetThisItem();
            if(thisItem != null){
                Debug.Log("Name item clicked : " + thisItem.itemName ); 
                if(thisItem.GetType().ToString() == "Weapon" || thisItem.GetType().ToString() == "Armor"){
                    GameController.instance.AddItemToEquipment(thisItem);
                    GameController.instance.RemoveItemInInventory(thisItem);
                }
            }
        }
        
    }
}
