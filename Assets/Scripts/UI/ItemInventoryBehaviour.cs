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
        if(buttonID > GameController.instance.itemsInventory.Count){
            return null;
        }
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
            stringBuilder.AppendFormat("<color=black><size=26><b>Name:</b></size></color> <color=orange><size=26><b>{0}</b></size></color>\n", _item.itemName);

            if(_item.GetType().ToString() == "Weapon"){
                Weapon weapon = (Weapon)_item;
                stringBuilder.AppendFormat("<color=black><size=26><b>Dame:</b></size></color> <color=red><size=26><b>{0}</b></size></color>\n", weapon.dame);
            }
            else if(_item.GetType().ToString() == "Armor"){
                Armor armor = (Armor)_item;
                stringBuilder.AppendFormat("<color=black><size=26><b>Defense:</b></size></color> <color=gray><size=26><b>{0}</b></size></color>\n", armor.defense);
            }
            else if(_item.GetType().ToString() == "HP"){
                HP hp = (HP)_item;
                stringBuilder.AppendFormat("<color=black><size=26><b>+ HP:</b></size></color> <color=red><size=26><b>{0}</b></size></color>\n", hp.rateHP);
            }
            else if(_item.GetType().ToString() == "MP"){
                MP mp = (MP)_item;
                stringBuilder.AppendFormat("<color=black><size=26><b>+ MP:</b></size></color> <color=blue><size=26><b>{0}</b></size></color>\n", mp.rateMP);
            }
            
            stringBuilder.AppendFormat("<color=black><size=26><b>Sell Price:</b></size></color> <color=yellow><size=26><b>{0}</b></size></color>\n", _item.price);
            stringBuilder.AppendFormat("<color=black><size=26><b>Description:</b></size></color> <color=gray><size=26><b>{0}</b></size></color>\n", _item.itemDescription);
            return stringBuilder.ToString();
        }
    }
    // handler exit in area item
    public void OnPointerExit(PointerEventData eventData)
    {
        GetThisItem();
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
                else if(thisItem.GetType().ToString() == "HP"){
                    // dùng HP
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    HP thisHP = (HP)thisItem;
                    if(player != null){
                        //  check xem hp hiện tại có bằng max hp hay không, nếu có thì kh đc dùng
                        int currentHPPlayer = player.GetComponent<PlayerController>().GetCurrentHealth();
                        int maxHPPlayer = player.GetComponent<PlayerController>().GetMaxHealth();
                        if(currentHPPlayer < maxHPPlayer){
                            player.GetComponent<PlayerController>().addHealth(thisHP.rateHP);
                            GameController.instance.RemoveItemInInventory(thisItem);
                        }
                    }
                    
                }
                else if(thisItem.GetType().ToString() == "MP"){
                    //  dùng bình mana
                    //  check xem mana hiện tại có bằng max mana hay không, nếu có thì kh đc dùng
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    MP thisMP = (MP)thisItem;
                    if(player != null){
                        int currentManaPlayer = player.GetComponent<PlayerController>().GetCurrentMana();
                        int maxManaPlayer = player.GetComponent<PlayerController>().GetMaxMana();
                        if(currentManaPlayer < maxManaPlayer){
                            player.GetComponent<PlayerController>().addMana(thisMP.rateMP);
                            GameController.instance.RemoveItemInInventory(thisItem);
                        }
                    }
                }
            }
        }
        
    }
}
