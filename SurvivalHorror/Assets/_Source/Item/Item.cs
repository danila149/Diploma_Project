using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private Sprite itemIcon;

    public Sprite ItemIcon => itemIcon;

    public Items.Resource ToResource() =>
        (Items.Resource)this;

    public Equipment ToEquipment() =>
        (Equipment)this;
}
