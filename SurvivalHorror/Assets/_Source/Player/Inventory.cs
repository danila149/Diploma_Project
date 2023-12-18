using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public const int MAX_STACK = 99;

    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject hudUI;
    [SerializeField] private Transform dropPos;
    [SerializeField] private Hotbar hotbar;

    private Dictionary<InventoryCell, Item> inventoryData;

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventoryUI.activeInHierarchy)
            {
                inventoryUI.SetActive(false);
                hudUI.SetActive(true);
                HotbarUpdateUI();
                PlayerMovement.Instance.PlayerInput = true;
            }
            else
            {
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
            if (inventoryData[cell]?.Data.Type == newItem.Data.Type && inventoryData[cell]?.Amount < MAX_STACK)
            {
                if(inventoryData[cell]?.Amount + newItem.Amount > MAX_STACK)
                {
                    Item currenItem = inventoryData[cell];
                    newItem.Amount = newItem.Amount-(MAX_STACK - currenItem.Amount);
                    currenItem.Amount += MAX_STACK - currenItem.Amount;
                    SetItemToCell(cell, currenItem);
                    HotbarUpdateUI();
                    if (!AddItem(newItem))
                        GetComponent<PhotonView>().RPC("DropItem", RpcTarget.AllBuffered, newItem.Data.Type.ToString(), newItem.Amount);

                    return true;
                }
                else
                {
                    Item currenItem = inventoryData[cell];
                    currenItem.Amount += newItem.Amount;
                    SetItemToCell(cell, currenItem);
                    HotbarUpdateUI();
                    return true;
                }
            } else if (cell.IsEmpty)
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
        if(inventoryData[origin].Data.Type == inventoryData[newCell]?.Data.Type)
        {
            if (inventoryData[newCell]?.Amount == MAX_STACK)
                return false;

            if(inventoryData[origin].Amount + inventoryData[newCell].Amount > MAX_STACK)
            {
                inventoryData[origin].Amount -= MAX_STACK - inventoryData[newCell].Amount;
                origin.SetItemCount(inventoryData[origin].Amount.ToString());
                inventoryData[newCell].Amount = MAX_STACK;
                newCell.SetItemCount(MAX_STACK.ToString());
                return true;
            }else if(inventoryData[origin].Amount + inventoryData[newCell].Amount <= MAX_STACK)
            {
                inventoryData[newCell].Amount += inventoryData[origin].Amount;
                newCell.SetItemCount(inventoryData[newCell].Amount.ToString());
                SetItemToCell(origin, null);
                return true;
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
        DropItem(item.Data.Type.ToString(), item.Amount);
    }
    private void DropItem(string itemName, int itemAmount)
    {
        Item droppedItem = PhotonNetwork.Instantiate(itemName, dropPos.position, Quaternion.identity).GetComponent<Item>();
        droppedItem.Amount = itemAmount;
    }

    public bool SearchItemBy(ItemType itemType, int amount)
    {
        int _counter = 0;
        foreach (Item item in inventoryData.Values)
        {
            if (item != null)
            {
                if(item.Data.Type == itemType)
                {
                    if (item.Amount >= amount)
                        return true;

                    _counter += item.Amount;
                    if (_counter >= amount)
                        return true;
                }
            }
        }
        return false;
    }
    public void UseItemBy(ItemType itemType, int amount)
    {
        if(SearchItemBy(itemType, amount))
        {
            int _counter = 0;
            List<InventoryCell> cellsInUse = new List<InventoryCell>();
            foreach (InventoryCell cell in inventoryData.Keys)
            {
                if (inventoryData[cell] != null)
                {
                    if (inventoryData[cell].Data.Type == itemType)
                    {
                        if (inventoryData[cell].Amount >= amount)
                        {
                            inventoryData[cell].Amount -= amount;
                            if(inventoryData[cell].Amount == 0)
                            {
                                SetItemToCell(cell, null);
                                return;
                            }

                            cell.SetItemCount(inventoryData[cell].Amount.ToString());
                            return;
                        }

                        _counter += inventoryData[cell].Amount;
                        cellsInUse.Add(cell);
                        if (_counter >= amount)
                        {
                            int _currentAmountNeed = amount;
                            foreach (InventoryCell cellInUse in cellsInUse)
                            {
                                if(inventoryData[cellInUse].Amount < _currentAmountNeed)
                                {
                                    _currentAmountNeed -= inventoryData[cellInUse].Amount;
                                    SetItemToCell(cellInUse, null);
                                } else if(inventoryData[cellInUse].Amount >= _currentAmountNeed)
                                {
                                    inventoryData[cellInUse].Amount -= _currentAmountNeed;
                                    if(inventoryData[cellInUse].Amount == 0)
                                    {
                                        SetItemToCell(cellInUse, null);
                                        return;
                                    }
                                    cellInUse.SetItemCount(inventoryData[cellInUse].Amount.ToString());
                                    return;
                                }
                            }
                        }
                    }
                }
            }
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
        }
        inventoryData[cell] = item;
        cell.SetItemCount(item.Amount.ToString());
        cell.SetSprite(item.Data.ItemIcon);
        cell.IsEmpty = false;
    }
}
