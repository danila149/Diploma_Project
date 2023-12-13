using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "SO/NewItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private ItemType type;
    [SerializeField] private Sprite itemIcon;

    public ItemType Type => type;
    public Sprite ItemIcon => itemIcon;
}
