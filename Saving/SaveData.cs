using NUnit.Framework;
using System.Globalization;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public Vector3 savedPosition; //posição do jogador quando ele salvou
    public string savedScene; //cena onde o jogo foi salvo

    public List<ObjectInteractionData> residentialData; //guarda todos os objetos que já realizaram uma ação
    public List<ObjectInteractionData> comercialData;
    public List<ObjectInteractionData> parkData;
    public List<ObjectInteractionData> policeData;


    public List<InventoryData> savedInventory = new();
}

[System.Serializable]
public class ObjectInteractionData
{
    public string objectId; //id criado no GlobalUtility
    public bool wasUsed;
}

[System.Serializable]
public class InventoryData
{
    public int itemId; // id na lista em DefaultItems
    public int itemAmount;
}

