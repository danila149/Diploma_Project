using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PickUpItemInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI itemNameText;

    public void SetInfo(int amount, Sprite itemIcon, string itemName)
    {
        amountText.text = "+"+amount;
        image.sprite = itemIcon;
        itemNameText.text = itemName;
        StartCoroutine(LifeTime());
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
