using UnityEngine;

[CreateAssetMenu(fileName = "DefaultData", menuName = "Scriptable Objects/DefaultData")]
public class DefaultData : ScriptableObject
{
    public string startSceneName;
    public Vector3 startPosition;

    //lista de todos os itens
    public InventoryItem[] defaultItems;
}
