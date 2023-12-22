using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using Items;

public class Inventory : MonoBehaviour
{
    public const int MAX_STACK = 99;

    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject hudUI;
    [SerializeField] private Transform dropPos;
    [SerializeField] private Hotbar hotbar;
    [SerializeField] private CraftingSystem craftingSystem;

    public bool IsCrafting { get; set; }
    private Dictionary<InventoryCell, Item> inventoryData;

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !IsCrafting)
        {
            if (inventoryUI.activeInHierarchy)
            {
                craftingSystem.IsInventoryOpen = false;
                inventoryUI.SetActive(false);
                hudUI.SetActive(true);
                HotbarUpdateUI();
                PlayerMovement.Instance.PlayerInput = true;
            }
            else
            {
                craftingSystem.IsInventoryOpen = true;
                PlayerMovement.Instance.PlayerInput = false;
                hudUI.SetActive(false);
                inventoryUI.SetActive(true);
            }
        }
    }

    private void Init()
    {
        inventoryData = new Dictionary<InventoryCell, Item>();
        for (int i = 0; i < gridParent.childCount; i++)
        {
            InventoryCell currentCell = gridParent.GetChild(i).GetComponent<InventoryCell>();
            inventoryData.Add(currentCell, null);
            currentCell.IsEmpty = true;
            currentCell.Inventory = this;
        }
    }

    public void HotbarUpdateUI()
    {
        List<Item> items = new List<Item>();
        foreach (InventoryCell cell in inventoryData.Keys)
        {
            if (cell.Hotbar)
            {
                items.Add(inventoryData[cell]);
            }
        }
        hotbar.UpdateUi(items);
    }

    public bool AddItem(Item newItem)
    {
        foreach (InventoryCell cell in inventoryData.Keys)
        {
            if(newItem.GetType() == typeof(Items.Resource))
            {
                Items.Resource currentNewItem = newItem.ToResource();
                if (inventoryData[cell]?.ToResource().ResourceType == currentNewItem.ResourceType && inventoryData[cell]?.ToResource().Amount < MAX_STACK)
                {
                    if (inventoryData[cell].ToResource()?.Amount + currentNewItem.Amount > MAX_STACK)
                    {
                        Items.Resource currenItem = inventoryData[cell].ToResource();
                        currentNewItem.Amount = currentNewItem.Amount - (MAX_STACK - currenItem.Amount);
                        currenItem.Amount += MAX_STACK - currenItem.Amount;
                        SetItemToCell(cell, currenItem);
                        HotbarUpdateUI();
                        if (!AddItem(newItem))
                            DropItem(currentNewItem);

                        return true;
                    }
                    else
                    {
                        Items.Resource currenItem = inventoryData[cell].ToResource();
                        currenItem.Amount += currentNewItem.Amount;
                        SetItemToCell(cell, currenItem);
                        HotbarUpdateUI();
                        return true;
                    }
                }
            } 
            if (cell.IsEmpty)
            {
                SetItemToCell(cell, newItem);
                HotbarUpdateUI();
                return true;
            }
        }
        return false;
    }

    public bool AddItemToStack(InventoryCell origin, InventoryCell newCell)
    {
        if (newCell.IsEmpty)
            return false;

        if(inventoryData[origin].GetType()==typeof(Items.Resource) && inventoryData[newCell].GetType() == typeof(Items.Resource))
        {
            if (inventoryData[origin].ToResource().ResourceType == inventoryData[newCell]?.ToResource().ResourceType)
            {
                if (inventoryData[newCell]?.ToResource().Amount == MAX_STACK)
                    return false;

                if (inventoryData[origin].ToResource().Amount + inventoryData[newCell].ToResource().Amount > MAX_STACK)
                {
                    inventoryData[origin].ToResource().Amount -= MAX_STACK - inventoryData[newCell].ToResource().Amount;
                    origin.SetItemCount(inventoryData[origin].ToResource().Amount.ToString());
                    inventoryData[newCell].ToResource().Amount = MAX_STACK;
                    newCell.SetItemCount(MAX_STACK.ToString());
                    return true;
                }
                else if (inventoryData[origin].ToResource().Amount + inventoryData[newCell].ToResource().Amount <= MAX_STACK)
                {
                    inventoryData[newCell].ToResource().Amount += inventoryData[origin].ToResource().Amount;
                    newCell.SetItemCount(inventoryData[newCell].ToResource().Amount.ToString());
                    SetItemToCell(origin, null);
                    return true;
                }
            }
        }
        return false;
    }

    public void SwitchItemCell(InventoryCell origin, InventoryCell newCell)
    {
        if (newCell.IsEmpty)
        {
            Item item = inventoryData[origin];
            SetItemToCell(newCell, item);
            SetItemToCell(origin, null);
        }
    }

    public void RemoveItem(InventoryCell cell)
    {
        Item item = inventoryData[cell];
        SetItemToCell(cell, null);
        if(item.GetType() == typeof(Resource))
            DropItem(item.ToResource());
        else if(item.GetType() == typeof(Equipment))
            DropItem(item.ToEquipment());
    }
    private void DropItem(Items.Resource resource)
    {
        Items.Resource droppedItem = PhotonNetwork.Instantiate(resource.ResourceType.ToString(), dropPos.position, Quaternion.identity).GetComponent<Items.Resource>();
        droppedItem.Amount = resource.Amount;
    }

    private void DropItem(Equipment equipment)=>
        PhotonNetwork.Instantiate(equipment.EquipmentType.ToString(), dropPos.position, Quaternion.identity);

    public bool SearchItemBy(ResourceType resourceType, int amount)
    {
        int _counter = 0;
        foreach (Item item in inventoryData.Values)
        {
            if (item != null && item.GetType()==typeof(Items.Resource))
            {
                Items.Resource currentItem = item.ToResource();
                if(currentItem.ResourceType == resourceType)
                {
                    if (currentItem.Amount >= amount)
                        return true;

                    _counter += currentItem.Amount;
                    if (_counter >= amount)
                        return true;
                }
            }
        }
        return false;
    }
    public void UseItemBy(ResourceType resourceType, int amount)
    {
        if(SearchItemBy(resourceType, amount))
        {
            int _counter = 0;
            List<InventoryCell> cellsInUse = new List<InventoryCell>();
            foreach (InventoryCell cell in inventoryData.Keys)
            {
                if (inventoryData[cell] != null && inventoryData[cell].GetType()==typeof(Items.Resource))
                {
                    if (inventoryData[cell].ToResource().ResourceType == resourceType)
                    {
                        if (inventoryData[cell].ToResource().Amount >= amount)
                        {
                            inventoryData[cell].ToResource().Amount -= amount;
                            if(inventoryData[cell].ToResource().Amount == 0)
                            {
                                SetItemToCell(cell, null);
                                return;
                            }

                            cell.SetItemCount(inventoryData[cell].ToResource().Amount.ToString());
                            return;
                        }

                        _counter += inventoryData[cell].ToResource().Amount;
                        cellsInUse.Add(cell);
                        if (_counter >= amount)
                        {
                            int _currentAmountNeed = amount;
                            foreach (InventoryCell cellInUse in cellsInUse)
                            {
                                if(inventoryData[cellInUse].ToResource().Amount < _currentAmountNeed)
                                {
                                    _currentAmountNeed -= inventoryData[cellInUse].ToResource().Amount;
                                    SetItemToCell(cellInUse, null);
                                } else if(inventoryData[cellInUse].ToResource().Amount >= _currentAmountNeed)
                                {
                                    inventoryData[cellInUse].ToResource().Amount -= _currentAmountNeed;
                                    if(inventoryData[cellInUse].ToResource().Amount == 0)
                                    {
                                        SetItemToCell(cellInUse, null);
                                        return;
                                    }
                                    cellInUse.SetItemCount(inventoryData[cellInUse].ToResource().Amount.ToString());
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            Debug.Log("error");
        }
    }

    private void SetItemToCell(InventoryCell cell, Item item)
    {
        if(item == null)
        {
            inventoryData[cell] = null;
            cell.SetItemCount("");
            cell.SetSprite(null);
            cell.IsEmpty = true;
            return;
        } else if (item.GetType() == typeof(Items.Resource))
        {
            inventoryData[cell] = item;
            Items.Resource currentItem = item.ToResource();
            cell.SetItemCount(currentItem.Amount.ToString());
            cell.SetSprite(currentItem.ItemIcon);
            cell.IsEmpty = false;
        } else if (item.GetType() == typeof(Equipment))
        {
            inventoryData[cell] = item;
            cell.SetSprite(item.ItemIcon);
            cell.IsEmpty = false;
        }
        
    }
}
