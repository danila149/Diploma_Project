using UnityEngine;

public class Food : Item
{
    [SerializeField] private int amount;
    [SerializeField] private int saturation;
    [SerializeField] private FoodType foodType;

    public FoodType FoodType => foodType;
    public bool IsEquiped { get; set; }
    public int Amount { get => amount; set => amount = value; }
    public int Saturation { get => saturation; set => saturation = value; }
}