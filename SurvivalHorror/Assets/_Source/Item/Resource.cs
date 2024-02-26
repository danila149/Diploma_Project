using UnityEngine;

namespace Items
{
    public class Resource : Item
    {
        [SerializeField] private ResourceType resourceType;
        [SerializeField] private int amount;

        public ResourceType ResourceType => resourceType;
        public int Amount { get => amount; set => amount = value; }

        public override void ShowText(bool onOff)
        {
            Text.text = $"Нажмите Е чтобы подобрать {amount} {resourceType}";
            base.ShowText(onOff);
        }
    }
}