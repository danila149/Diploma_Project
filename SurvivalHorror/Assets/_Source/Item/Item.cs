using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private ItemData data;
    [SerializeField] private int amount;

    public ItemData Data { get => data; set => data = value; }
    public int Amount { get => amount; set => amount = value; }


    public void ShowText(bool onOff)
    {
        text.text = $"Нажмите Е чтобы подобрать {amount} {data.Type}";
        if (onOff)
            text.gameObject.SetActive(true);
        else
            text.gameObject.SetActive(false);
    }
}
