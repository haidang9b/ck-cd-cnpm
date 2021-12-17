using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    private Item itemData;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = transform.position.x;
        float y =transform.position.y;
        Rect bounds = new Rect(x-50, y-50,100,100); // kiem tra click trong vung can click
        if(Input.GetMouseButtonDown(0) && bounds.Contains(Input.mousePosition)){
            Debug.Log("click at equipment name = "+ gameObject.name );
        }
    }
    public void setItem(Weapon weapon){
        this.itemData = weapon;
    }
    public Item getItem(){
        return this.itemData;
    }
}
