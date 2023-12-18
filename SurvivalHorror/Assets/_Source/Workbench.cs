using TMPro;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;

    public void ShowText(bool onOff)
    {
        text.text = $"Нажмите Е чтобы рикрыть верстак";
        if (onOff)
            text.gameObject.SetActive(true);
        else
            text.gameObject.SetActive(false);
    }
}
