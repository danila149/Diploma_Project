using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Image cellImage;
    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private LayerMask dropZoneLayerMask;
    [SerializeField] private LayerMask cellLayerMask;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private bool hotbar;
    [SerializeField] private InventoryInfoPanel infoPanel;
    [SerializeField] private DropSplitWindow dropSplitWindow;

    private Vector3 _currentPosition;
    private int _dropZoneLayer;
    private int _cellLayer;
    private Canvas _canvas;
    private int sibIndex;

    public bool Hotbar => hotbar;
    public bool IsEmpty { get; set; }
    public Inventory Inventory { get; set; }

    private void Start()
    {
        _canvas = transform.parent.parent.parent.GetComponent<Canvas>();
        _currentPosition = transform.position;
        _dropZoneLayer = (int)Mathf.Log(dropZoneLayerMask.value, 2);
        _cellLayer = (int)Mathf.Log(cellLayerMask.value, 2);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsEmpty)
        {
            transform.position = _currentPosition;
            rectTransform.SetSiblingIndex(sibIndex);
            canvasGroup.blocksRaycasts = true;
            foreach (GameObject item in eventData.hovered)
            {
                if (item.layer == _dropZoneLayer)
                {
                    Inventory.RemoveItem(this);
                    return;
                } else if(item.layer == _cellLayer)
                {
                    if (gameObject != item)
                    {
                        if(!Inventory.AddItemToStack(this, item.GetComponent<InventoryCell>()))
                            Inventory.SwitchItemCell(this, item.GetComponent<InventoryCell>());
                        return;
                    }
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        sibIndex = rectTransform.GetSiblingIndex();
        rectTransform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!IsEmpty)
            rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void SetSprite(Sprite sprite) =>
        cellImage.sprite = sprite;

    public void SetItemCount(string count) =>
        itemCountText.text = count;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Item item = Inventory.GetItemByCell(this);
        if (item != null)
        {
            infoPanel.gameObject.SetActive(true);
            string description = "";
            string itemName = "";
            if (item.GetType() == typeof(Items.Resource))
            {
                description = $"Amount: {item.ToResource().Amount}";
                itemName = item.ToResource().ResourceType.ToString();
            }
            else if (item.GetType() == typeof(Equipment))
            {
                itemName = item.ToEquipment().EquipmentType.ToString();
                description += $"Durability: {item.ToEquipment().Durability} \nDamage: {item.ToEquipment().Damage}";
            }
            else if (item.GetType() == typeof(Food))
            {
                description += $"Amount: {item.ToFood().Amount} \nSaturation: {item.ToFood().Saturation}";
                itemName = item.ToFood().FoodType.ToString();
            }

            infoPanel.SetInfo(item.ItemIcon, itemName, description);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoPanel.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButton(1))
        {
            dropSplitWindow.DropBtn.onClick.AddListener(DropItem);
            dropSplitWindow.SplitBtn.onClick.AddListener(SplitItem);

            if (Inventory.GetItemByCell(this) == null)
                return;

            dropSplitWindow.gameObject.SetActive(true);
            dropSplitWindow.gameObject.transform.position = Input.mousePosition;

            if (Inventory.GetItemByCell(this).GetType() == typeof(Food))
                dropSplitWindow.SetSliderMaxValue(Inventory.GetItemByCell(this).ToFood().Amount);
            else if (Inventory.GetItemByCell(this).GetType() == typeof(Items.Resource))
                dropSplitWindow.SetSliderMaxValue(Inventory.GetItemByCell(this).ToResource().Amount);

            if (Inventory.GetItemByCell(this).GetType() == typeof(Equipment))
                dropSplitWindow.TurnOffSplitMenu();
        }
    }

    private void SplitItem()
    {
        Inventory.Split(this, (int)dropSplitWindow.CurrentValue);
        dropSplitWindow.gameObject.SetActive(false);
        dropSplitWindow.TurnOnSplitMenu();
        dropSplitWindow.DropBtn.onClick.RemoveListener(DropItem);
        dropSplitWindow.SplitBtn.onClick.RemoveListener(SplitItem);
    }

    private void DropItem()
    {
        Inventory.RemoveItem(this);
        dropSplitWindow.TurnOnSplitMenu();
        dropSplitWindow.gameObject.SetActive(false);
        dropSplitWindow.DropBtn.onClick.RemoveListener(DropItem);
        dropSplitWindow.SplitBtn.onClick.RemoveListener(SplitItem);
    }
}
