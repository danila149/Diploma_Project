using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Image cellImage;
    [SerializeField] private TextMeshProUGUI itemCountText;
    [SerializeField] private LayerMask dropZoneLayerMask;
    [SerializeField] private LayerMask cellLayerMask;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private bool hotbar;

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

}
