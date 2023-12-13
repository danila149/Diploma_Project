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
    [SerializeField] private PlayerMovement playerMovement;
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
                playerMovement.PlayerInput = true;
            }
            else
            {
                playerMovement.PlayerInput = false;
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
                    inventoryData[cell] = currenItem;
                    cell.SetItemCount(currenItem.Amount.ToString());
                    cell.SetSprite(currenItem.Data.ItemIcon);

                    cell.IsEmpty = false;
                    HotbarUpdateUI();
                    if (!AddItem(newItem))
                        GetComponent<PhotonView>().RPC("DropItem", RpcTarget.AllBuffered, newItem.Data.Type.ToString(), newItem.Amount);

                    return true;
                }
                else
                {
                    Item currenItem = inventoryData[cell];
                    currenItem.Amount += newItem.Amount;
                    inventoryData[cell] = currenItem;
                    cell.SetItemCount(currenItem.Amount.ToString());
                    cell.SetSprite(currenItem.Data.ItemIcon);
                    cell.IsEmpty = false;
                    HotbarUpdateUI();
                    return true;
                }
            } else if (cell.IsEmpty)
            {
                inventoryData[cell] = newItem;
                cell.SetItemCount(newItem.Amount.ToString());
                cell.SetSprite(newItem.Data.ItemIcon);
                cell.IsEmpty = false;
                HotbarUpdateUI();
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
            inventoryData[newCell] = item;
            newCell.SetItemCount(item.Amount.ToString());
            newCell.SetSprite(item.Data.ItemIcon);
            newCell.IsEmpty = false;

            inventoryData[origin] = null;
            origin.SetItemCount("");
            origin.SetSprite(null);
            origin.IsEmpty = true;
        }
    }

    public void RemoveItem(InventoryCell cell)
    {
        Item item = inventoryData[cell];
        cell.IsEmpty = true;
        cell.SetItemCount("");
        cell.SetSprite(null);
        inventoryData[cell] = null;
        DropItem(item.Data.Type.ToString(), item.Amount);
    }
    private void DropItem(string itemName, int itemAmount)
    {
        Item droppedItem = PhotonNetwork.Instantiate(itemName, dropPos.position, Quaternion.identity).GetComponent<Item>();
        droppedItem.Amount = itemAmount;
    }
}
