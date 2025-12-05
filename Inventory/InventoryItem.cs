using System.Globalization;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Scriptable Objects/InventoryItem")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public int itemAmount;
    public bool canStack;
    public Sprite itemIcon; //ícone da interface
    public int id; //usado para identificar no item na lista 
}
