using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
    [SerializeField] private Transform gridParent;

    private Dictionary<InventoryCell, Item> hotbarData;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        hotbarData = new Dictionary<InventoryCell, Item>();
        for (int i = 0; i < gridParent.childCount; i++)
        {
            InventoryCell currentCell = gridParent.GetChild(i).GetComponent<InventoryCell>();
            hotbarData.Add(currentCell, null);
            currentCell.IsEmpty = true;
        }
    }

    public void UpdateUi(List<Item> items)
    {
        int count = 0;
        Dictionary<InventoryCell, Item> newHotbarData = new Dictionary<InventoryCell, Item>();
        foreach (InventoryCell cell in hotbarData.Keys)
        {
            newHotbarData.Add(cell, items[count]);
            if (items[count] == null)
            {
                cell.SetItemCount("");
                cell.SetSprite(null);
            }
            else
            {
                cell.SetItemCount(items[count].Amount.ToString());
                cell.SetSprite(items[count].Data.ItemIcon);
            }
            count++;
        }
        hotbarData = newHotbarData;
    }
}
