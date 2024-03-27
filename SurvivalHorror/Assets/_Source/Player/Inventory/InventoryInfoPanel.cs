using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryInfoPanel : MonoBehaviour
{
    [SerializeField] private Image itemImg;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI description;

    public void SetInfo(Sprite itemIcon, string itemName, string description)
    {
        itemImg.sprite = itemIcon;
        this.itemName.text = itemName;
        this.description.text = description;
    }
}
