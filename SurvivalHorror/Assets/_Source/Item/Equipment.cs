using UnityEngine;

public class Equipment : Item
{
    [SerializeField] private EquipmentType equipmentType;
    [SerializeField] private int durability;
    [SerializeField] private int damage;

    public int Durability { get => durability; set => durability = value; }
    public int Damage { get => damage; set => damage = value; }

    public EquipmentType EquipmentType => equipmentType;

    public override void ShowText(bool onOff)
    {
        Text.text = $"Нажмите Е чтобы подобрать {equipmentType}";
        base.ShowText(onOff);
    }
}
