using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingListItem : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI text;

    public InfoPanel InfoPanel { get; set; }
    public CraftingSystem CraftingSystem { get; set; }

    private CraftData _craftData;

    public void SetRecipe(CraftData craftData)
    {
        img.sprite = craftData.ItemPrefab.GetComponent<Item>().ItemIcon;
        text.text = craftData.ItemPrefab.name;
        _craftData = craftData;
        btn.onClick.AddListener(ShowInfoPanel);
    }

    public void ShowInfoPanel()
    {
        this.CraftingSystem.CheckForResources(_craftData);
        InfoPanel.gameObject.SetActive(true);
        Equipment equipment;
        _craftData.ItemPrefab.TryGetComponent<Equipment>(out equipment);
        if (equipment != null)
        {
            string description = "";
            description += "Durability: " + equipment.Durability + "\n" + "Damage: " + equipment.Damage + "\n \n";
            foreach (Resource item in _craftData.ResourceForCraft)
            {
                description += $"{item.amount} {item.resourceType} ";
            }
            InfoPanel.SetInfo(equipment.ItemIcon, equipment.EquipmentType.ToString(), description);
        }
        InfoPanel.Button.onClick.RemoveAllListeners();
        InfoPanel.Button.onClick.AddListener(OnCraftBtnClick);
    }

    private void OnCraftBtnClick() =>
        CraftingSystem.Craft(this);
}