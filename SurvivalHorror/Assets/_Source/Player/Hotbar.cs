using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [SerializeField] private Transform gridParent;

    private Dictionary<InventoryCell, Item> hotbarData;
    private List<HotbarCellData> hotbarCells;
    private InventoryCell _currentActiveCell;

    void Start()
    {
        Init();
    }

    private void Update()
    {
        ChooseCell();
    }

    private void Init()
    {
        hotbarData = new Dictionary<InventoryCell, Item>();
        hotbarCells = new List<HotbarCellData>();
        for (int i = 0; i < gridParent.childCount; i++)
        {
            InventoryCell currentCell = gridParent.GetChild(i).GetChild(0).GetComponent<InventoryCell>();

            hotbarData.Add(currentCell, null);
            hotbarCells.Add( new HotbarCellData(gridParent.GetChild(i).GetComponent<Outline>(),currentCell));
            currentCell.IsEmpty = true;

            if (_currentActiveCell == null)
            {
                _currentActiveCell = currentCell;
                hotbarCells[0].CellOutline.enabled = true;
            }
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

    private void ActivateCell(int cellIndex)
    {
        foreach (HotbarCellData hotbarCell in hotbarCells)
            hotbarCell.CellOutline.enabled = false;

        hotbarCells[cellIndex].CellOutline.enabled = true;
        _currentActiveCell = hotbarCells[cellIndex].InventoryCell;
    }
}