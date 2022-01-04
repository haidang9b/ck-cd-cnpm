using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemEquipmentBehavior : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Item thisItem;
    public int equipmentID;
    public Tooltips tooltip;
    Vector2 positionDisplay;

    private void GetThisItemEquipment(){
        // weapon
        if(equipmentID == 0){
            thisItem = GameController.instance.GetCurrentWeapon();
        }
        // armor
        if(equipmentID == 1){
            thisItem = GameController.instance.GetCurrentArmor();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
            
            stringBuilder.AppendFormat("<color=black><size=26><b>Sell Price:</b></size></color> <color=yellow><size=26><b>{0}</b></size></color>\n", _item.price);
            stringBuilder.AppendFormat("<color=black><size=26><b>Description:</b></size></color> <color=gray><size=26><b>{0}</b></size></color>\n", _item.itemDescription);
            return stringBuilder.ToString();
        }
    }

    // xử lý khi hover 
    public void OnPointerEnter(PointerEventData eventData)
    {
        GetThisItemEquipment();
        if(thisItem != null){
            
            // Debug.Log("Enter " + thisItem.itemName + " slot");
            tooltip.ShowTooltip();
            tooltip.UpdateTooltip(GetToolTipDescription(thisItem));
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GameObject.Find("Canvas").transform as RectTransform, Input.mousePosition, null, out positionDisplay);
            tooltip.SetPosition(positionDisplay);
        }
    }
    
    // Xử lý khi out ra khỏi vùng item
    public void OnPointerExit(PointerEventData eventData)
    {
        if(thisItem != null){
            // Debug.Log("Exit " + thisItem.itemName + " slot");
            tooltip.HideTooltip();
            tooltip.UpdateTooltip("");
        }
    }

    // Xử lý click item của trang bị
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left && eventData.clickCount == 2){
            GetThisItemEquipment();
            if(thisItem != null){
                GameController.instance.AddItemToEquipment(thisItem);
                if(thisItem.GetType().ToString() == "Armor"){
                    GameController.instance.RemoveArmor();
                }
                
                if(thisItem.GetType().ToString() == "Weapon"){
                    GameController.instance.RemoveWeapon();
                }
            }
        }
        
    }
}
