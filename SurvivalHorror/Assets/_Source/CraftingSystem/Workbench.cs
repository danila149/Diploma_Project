using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private List<CraftData> craftables;

    public List<CraftData> Craftables => craftables;

    public void ShowText(bool onOff)
    {
        text.text = $"Нажмите Е чтобы открыть верстак";
        if (onOff)
            text.gameObject.SetActive(true);
        else
            text.gameObject.SetActive(false);
    }
}
