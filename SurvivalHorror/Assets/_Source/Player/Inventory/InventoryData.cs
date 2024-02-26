using UnityEngine;

public class InventoryData : MonoBehaviour
{
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject hudUI;
    [SerializeField] private Transform dropPos;

    public Transform GridParent => gridParent;
    public GameObject InventoryUI => inventoryUI;
    public GameObject HudUI => hudUI;
    public Transform DropPos => dropPos;
}
