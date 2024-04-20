using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public const int MAX_STACK = 99;
    public const int MAX_FOOD_STACK = 20;

    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject hudUI;
    [SerializeField] private Transform dropPos;
    [SerializeField] private Hotbar hotbar;
    [SerializeField] private CraftingSystem craftingSystem;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PickUpSystem pickUpSystem;

    public bool IsInvetoryOpen { get; set; }
    private Dictionary<InventoryCell, Item> _inventoryData;

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsInvetoryOpen = false;
            inventoryUI.SetActive(false);
            hudUI.SetActive(true);
            HotbarUpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.Tab) && !craftingSystem.IsCrafting)
        {
            if (inventoryUI.activeInHierarchy)
            {
                IsInvetoryOpen = false;
                inventoryUI.SetActive(false);
                hudUI.SetActive(true);
                HotbarUpdateUI();
                playerMovement.PlayerInput = true;
            }
            else
            {
                IsInvetoryOpen = true;
                playerMovement.PlayerInput = false;
                hudUI.SetActive(false);
                inventoryUI.SetActive(true);
            }
        }
    }

    private void Init()
    {
        _inventoryData = new Dictionary<InventoryCell, Item>();
        for (int i = 0; i < gridParent.childCount; i++)
        {
            InventoryCell currentCell = gridParent.GetChild(i).GetComponent<InventoryCell>();
            _inventoryData.Add(currentCell, null);
            currentCell.SetItemCount("");
            currentCell.SetSprite(null);
            currentCell.IsEmpty = true;
            currentCell.Inventory = this;
        }
    }

    public void UpdateInventory(int cellNum, Item item)
    {
        int counter = 0;
        foreach (InventoryCell cell in _inventoryData.Keys)
        {
            if (cell.Hotbar)
            {
                if (counter == cellNum)
                {
                    SetItemToCell(cell, item);
                    return;
                }
                counter++;
            }
        }
    }

    public void HotbarUpdateUI()
    {
        List<Item> items = new List<Item>();
        foreach (InventoryCell cell in _inventoryData.Keys)
        {
            if (cell.Hotbar)
            {
                items.Add(_inventoryData[cell]);
            }
        }
        hotbar.UpdateUi(items);
    }

    public bool AddItem(Item newItem)
    {
        if (newItem.GetType() == typeof(Equipment))
            if (newItem.ToEquipment().IsEquiped)
                return false;

        foreach (InventoryCell cell in _inventoryData.Keys)
        {
            if(_inventoryData[cell]?.GetType() != typeof(Equipment))
            {
                if (newItem.GetType()== typeof(Items.Resource))
                {
                    Items.Resource currentNewItem = newItem.ToResource();
                    if(_inventoryData[cell]?.GetType() != typeof(Food))
                    {
                        if (_inventoryData[cell]?.ToResource().ResourceType == currentNewItem.ResourceType && _inventoryData[cell]?.ToResource().Amount < MAX_STACK)
                        {
                            if (_inventoryData[cell].ToResource()?.Amount + currentNewItem.Amount > MAX_STACK)
                            {
                                Items.Resource currenItem = _inventoryData[cell].ToResource();
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
                                Items.Resource currenItem = _inventoryData[cell].ToResource();
                                currenItem.Amount += currentNewItem.Amount;
                                SetItemToCell(cell, currenItem);
                                HotbarUpdateUI();
                                return true;
                            }
                        }
                    }
                }
                else if(newItem.GetType()== typeof(Food))
                {
                    Food currentNewItem = newItem.ToFood();
                    if (_inventoryData[cell]?.GetType() != typeof(Items.Resource))
                    {
                        if (_inventoryData[cell]?.ToFood().FoodType == currentNewItem.FoodType && _inventoryData[cell]?.ToFood().Amount < MAX_FOOD_STACK)
                        {
                            if (_inventoryData[cell].ToFood()?.Amount + currentNewItem.Amount > MAX_FOOD_STACK)
                            {
                                Food currenItem = _inventoryData[cell].ToFood();
                                currentNewItem.Amount = currentNewItem.Amount - (MAX_FOOD_STACK - currenItem.Amount);
                                currenItem.Amount += MAX_FOOD_STACK - currenItem.Amount;
                                SetItemToCell(cell, currenItem);
                                HotbarUpdateUI();
                                if (!AddItem(newItem))
                                    DropItem(currentNewItem);

                                return true;
                            }
                            else
                            {
                                Food currenItem = _inventoryData[cell].ToFood();
                                currenItem.Amount += currentNewItem.Amount;
                                SetItemToCell(cell, currenItem);
                                HotbarUpdateUI();
                                return true;
                            }
                        }
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

        if(_inventoryData[origin].GetType()==typeof(Items.Resource) && _inventoryData[newCell].GetType() == typeof(Items.Resource))
        {
            if (_inventoryData[origin].ToResource().ResourceType == _inventoryData[newCell]?.ToResource().ResourceType)
            {
                if (_inventoryData[newCell]?.ToResource().Amount == MAX_STACK)
                    return false;

                if (_inventoryData[origin].ToResource().Amount + _inventoryData[newCell].ToResource().Amount > MAX_STACK)
                {
                    _inventoryData[origin].ToResource().Amount -= MAX_STACK - _inventoryData[newCell].ToResource().Amount;
                    origin.SetItemCount(_inventoryData[origin].ToResource().Amount.ToString());
                    _inventoryData[newCell].ToResource().Amount = MAX_STACK;
                    newCell.SetItemCount(MAX_STACK.ToString());
                    return true;
                }
                else if (_inventoryData[origin].ToResource().Amount + _inventoryData[newCell].ToResource().Amount <= MAX_STACK)
                {
                    _inventoryData[newCell].ToResource().Amount += _inventoryData[origin].ToResource().Amount;
                    newCell.SetItemCount(_inventoryData[newCell].ToResource().Amount.ToString());
                    SetItemToCell(origin, null);
                    return true;
                }
            }
        }

        if (_inventoryData[origin].GetType() == typeof(Food) && _inventoryData[newCell].GetType() == typeof(Food))
        {
            if (_inventoryData[origin].ToFood().FoodType == _inventoryData[newCell]?.ToFood().FoodType)
            {
                if (_inventoryData[newCell]?.ToFood().Amount == MAX_FOOD_STACK)
                    return false;

                if (_inventoryData[origin].ToFood().Amount + _inventoryData[newCell].ToFood().Amount > MAX_FOOD_STACK)
                {
                    _inventoryData[origin].ToFood().Amount -= MAX_FOOD_STACK - _inventoryData[newCell].ToFood().Amount;
                    origin.SetItemCount(_inventoryData[origin].ToFood().Amount.ToString());
                    _inventoryData[newCell].ToFood().Amount = MAX_FOOD_STACK;
                    newCell.SetItemCount(MAX_FOOD_STACK.ToString());
                    return true;
                }
                else if (_inventoryData[origin].ToFood().Amount + _inventoryData[newCell].ToFood().Amount <= MAX_FOOD_STACK)
                {
                    _inventoryData[newCell].ToFood().Amount += _inventoryData[origin].ToFood().Amount;
                    newCell.SetItemCount(_inventoryData[newCell].ToFood().Amount.ToString());
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
            Item item = _inventoryData[origin];
            SetItemToCell(newCell, item);
            SetItemToCell(origin, null);
        }
    }

    public void RemoveItem(InventoryCell cell)
    {
        Item item = _inventoryData[cell];
        SetItemToCell(cell, null);
        DropItem(item);
    }

    private void DropItem(Item item)
    {
        if (item.GetType() == typeof(Items.Resource))
        {
            Items.Resource droppedItem = PhotonNetwork.Instantiate(item.ToResource().ResourceType.ToString(), dropPos.position, Quaternion.identity).GetComponent<Items.Resource>();
            droppedItem.Amount = item.ToResource().Amount;
        }
        else if (item.GetType() == typeof(Food))
        {
            Food droppedItem = PhotonNetwork.Instantiate(item.ToFood().FoodType.ToString(), dropPos.position, Quaternion.identity).GetComponent<Food>();
            droppedItem.Amount = item.ToFood().Amount;
        }
        else if (item.GetType() == typeof(Equipment))
            PhotonNetwork.Instantiate(item.ToEquipment().EquipmentType.ToString(), dropPos.position, Quaternion.identity);
    }   

    public void DropAll()
    {
        foreach (InventoryCell cell in _inventoryData.Keys)
        {
            if (_inventoryData[cell] != null)
                DropItem(_inventoryData[cell]);
        }

        Init();
        HotbarUpdateUI();
    }

    public bool SearchItemBy(ResourceType resourceType, int amount)
    {
        int _counter = 0;
        foreach (Item item in _inventoryData.Values)
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
            foreach (InventoryCell cell in _inventoryData.Keys)
            {
                if (_inventoryData[cell] != null && _inventoryData[cell].GetType()==typeof(Items.Resource))
                {
                    if (_inventoryData[cell].ToResource().ResourceType == resourceType)
                    {
                        if (_inventoryData[cell].ToResource().Amount >= amount)
                        {
                            _inventoryData[cell].ToResource().Amount -= amount;
                            if(_inventoryData[cell].ToResource().Amount == 0)
                            {
                                SetItemToCell(cell, null);
                                return;
                            }

                            cell.SetItemCount(_inventoryData[cell].ToResource().Amount.ToString());
                            return;
                        }

                        _counter += _inventoryData[cell].ToResource().Amount;
                        cellsInUse.Add(cell);
                        if (_counter >= amount)
                        {
                            int _currentAmountNeed = amount;
                            foreach (InventoryCell cellInUse in cellsInUse)
                            {
                                if(_inventoryData[cellInUse].ToResource().Amount < _currentAmountNeed)
                                {
                                    _currentAmountNeed -= _inventoryData[cellInUse].ToResource().Amount;
                                    SetItemToCell(cellInUse, null);
                                } else if(_inventoryData[cellInUse].ToResource().Amount >= _currentAmountNeed)
                                {
                                    _inventoryData[cellInUse].ToResource().Amount -= _currentAmountNeed;
                                    if(_inventoryData[cellInUse].ToResource().Amount == 0)
                                    {
                                        SetItemToCell(cellInUse, null);
                                        return;
                                    }
                                    cellInUse.SetItemCount(_inventoryData[cellInUse].ToResource().Amount.ToString());
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

    public void Split(InventoryCell cell, int amount)
    {
        if (_inventoryData[cell]?.GetType() != typeof(Equipment))
        {
            if(_inventoryData[cell].GetType() == typeof(Items.Resource))
            {
                if (_inventoryData[cell].ToResource().Amount == amount)
                    RemoveItem(cell);
                else
                {
                    _inventoryData[cell].ToResource().Amount -= amount;
                    SetItemToCell(cell, _inventoryData[cell]);
                    Items.Resource splitedItem = PhotonNetwork.Instantiate(_inventoryData[cell].ToResource().ResourceType.ToString(), dropPos.position, Quaternion.identity).GetComponent<Items.Resource>();
                    splitedItem.Amount = amount;
                    pickUpSystem.RemoveItem(splitedItem.gameObject.transform.position);
                    foreach (InventoryCell invCell in _inventoryData.Keys)
                    {
                        if (invCell.IsEmpty)
                        {
                            SetItemToCell(invCell, splitedItem);
                            return;
                        }
                    }
                    DropItem(splitedItem);
                }
            } else if (_inventoryData[cell].GetType() == typeof(Food))
            {
                if (_inventoryData[cell].ToFood().Amount == amount)
                    RemoveItem(cell);
                else
                {
                    _inventoryData[cell].ToFood().Amount -= amount;
                    SetItemToCell(cell, _inventoryData[cell]);
                    Food splitedItem = PhotonNetwork.Instantiate(_inventoryData[cell].ToFood().FoodType.ToString(), dropPos.position, Quaternion.identity).GetComponent<Food>();
                    splitedItem.Amount = amount;
                    pickUpSystem.RemoveItem(splitedItem.gameObject.transform.position);
                    foreach (InventoryCell invCell in _inventoryData.Keys)
                    {
                        if (invCell.IsEmpty)
                        {
                            SetItemToCell(invCell, splitedItem);
                            return;
                        }
                    }
                    DropItem(splitedItem);
                }
            }
        }
    }

    private void SetItemToCell(InventoryCell cell, Item item)
    {
        if (item == null)
        {
            _inventoryData[cell] = null;
            cell.SetItemCount("");
            cell.SetSprite(null);
            cell.IsEmpty = true;
            return;
        }
        else if (item.GetType() == typeof(Items.Resource))
        {
            _inventoryData[cell] = item;
            Items.Resource currentItem = item.ToResource();
            cell.SetItemCount(currentItem.Amount.ToString());
            cell.SetSprite(currentItem.ItemIcon);
            cell.IsEmpty = false;
        }
        else if (item.GetType() == typeof(Food))
        {
            _inventoryData[cell] = item;
            Food currentItem = item.ToFood();
            cell.SetItemCount(currentItem.Amount.ToString());
            cell.SetSprite(currentItem.ItemIcon);
            cell.IsEmpty = false;
        }
        else if (item.GetType() == typeof(Equipment))
        {
            _inventoryData[cell] = item;
            cell.SetSprite(item.ItemIcon);
            cell.IsEmpty = false;
        }
    }

    public Item GetItemByCell(InventoryCell cell) =>
        _inventoryData[cell];
}
