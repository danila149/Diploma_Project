using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private List<CraftData> craftables;
    [SerializeField] private Transform spawnpoint;

    public List<CraftData> Craftables => craftables;
    public Transform Spawnpoint => spawnpoint;

    public void ShowText(bool onOff)
    {
        text.text = $"Нажмите Е чтобы открыть верстак";
        if (onOff)
            text.gameObject.SetActive(true);
        else
            text.gameObject.SetActive(false);
    }
}
