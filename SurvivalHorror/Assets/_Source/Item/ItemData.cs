using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "SO/NewItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private ItemType type;
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private EquipmentType equipmentType;
    [SerializeField] private Sprite itemIcon;


    public ItemType Type => type;
    public ResourceType ResourceType => resourceType;
    public EquipmentType EquipmentType => equipmentType;
    public Sprite ItemIcon => itemIcon;
}