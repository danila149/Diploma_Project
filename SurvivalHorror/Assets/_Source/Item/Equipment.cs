using UnityEngine;

public class Equipment : Item
{
    [SerializeField] private EquipmentType equipmentType;
    [SerializeField] private int durability;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask layerMask;

    public int Durability { get => durability; set => durability = value; }
    public int Damage { get => damage; set => damage = value; }
    public bool IsEquiped { get; set; }

    public EquipmentType EquipmentType => equipmentType;
    public LayerMask LayerMask => layerMask;
}
