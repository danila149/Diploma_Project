using UnityEngine;

namespace Items
{
    public class Resource : Item
    {
        [SerializeField] private ResourceType resourceType;
        [SerializeField] private int amount;

        public ResourceType ResourceType => resourceType;
        public int Amount { get => amount; set => amount = value; }
    }
}