using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [SerializeField] private Transform gridParent;
    [SerializeField] private Transform hand;
    [SerializeField] private HungerSystem hungerSystem;
    [SerializeField] private Inventory inventory;
    [SerializeField] private CraftingSystem craftingSystem;
    [SerializeField] private AttackZone triggerZone;
    [SerializeField] private PickUpSystem pickUpSystem;

    private Dictionary<InventoryCell, Item> _hotbarData;
    private List<HotbarCellData> _hotbarCells;
    private InventoryCell _currentActiveCell;

    void Start()
    {
        Init();
    }

    private void Update()
    {
        ChooseCell();
        if (_hotbarData[_currentActiveCell]?.GetType() == typeof(Food))
        {
            UseFood();
        }
        if (_hotbarData[_currentActiveCell]?.GetType() == typeof(Equipment))
        {
            UseEquipment();
        }
    }

    private void Init()
    {
        _hotbarData = new Dictionary<InventoryCell, Item>();
        _hotbarCells = new List<HotbarCellData>();
        for (int i = 0; i < gridParent.childCount; i++)
        {
            InventoryCell currentCell = gridParent.GetChild(i).GetChild(0).GetComponent<InventoryCell>();

            _hotbarData.Add(currentCell, null);
            _hotbarCells.Add( new HotbarCellData(gridParent.GetChild(i).GetComponent<Outline>(),currentCell));
            currentCell.IsEmpty = true;

            if (_currentActiveCell == null)
            {
                _currentActiveCell = currentCell;
                _hotbarCells[0].CellOutline.enabled = true;
            }
        }
    }

    private void UseFood()
    {
        if (Input.GetMouseButtonDown(0) && !inventory.IsInvetoryOpen && !craftingSystem.IsCrafting)
        {
            if (!hungerSystem.IsFullHunger)
            {
                hungerSystem.ChangeValue(_hotbarData[_currentActiveCell].ToFood().Saturation);
                _hotbarData[_currentActiveCell].ToFood().Amount--;
                if (_hotbarData[_currentActiveCell].ToFood().Amount != 0)
                    _currentActiveCell.SetItemCount(_hotbarData[_currentActiveCell].ToFood().Amount.ToString());
                else
                {
                    _currentActiveCell.SetItemCount("");
                    _currentActiveCell.SetSprite(null);
                    _hotbarData[_currentActiveCell] = null;
                }

                UpdateInventory();
            }
        }
    }

    public void GetResource(Item resource)
    {
        pickUpSystem.PickUp(resource);
    }

    private void UseEquipment()
    {
        if (Input.GetMouseButtonDown(0) && !inventory.IsInvetoryOpen && !craftingSystem.IsCrafting)
        {
            triggerZone.gameObject.SetActive(true);
            triggerZone.SetDamage(_hotbarData[_currentActiveCell].ToEquipment().Damage);
            triggerZone.SetLayerMask(_hotbarData[_currentActiveCell].ToEquipment().LayerMask);
            triggerZone.SetHotbar(this);
            StartCoroutine(WaitAHalfSecond());
        }
    }

    private IEnumerator WaitAHalfSecond()
    {
        yield return new WaitForSeconds(0.5f);
        triggerZone.gameObject.SetActive(false);
    }

    public void UpdateInventory()
    {
        int counter = 0;
        foreach (InventoryCell cell in _hotbarData.Keys)
        {
            if (cell == _currentActiveCell)
                break;
            counter++;
        }
        inventory.UpdateInventory(counter,_hotbarData[_currentActiveCell]);
    }

    public void UpdateUi(List<Item> items)
    {
        int count = 0;
        Dictionary<InventoryCell, Item> newHotbarData = new Dictionary<InventoryCell, Item>();
        foreach (InventoryCell cell in _hotbarData.Keys)
        {
            newHotbarData.Add(cell, items[count]);
            if (items[count] == null)
            {
                cell.SetItemCount("");
                cell.SetSprite(null);
            }
            else
            {
                cell.SetSprite(items[count].ItemIcon);
                if(items[count].GetType() == typeof(Items.Resource))
                    cell.SetItemCount(items[count].ToResource().Amount.ToString());
                else if (items[count].GetType() == typeof(Food))
                    cell.SetItemCount(items[count].ToFood().Amount.ToString());
            }
            count++;
        }
        _hotbarData = newHotbarData;
        ShowItem();
    }

    public void ChooseCell()
    {
        foreach (KeyCode key in KeyboardInputHelper.GetCurrentKeys())
        {
            switch (key)
            {
                case KeyCode.Alpha1:
                    ActivateCell(0);
                    return;
                case KeyCode.Alpha2:
                    ActivateCell(1);
                    return;
                case KeyCode.Alpha3:
                    ActivateCell(2);
                    return;
                case KeyCode.Alpha4:
                    ActivateCell(3);
                    return;
                case KeyCode.Alpha5:
                    ActivateCell(4);
                    return;
                case KeyCode.Alpha6:
                    ActivateCell(5);
                    return;
                case KeyCode.Alpha7:
                    ActivateCell(6);
                    return;
                case KeyCode.Alpha8:
                    ActivateCell(7);
                    return;
                case KeyCode.Alpha9:
                    ActivateCell(8);
                    return;
                default:
                    break;
            }
        }
    }

    private void ShowItem()
    {
        if(hand.childCount > 0)
        {
            for (int i = 0; i < hand.childCount; i++)
            {
                hand.GetChild(i).gameObject.SetActive(false);
            }
        }

        if (_hotbarData[_currentActiveCell]?.GetType() == typeof(Equipment))
        {
            _hotbarData[_currentActiveCell].gameObject.SetActive(true);
        } else if(_hotbarData[_currentActiveCell]?.GetType() == typeof(Food))
        {
            _hotbarData[_currentActiveCell].gameObject.SetActive(true);
        }
    }

    private void ActivateCell(int cellIndex)
    {
        foreach (HotbarCellData hotbarCell in _hotbarCells)
            hotbarCell.CellOutline.enabled = false;

        _hotbarCells[cellIndex].CellOutline.enabled = true;
        _currentActiveCell = _hotbarCells[cellIndex].InventoryCell;
        ShowItem();
    }
}