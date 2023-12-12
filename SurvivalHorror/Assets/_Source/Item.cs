using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private GameObject text;

    public void ShowText(bool onOff)
    {
        if (onOff)
            text.SetActive(true);
        else
            text.SetActive(false);
    }
}
