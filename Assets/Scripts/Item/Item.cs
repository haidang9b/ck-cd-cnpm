using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemSpite;
    public int price;
}