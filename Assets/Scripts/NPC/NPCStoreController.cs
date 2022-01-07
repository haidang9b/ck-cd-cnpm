using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCStoreController : MonoBehaviour, Interactable
{
    private List<Item> items; // list item đang bán
    public GameObject prefab;
    public GameObject itemContainer;
    public GameObject panelMain;
    public GameObject panelNPCStore;
    float widthContent, heightContent;
    void Start(){
        // StartCoroutine(GetItems());
        items = GameController.instance.itemsInGame;
        CloseDialogPanel();
        LoadItemStore();
        RectTransform rt = (RectTransform)itemContainer.transform;
        widthContent = rt.rect.width;
        heightContent = rt.rect.height;
    }

    // get item từ database, sau đó so sánh vs item đang có
    // private IEnumerator GetItems(){

    // }
    public void Interact()
    {
        panelNPCStore.SetActive(true);
        Debug.Log("Interacting with NPC store");
    }

    public void CloseDialogPanel(){
        panelNPCStore.SetActive(false);
    }

    public void OpenStore(){
        CloseDialogPanel();
        panelMain.SetActive(true);
    }

    public void CloseStore(){
        panelMain.SetActive(false);
    }

    private void LoadItemStore(){ // load tất cả các item cần có trong store
        GameObject newItem;
        for(int i=0; i< items.Count; i++){
            newItem = (GameObject)Instantiate(prefab, itemContainer.transform); // clone ra bằng prefab
            newItem.transform.GetChild(0).GetComponent<Image>().sprite = items[i].itemSpite;
            newItem.transform.GetChild(1).GetComponent<Text>().text = items[i].itemName;
            newItem.transform.GetChild(2).GetComponent<Text>().text = "Price : " +items[i].price + "$";
            newItem.GetComponent<ItemStoreBehaviour>().thisItem = items[i];
            newItem.GetComponent<ItemStoreBehaviour>().IDButton = i;
            if(i % 2 == 0){
                heightContent += 190f; // vì padding 30 +  item cao 160
                itemContainer.GetComponent<RectTransform>().sizeDelta = new Vector2 (widthContent, heightContent);  
            }
        }
    }
}
