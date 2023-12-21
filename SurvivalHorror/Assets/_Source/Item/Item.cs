using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private Sprite itemIcon;

    public TextMeshPro Text => text;
    public Sprite ItemIcon => itemIcon;

    public Items.Resource ToResource() =>
        (Items.Resource)this;

    public Equipment ToEquipment() =>
        (Equipment)this;

    public virtual void ShowText(bool onOff)
    {
        if (onOff)
            text.gameObject.SetActive(true);
        else
            text.gameObject.SetActive(false);
    }
}
