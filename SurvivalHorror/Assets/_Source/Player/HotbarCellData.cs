using UnityEngine.UI;

public class HotbarCellData
{
    public Outline CellOutline { get; set; }
    public InventoryCell InventoryCell { get; set; }

    public HotbarCellData(Outline cellOutline, InventoryCell inventoryCell)
    {
        CellOutline = cellOutline;
        InventoryCell = inventoryCell;
    }
}